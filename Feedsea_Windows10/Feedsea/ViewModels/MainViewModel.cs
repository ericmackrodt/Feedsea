using Broadcaster;
using Feedsea.Common;
using Feedsea.Common.Events;
using Feedsea.Common.Helpers;
using Feedsea.Common.Providers;
using Feedsea.Common.Providers.Data;
using Feedsea.Common.Services;
using Feedsea.Models;
using Feedsea.Settings;
using MVVMBasic;
using MVVMBasic.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Feedsea.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private IAuthenticationProvider authProvider;
        private INewsSourceProvider sourceProvider;
        private IShareService share;
        private IGeneralSettings generalSettings;
        private IBroadcaster broadcaster;

        private INewsSource selectedSource;
        public INewsSource SelectedSource
        {
            get { return selectedSource; }
            set
            {
                if (selectedSource != value)
                {
                    selectedSource = value;
                    NotifyChanged();
                }
            }
        }
        
        private ObservableCollection<INewsSource> sources;
        public ObservableCollection<INewsSource> Sources
        {
            get { return sources; }
            set
            {
                if (sources != value)
                {
                    sources = value;
                    NotifyChanged();
                }
            }
        }
        
        private ObservableCollection<ArticleData> articles;
        public ObservableCollection<ArticleData> Articles
        {
            get { return articles; }
            set
            {
                if (articles != value)
                {
                    articles = value;
                    NotifyChanged();
                }
            }
        }

        private ICommand selectSourceCommand;
        public ICommand SelectSourceCommand
        {
            get { return selectSourceCommand; }
        }

        private ICommand shareArticleCommand;
        public ICommand ShareArticleCommand
        {
            get { return shareArticleCommand; }
        }

        private ICommand toggleArticleReadCommand;
        public ICommand ToggleArticleReadCommand
        {
            get { return toggleArticleReadCommand; }
        }

        private ICommand toggleArticleSavedCommand;
        public ICommand ToggleArticleSavedCommand
        {
            get { return toggleArticleSavedCommand; }
        }

        private ICommand refreshNewsCommand;
        public ICommand RefreshNewsCommand
        {
            get { return refreshNewsCommand; }
        }
        
        public MainViewModel(
            IAuthenticationProvider authProvider, 
            INewsSourceProvider sourceProvider,
            IShareService share, 
            IGeneralSettings generalSettings,
            IBroadcaster broadcaster)
        {
            this.authProvider = authProvider;
            this.sourceProvider = sourceProvider;
            this.share = share;
            this.generalSettings = generalSettings;
            this.broadcaster = broadcaster;

            selectSourceCommand = new RelayCommandAsync<INewsSource>(o => ConnectionVerifier.Verify(SelectSource, o, OnCommandFail));
            shareArticleCommand = new RelayCommandAsync<ArticleData>(o => ConnectionVerifier.Verify(ShareArticle, o, OnCommandFail));
            toggleArticleReadCommand = new RelayCommandAsync<ArticleData>(o => ConnectionVerifier.Verify(ToggleArticleRead, o, OnCommandFail));
            toggleArticleSavedCommand = new RelayCommandAsync<ArticleData>(o => ConnectionVerifier.Verify(ToggleArticleSaved, o, OnCommandFail));
            refreshNewsCommand = new RelayCommandAsync(o => ConnectionVerifier.Verify(RefreshNews, o, OnCommandFail));
        }

        private void OnCommandFail()
        {
            IsBusy = false;
        }

        private async Task RefreshNews(object arg)
        {
            IsBusy = true;

            if (Articles != null && Articles.Any())
                Articles.Clear();

            //var result = await authProvider.Refresh(SelectedSource);

            throw new Exception("FIX THIS");
            //if (result != null)
            //{
            //    Sources = result.Sources;
            //    Articles = result.Articles;
            //}

            IsBusy = false;
        }

        

        private async Task ToggleArticleSaved(ArticleData article)
        {
            //if (article.IsFavorite)
            //    await authProvider.RemoveFromSaved(article);
            //else
            //    await authProvider.SaveArticleForLater(article);
        }

        private async Task ToggleArticleRead(ArticleData article)
        {
            //if (article.IsRead)
            //    await authProvider.UnmarkArticleRead(article);
            //else
            //    await authProvider.MarkArticleRead(article);

            //if (article.Source != null)
            //    ChangeUnreadNumber(article.IsRead, article.Source.UrlID);
        }

        private async Task ShareArticle(ArticleData arg)
        {
            //throw new NotImplementedException();
            //share.Share(arg);
        }

        private async Task SelectSource(INewsSource source)
        {
            IsBusy = true;

            if (Articles != null && Articles.Any())
                Articles.Clear();

            var menuItem = source as MenuItem;
            //if (menuItem != null && menuItem.IsMostEngaging)
            //    Articles = await _provider.LoadMostEngagingArticles(source);
            //else
            //    Articles = await _provider.LoadArticles(source);

            SelectedSource = source;

            IsBusy = false;
        }

        public override async Task LoadData(object arg)
        {
            IsBusy = true;

            await authProvider.Initialization();

            var byId = true;

            //if (string.IsNullOrWhiteSpace(open))
            //    open = _generalSettings.CategoryToLoadSetting;

            var sources = await sourceProvider.LoadNewsSources();
            Sources = new ObservableCollection<INewsSource>(sources);

            //SelectedSource = null;

            //INewsSource source = null;

            //if (byId)
            //    source = GetSourceById((string)arg);
            //else
            //    source = GetSourceByName((string)arg);

            //var result = await _provider.Refresh(source);

            var result = await sourceProvider.DownloadNewsSources(Sources);

            if (!result.Key)
                Sources = new ObservableCollection<INewsSource>(result.Value);

            await CheckFirstLoad();

            IsBusy = false;
            IsDataLoaded = true;
        }

        private INewsSource GetSourceByName(string query)
        {
            if (string.IsNullOrWhiteSpace(query) || Sources == null) return null;

            INewsSource source = Sources.FirstOrDefault(o => o.Name.ToLower().Contains(query.ToLower()));

            if (source == null)
                source = Sources.FirstOrDefault(o => o.Name.ToLower().Contains(query.ToLower()));

            return source;
        }

        private INewsSource GetSourceById(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || Sources == null) return null;

            INewsSource source = Sources.FirstOrDefault(o => o.UrlID == WebUtility.UrlDecode(id));

            if (source == null)
                source = Sources.FirstOrDefault(o => o.UrlID == WebUtility.UrlDecode(id));
            
            return source;
        }

        private void ChangeUnreadNumber(bool isRead, string id)
        {
            var src = Sources.FirstOrDefault(o => o.UrlID == id) as SubscriptionData;

            if (src == null) return;

            if (isRead)
                src.UnreadNumber--;
            else
                src.UnreadNumber++;
        }

        private async Task CheckFirstLoad()
        {
            //if ((Sources == null || !Sources.Any()) && _generalSettings.FirstLoadSetting)
            //{
            //    var result = await _messageBox.ShowAsync(AppResources.Msg_Greetings_Message, AppResources.Msg_Greetings_Title, new string[] { AppResources.AddSource, AppResources.Cancel });

            //    if (result == 0 && AddSource != null)
            //        AddSource(this, new EventArgs());
            //}
            //else
            //    _generalSettings.FirstLoadSetting = false;
        }
    }
}
