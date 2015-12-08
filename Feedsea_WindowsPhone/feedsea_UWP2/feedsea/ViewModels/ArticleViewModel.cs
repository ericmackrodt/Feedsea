using feedsea.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using feedsea.Resources;
using System.Net;
using feedsea.Common.Providers;
using feedsea.Common.MVVM;
using feedsea.Common.MVVM.Commands;
using feedsea.Common.Helpers;
using Microsoft.Phone.BackgroundAudio;
using feedsea.Settings;
using feedsea.Common.Providers.Feedly;
using System.Runtime.Serialization;
using feedsea.Common.Controls;
using feedsea.Services;
using feedsea.Common.Providers.Data;
using feedsea.Common.Providers.MobilizerProvider;

namespace feedsea.ViewModels
{

   [DataContract]
   public class ArticleViewModel
      : AViewModel<ArticleViewModel>
   {
      private IGeneralSettings _generalSettings;
      private IThirdPartySettings _thirdPartySettings;
      private INewsService _service;
      private IArticleHtmlBuilder _articleHtml;
      private IMobilizerProvider _contentDownloader;

#region Public_Properties
      public bool IsAdsEnabled
      {
         get
         {
            return !_generalSettings.IsAdsDisabledSetting;
         }
      }

      private bool canLoadNext;

      public bool CanLoadNext
      {
         get
         {
            return canLoadNext;
         }
         set
         {
            canLoadNext = value;
            NotifyChanged(o => o.CanLoadNext);
         }
      }

      private bool canLoadPrevious;

      public bool CanLoadPrevious
      {
         get
         {
            return canLoadPrevious;
         }
         set
         {
            canLoadPrevious = value;
            NotifyChanged(o => o.CanLoadPrevious);
         }
      }

      private INewsSource selectedSource;

      [DataMember]
      public INewsSource SelectedSource
      {
         get
         {
            return selectedSource;
         }
         set
         {
            selectedSource = value;
         }
      }

      private ArticleData article;

      [DataMember]
      public ArticleData Article
      {
         get
         {
            return article;
         }
         set
         {
            article = value;
            NotifyChanged(o => o.Article);
         }
      }

      private IEnclosure audioFile;

      [DataMember]
      public IEnclosure AudioFile
      {
         get
         {
            return audioFile;
         }
         set
         {
            audioFile = value;
            NotifyChanged(o => o.AudioFile);
         }
      }

      private bool isPlaying;

      [DataMember]
      public bool IsPlaying
      {
         get
         {
            return isPlaying;
         }
         set
         {
            isPlaying = value;
            NotifyChanged(o => o.IsPlaying);
         }
      }
#endregion Public_Properties

#region Commands
      private ICommand toggleReadCommand;

      public ICommand ToggleReadCommand
      {
         get
         {
            return toggleReadCommand;
         }
      }

      private ICommand toggleFavoriteCommand;

      public ICommand ToggleFavoriteCommand
      {
         get
         {
            return toggleFavoriteCommand;
         }
      }

      private ICommand playArticleAudioCommand;

      public ICommand PlayArticleAudioCommand
      {
         get
         {
            return playArticleAudioCommand;
         }
      }

      private ICommand loadNextArticleCommand;

      public ICommand LoadNextArticleCommand
      {
         get
         {
            return loadNextArticleCommand;
         }
      }

      private ICommand loadPreviousArticleCommand;

      public ICommand LoadPreviousArticleCommand
      {
         get
         {
            return loadPreviousArticleCommand;
         }
      }
#endregion Commands


#region Constructor
      public ArticleViewModel()
      {
         toggleReadCommand = new AsyncDelegateCommand(ToggleRead);
         toggleFavoriteCommand = new AsyncDelegateCommand(ToggleFavorite);
         playArticleAudioCommand = new RelayCommand(PlayArticleAudio)
            {
               IsEnabled = true
            };
         loadNextArticleCommand = new AsyncDelegateCommand(LoadNextArticle);
         loadPreviousArticleCommand = new AsyncDelegateCommand(LoadPreviousArticle);
         BackgroundAudioPlayer.Instance.PlayStateChanged += Instance_PlayStateChanged;
      }

      public ArticleViewModel(IConnectionVerify connectionVerify, INewsService newsService, IGeneralSettings appSettings, IArticleHtmlBuilder htmlBuilder, IMobilizerProvider mobilizerProvider, IThirdPartySettings thirdPartySettings)
      : this()
      {
         _generalSettings = appSettings;
         _service = newsService;
         connection = connectionVerify;
         _articleHtml = htmlBuilder;
         _contentDownloader = mobilizerProvider;
         _thirdPartySettings = thirdPartySettings;
      }
#endregion Constructor

      public async Task LoadArticle(ArticleData article)
      {
         if ( article == null )
         {
            if ( IsBusy )
               IsBusy = false;
            return ;
         }
         if ( !IsBusy )
            IsBusy = true;
         if ( !article.IsDataLoaded )
         {
            if ( string.IsNullOrWhiteSpace(article.Content) && _generalSettings.DownloadArticleIfNoContentSetting )
            {
               article.Content = await ConnectionVerifyCall(async () =>
                     {
                        return await _contentDownloader.GetMobilized(article.URL);
                     });
            }
            article.Content = await _articleHtml.BuildHtml(article, _thirdPartySettings.YoutubeClientSetting);
         }
         if ( _service.Articles != null && _service.Articles.Any() )
         {
            if ( Article.UniqueID == _service.Articles.Last().UniqueID )
               await _service.LoadMoreNews();
            CanLoadNext = Article.UniqueID != _service.Articles.Last().UniqueID;
            CanLoadPrevious = Article.UniqueID != _service.Articles.First().UniqueID;
         }
         if ( article.Enclosure != null && article.Enclosure.Any(o => o.Type.ToLower() == "audio/mpeg") )
            AudioFile = article.Enclosure.FirstOrDefault(o => o.Type.ToLower() == "audio/mpeg");
         else
            AudioFile = null;
         if ( AudioFile != null )
            UpdatePlayerState();
         IsBusy = false;
         article.IsDataLoaded = true;
         if ( !article.IsRead && _generalSettings.MarkArticlesReadWhenOpenedSetting )
            await ConnectionVerifyCall(async () =>
               {
                  await ToggleRead(article);
               });
      }

      public async Task LoadDataAsync(string articleId)
      {
         IsBusy = true;
         Article = _service.GetArticle(articleId);
         SelectedSource = _service.SelectedSource;
         await LoadArticle(Article);
         IsDataLoaded = true;
      }

      public void SetMobilizer(Mobilizers e)
      {
         switch ( e )
         {
            case Mobilizers.Instapaper:
               Article.MobilizedUrl = string.Format("http://mobilizer.instapaper.com/m?u={0}", Article.URL);
               break;
            case Mobilizers.Readability:
               Article.MobilizedUrl = string.Format("http://www.readability.com/m?url={0}", Article.URL);
               break;
            case Mobilizers.Google:
               Article.MobilizedUrl = string.Format("http://www.google.com/gwt/x?u={0}&noimg=0&btnGo=Go&source=wax&ie=UTF-8&oe=UTF-8", Article.URL);
               break;
            case Mobilizers.GoogleNoImg:
               Article.MobilizedUrl = string.Format("http://www.google.com/gwt/x?u={0}&noimg=1&btnGo=Go&source=wax&ie=UTF-8&oe=UTF-8", Article.URL);
               break;
            case Mobilizers.Page:
            default:
               Article.MobilizedUrl = Article.URL;
               break;
         }
      }

      private void Instance_PlayStateChanged(object sender, EventArgs e)
      {
         if ( AudioFile != null )
            UpdatePlayerState();
      }

      private void UpdatePlayerState()
      {
         if ( BackgroundAudioPlayer.Instance.Track != null && BackgroundAudioPlayer.Instance.Track.Source.ToString() == AudioFile.Href )
         {
            IsPlaying = BackgroundAudioPlayer.Instance.PlayerState == PlayState.Playing;
         }
         else
         {
            IsPlaying = false;
         }
      }

      private async Task ToggleRead(object arg)
      {
         if ( !connection.HasInternetConnection() )
            return ;
         try
         {
            await _service.ToggleRead(arg as ArticleData ?? Article);
         }
         catch ( Exception ex )
         {
            connection.VerifyConnectionException(ex);
         }
      }

      private async Task ToggleFavorite(object arg)
      {
         if ( !connection.HasInternetConnection() )
            return ;
         try
         {
            await _service.ToggleSaved(Article);
         }
         catch ( Exception ex )
         {
            connection.VerifyConnectionException(ex);
         }
      }

      private async Task LoadPreviousArticle(object arg)
      {
         if ( _service == null || _service.Articles == null || !_service.Articles.Any() )
            return ;
         var previousArticle = _service.Articles.TakeWhile(o => o.UniqueID != Article.UniqueID).LastOrDefault();
         if ( previousArticle == null )
            return ;
         Article = previousArticle;
         await LoadArticle(Article);
      }

      private async Task LoadNextArticle(object arg)
      {
         if ( _service == null || _service.Articles == null || !_service.Articles.Any() )
            return ;
         var nextArticle = _service.Articles.SkipWhile(o => o.UniqueID != Article.UniqueID).Skip(1).FirstOrDefault();
         if ( nextArticle == null )
            return ;
         Article = nextArticle;
         await LoadArticle(Article);
      }

      private void PlayArticleAudio()
      {
         if ( BackgroundAudioPlayer.Instance.Track != null && BackgroundAudioPlayer.Instance.Track.Source.ToString() != AudioFile.Href )
            BackgroundAudioPlayer.Instance.Close();
         if ( BackgroundAudioPlayer.Instance.PlayerState == PlayState.Playing )
         {
            BackgroundAudioPlayer.Instance.Pause();
            return ;
         }
         BackgroundAudioPlayer.Instance.Track = new AudioTrack(new Uri(AudioFile.Href), Article.Title, Article.Source.Name, null, article.MainImageUrl != null ? new Uri(article.MainImageUrl) : null);
         BackgroundAudioPlayer.Instance.Play();
      }

      public void Unload()
      {
         BackgroundAudioPlayer.Instance.PlayStateChanged -= Instance_PlayStateChanged;
      }


#if DEBUG
      ~ ArticleViewModel()
      {
         System.Diagnostics.Debug.WriteLine("ArticleViewModel was killed!");
      }
      #endif
   }

}