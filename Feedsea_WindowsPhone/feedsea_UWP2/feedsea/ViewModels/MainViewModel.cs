using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using feedsea.Resources;
using feedsea.Common;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using Cimbalino.Phone.Toolkit.Services;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Messaging;
using System.Net;
using feedsea.Common.Providers;
using feedsea.Common.MVVM;
using feedsea.Common.MVVM.Commands;
using feedsea.BackgroundAgent.Common;
using feedsea.Settings;
using feedsea.Common.Providers.Pocket;
using System.Runtime.Serialization;
using feedsea.Common.Providers.Feedly;
using feedsea.Common.Controls;
using feedsea.Services;
using feedsea.Common.Providers.Data;
using feedsea.Models;

namespace feedsea.ViewModels
{

   public class MainViewModel
      : AViewModel<MainViewModel>
   {
#region Events
      public event EventHandler SelectedSourceLoading;
      public event EventHandler AddSource;
#endregion Events
#region Members
      private IGeneralSettings _generalSettings;
      private IAppearanceSettings _appearanceSettings;
      private IMessageBoxService _messageBox;
      private INewsService _newsService;
      private ITilePin _tilePinner;
      private IPaidFeatures _paidFeatures;
      private IFullLoadingService _loadingService;
#endregion Members

#region Properties
      [DataMember]
      public override bool IsBusy
      {
         get
         {
            return base.IsBusy;
         }
         set
         {
            base.IsBusy = value;
            ToggleCommandsEnabled(!base.IsBusy);
         }
      }

      private bool paginationBusy;

      [DataMember]
      public bool PaginationBusy
      {
         get
         {
            return paginationBusy;
         }
         set
         {
            paginationBusy = value;
            NotifyChanged(o => o.PaginationBusy);
         }
      }

      [DataMember]
      public INewsSource SelectedSource
      {
         get
         {
            return _newsService.SelectedSource;
         }
         set
         {
            _newsService.SelectedSource = value;
         }
      }

      [IgnoreDataMember]
      public ObservableCollection<ArticleData> Articles
      {
         get
         {
            return _newsService.Articles;
         }
         set
         {
            _newsService.Articles = value;
         }
      }

      [IgnoreDataMember]
      public ObservableCollection<CategoryData> Sources
      {
         get
         {
            return _newsService.Sources;
         }
         set
         {
            _newsService.Sources = value;
         }
      }

      [IgnoreDataMember]
      public override bool IsDataLoaded
      {
         get
         {
            return base.IsDataLoaded;
         }
         set
         {
            base.IsDataLoaded = value;
         }
      }

      [IgnoreDataMember]
      public bool IsAdsEnabled
      {
         get
         {
            return !_generalSettings.IsAdsDisabledSetting;
         }
         set
         {
            _generalSettings.IsAdsDisabledSetting = !value;
            NotifyChanged(o => o.IsAdsEnabled);
         }
      }

      [IgnoreDataMember]
      public ArticleTemplateType ArticleTemplateType
      {
         get
         {
            return _appearanceSettings.ArticleItemsTemplateTypeSetting;
         }
         set
         {
            _appearanceSettings.ArticleItemsTemplateTypeSetting = value;
            NotifyChanged(o => o.ArticleTemplateType);
         }
      }
#endregion Properties

#region Commands
      private ICommand refreshCommand;

      public ICommand RefreshCommand
      {
         get
         {
            return refreshCommand;
         }
      }

      private ICommand loadMoreItemsCommand;

      public ICommand LoadMoreItemsCommand
      {
         get
         {
            return loadMoreItemsCommand;
         }
      }

      private ICommand addToFavoritesCommand;

      public ICommand AddToFavoritesCommand
      {
         get
         {
            return addToFavoritesCommand;
         }
      }

      private ICommand toggleArticleReadCommand;

      public ICommand ToggleArticleReadCommand
      {
         get
         {
            return toggleArticleReadCommand;
         }
      }

      private ICommand removeSourceCommand;

      public ICommand RemoveSourceCommand
      {
         get
         {
            return removeSourceCommand;
         }
      }

      private ICommand markReadCommand;

      public ICommand MarkReadCommand
      {
         get
         {
            return markReadCommand;
         }
      }

      private ICommand selectSourceCommand;

      public ICommand SelectSourceCommand
      {
         get
         {
            return selectSourceCommand;
         }
      }

      private ICommand removeCategoryCommand;

      public ICommand RemoveCategoryCommand
      {
         get
         {
            return removeCategoryCommand;
         }
      }

      private ICommand pinToStartCommand;

      public ICommand PinToStartCommand
      {
         get
         {
            return pinToStartCommand;
         }
      }

      private ICommand markArticleReadCommand;

      public ICommand MarkArticleReadCommand
      {
         get
         {
            return markArticleReadCommand;
         }
      }

      private ICommand markMultipleArticlesReadCommand;

      public ICommand MarkMultipleArticlesReadCommand
      {
         get
         {
            return markMultipleArticlesReadCommand;
         }
      }

      private ICommand loadSavedArticlesCommand;

      public ICommand LoadSavedArticlesCommand
      {
         get
         {
            return loadSavedArticlesCommand;
         }
      }

      private ICommand disableAdsCommand;

      public ICommand DisableAdsCommand
      {
         get
         {
            return disableAdsCommand;
         }
      }

      private ICommand loadAllArticlesCommand;

      public ICommand LoadAllArticlesCommand
      {
         get
         {
            return loadAllArticlesCommand;
         }
      }
#endregion Commands


#region Constructors
      public MainViewModel()
      {
         //Commands Startup
         AssignCommands();
         //Setup Messenger
         Messenger.Default.Register<GenericMessage<MessageContent>>(this, MessageCallback);
      }

      public MainViewModel(INewsService newsService, IMessageBoxService messageBoxService, IConnectionVerify connectionVerify, IGeneralSettings generalSettings, IAppearanceSettings appearanceSettings, ITilePin pinner, IPaidFeatures paidFeatures, IFullLoadingService loadingService)
      : this()
      {
         _newsService = newsService;
         _messageBox = messageBoxService;
         connection = connectionVerify;
         _generalSettings = generalSettings;
         _appearanceSettings = appearanceSettings;
         _tilePinner = pinner;
         _paidFeatures = paidFeatures;
         _loadingService = loadingService;
         _newsService.PropertyChanged += NewsService_PropertyChanged;
         _newsService.LoadingChanged += NewsService_LoadingChanged;
      }
#endregion Constructors

#region Startup_Methods
      private void AssignCommands()
      {
         this.loadMoreItemsCommand = new AsyncDelegateCommand(o => ConnectionVerifyCall(LoadItemsPagination, o, OnConnectionFail));
         this.refreshCommand = new AsyncDelegateCommand(o => ConnectionVerifyCall(RefreshNews, o, OnConnectionFail));
         this.loadAllArticlesCommand = new AsyncDelegateCommand(o => ConnectionVerifyCall(LoadAllArticles, o, OnConnectionFail));
         this.addToFavoritesCommand = new AsyncDelegateCommand<ArticleData>(o => ConnectionVerifyCall(AddArticleToFavorites, o, OnConnectionFail));
         this.toggleArticleReadCommand = new AsyncDelegateCommand<ArticleData>(o => ConnectionVerifyCall(ToggleArticleRead, o, OnConnectionFail));
         this.removeSourceCommand = new AsyncDelegateCommand<SubscriptionData>(o => ConnectionVerifyCall(RemoveSource, o, OnConnectionFail));
         this.markReadCommand = new AsyncDelegateCommand<INewsSource>(o => ConnectionVerifyCall(MarkRead, o, OnConnectionFail));
         this.selectSourceCommand = new AsyncDelegateCommand<INewsSource>(o => ConnectionVerifyCall(SelectSource, o, OnConnectionFail));
         this.removeCategoryCommand = new AsyncDelegateCommand<CategoryData>(o => ConnectionVerifyCall(RemoveCategory, o, OnConnectionFail));
         this.pinToStartCommand = new AsyncDelegateCommand<INewsSource>(o => ConnectionVerifyCall(PinToStart, o, OnConnectionFail));
         this.markArticleReadCommand = new AsyncDelegateCommand<ArticleData>(o => ConnectionVerifyCall(MarkArticleRead, o, OnConnectionFail));
         this.markMultipleArticlesReadCommand = new AsyncDelegateCommand<IEnumerable<ArticleData>>(MarkMultipleArticlesRead);
         this.loadSavedArticlesCommand = new AsyncDelegateCommand(o => ConnectionVerifyCall(LoadSavedArticles, o, OnConnectionFail));
         this.disableAdsCommand = new AsyncDelegateCommand(DisableAds);
      }

      public async Task<bool> Authenticate()
      {
         return await ConnectionVerifyCall(async () => await _newsService.Authenticate());
      }

      public async Task LoadDataAsync(string open = null)
      {
         if ( IsAdsEnabled )
         {
            IsAdsEnabled = !_paidFeatures.CheckProductPurchase(_paidFeatures.DisableAdsProduct);
         }
         await ConnectionVerifyCall(async () =>
            {
               if ( string.IsNullOrWhiteSpace(open) )
                  open = _generalSettings.CategoryToLoadSetting;
               await _newsService.Initialize(open);
               await CheckFirstLoad();
               IsDataLoaded = true;
            });
      }

      public async Task SemanticLoadDataAsync(string query)
      {
         if ( _generalSettings.FirstLoadSetting )
         {
            await LoadDataAsync();
            return ;
         }
         await ConnectionVerifyCall(async () =>
            {
               await _newsService.Initialize(query, false);
               IsDataLoaded = true;
            });
      }
#endregion Startup_Methods

#region Command_Methods
      private async Task RefreshNews(object arg)
      {
         await _newsService.RefreshNews();
         await CheckFirstLoad();
      }

      private async Task LoadAllArticles(object arg)
      {
         var task = _newsService.ClearSelectedSource();
         if ( SelectedSourceLoading != null )
            SelectedSourceLoading(this, new EventArgs());
         await task;
      }

      private async Task LoadItemsPagination(object arg)
      {
         if ( PaginationBusy || IsBusy || !Articles.Any() )
            return ;
         PaginationBusy = true;
         await _newsService.LoadMoreNews();
         PaginationBusy = false;
      }

      private async Task ToggleArticleRead(ArticleData obj)
      {
         if ( IsBusy || obj == null )
            return ;
         await _newsService.ToggleRead(obj);
      }

      private async Task AddArticleToFavorites(ArticleData obj)
      {
         await _newsService.ToggleSaved(obj);
      }

      private async Task MarkRead(INewsSource arg)
      {
         if ( IsBusy )
            return ;
         if ( _generalSettings.AskConfirmationMarkReadSetting )
         {
            var result = _messageBox.Show(AppResources.MSG_MarkCurrentArticlesReadConfirm, AppResources.Attention, MessageBoxButton.OKCancel);
            if ( result != 10 )
               return ;
         }
         await _newsService.MarkRead(arg);
      }

      private async Task RemoveSource(SubscriptionData arg)
      {
         if ( PaginationBusy || IsBusy )
            return ;
         var result = _messageBox.Show(AppResources.RemoveSourceConfirmation, AppResources.Remove, MessageBoxButton.OKCancel);
         if ( result != 10 )
            return ;
         await _newsService.RemoveSource(arg);
      }

      private async Task SelectSource(INewsSource arg)
      {
         var selectTask = _newsService.LoadSource(arg);
         if ( SelectedSourceLoading != null )
            SelectedSourceLoading(this, new EventArgs());
         await selectTask;
      }

      private async Task RemoveCategory(CategoryData arg)
      {
         var result = await _messageBox.ShowAsync(AppResources.MSG_RemoveCategoryConfirmation, AppResources.Attention, new string[] { AppResources.Yes, AppResources.No });
         if ( result == 1 )
            return ;
         await _newsService.RemoveCategory(arg);
      }

      private async Task PinToStart(INewsSource obj)
      {
         _loadingService.StartLoading(AppResources.Msg_PinningLiveTile);
         var isCategory = false;
         var title = obj.Name;
         if ( obj is ICategory )
         {
            isCategory = true;
            var name = Resources.AppResources.ResourceManager.GetString(obj.Name);
            if ( !string.IsNullOrWhiteSpace(name) )
               title = name;
         }
         var tileData = new LiveTileData()
            {
               NavigationUri = new Uri(new Uri("ms-appx://"), "/Views/MainPage.xaml?open=" + System.Net.WebUtility.UrlEncode(obj.UrlID)), Type = isCategory ? TileType.Category : TileType.Subscription, Title = title
            };
         var articles = await _newsService.GetTileArticles(obj);
         var firstArticle = articles.First();
         var secondArticle = articles.Last();
         tileData.Front = new TileFace()
            {
               SourceID = obj.UrlID, SourceName = firstArticle.Source.Name, Title = firstArticle.Title, Image = firstArticle.MainImageUrl, Favicon = firstArticle.Source.Link
            };
         tileData.Back = new TileFace()
            {
               SourceID = obj.UrlID, SourceName = secondArticle.Source.Name, Title = secondArticle.Title, Image = secondArticle.MainImageUrl, Favicon = secondArticle.Source.Link
            };
         var result = await _tilePinner.PinTile(tileData);
         _loadingService.EndLoading();
         if ( !result )
            _messageBox.Show(AppResources.Msg_AlreadyPinned);
      }

      private async Task MarkArticleRead(ArticleData arg)
      {
         if ( !arg.IsRead && _generalSettings.MarkReadOnFeedScrollSetting )
         {
            arg.IsRead = true;
            await ToggleArticleRead(arg);
         }
      }

      private async Task MarkMultipleArticlesRead(IEnumerable<ArticleData> arg)
      {
         if ( IsBusy || arg == null || !arg.Any() || !_generalSettings.MarkReadOnFeedScrollSetting )
            return ;
         try
         {
            await _newsService.MarkMultipleArticlesRead(arg);
         }
         catch ( ProviderException )
         {
         }
         catch ( TileCreationException )
         {
         }
         catch ( TaskCanceledException )
         {
         }
      }

      private async Task LoadSavedArticles(object arg)
      {
         var task = _newsService.LoadSavedArticles();
         if ( SelectedSourceLoading != null )
            SelectedSourceLoading(this, new EventArgs());
         await task;
      }

      private async Task DisableAds(object arg)
      {
         //await paidFeatures.Update();
         //if (!paidFeatures.IsAdsDisabled)
         var result = await _paidFeatures.BuyFeature(_paidFeatures.DisableAdsProduct);
         if ( result == LicenseStatus.AlreadyActivated )
         {
            _messageBox.Show(AppResources.Purchase_AlreadyPurchased);
            IsAdsEnabled = false;
         }
         else if ( result == LicenseStatus.Purchased )
         {
            _messageBox.Show(AppResources.Purchase_Success);
            IsAdsEnabled = false;
         }
         else if ( result == LicenseStatus.NotPurchased )
         {
            _messageBox.Show(AppResources.Purchase_Canceled);
            IsAdsEnabled = true;
         }
      }
#endregion Command_Methods

#region Private_Methods
      private async Task CheckFirstLoad()
      {
         if ( (Sources == null || !Sources.Any()) && _generalSettings.FirstLoadSetting )
         {
            var result = await _messageBox.ShowAsync(AppResources.Msg_Greetings_Message, AppResources.Msg_Greetings_Title, new string[] { AppResources.AddSource, AppResources.Cancel });
            if ( result == 0 && AddSource != null )
               AddSource(this, new EventArgs());
         }
         else
            _generalSettings.FirstLoadSetting = false;
      }

      private void ToggleCommandsEnabled(bool value)
      {
         (refreshCommand as AsyncDelegateCommand).IsEnabled = value;
         (toggleArticleReadCommand as AsyncDelegateCommand<ArticleData>).IsEnabled = value;
         (markReadCommand as AsyncDelegateCommand<INewsSource>).IsEnabled = value;
      }

      [Obsolete]
      private async void MessageCallback(GenericMessage<MessageContent> obj)
      {
         IsBusy = true;
         if ( obj.Content.Type == MessageContentType.UpdateSources )
         {
            await ConnectionVerifyCall(async () => await RefreshNews(null), OnConnectionFail);
         }
         if ( obj.Content.Type == MessageContentType.SettingsUpdated )
         {
            NotifyChanged((string)obj.Content.Content);
         }
         if ( obj.Content.Type == MessageContentType.Logoff )
         {
            IsDataLoaded = false;
         }
         IsBusy = false;
      }

      private void OnConnectionFail()
      {
         IsBusy = false;
         PaginationBusy = false;
         if ( _loadingService.IsLoading )
            _loadingService.EndLoading();
      }

      private void NewsService_PropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         NotifyChanged(e.PropertyName);
      }

      private void NewsService_LoadingChanged(object sender, bool e)
      {
         IsBusy = e;
      }
#endregion Private_Methods

   }

}