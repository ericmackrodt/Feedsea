using Feedsea.Common;
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
        private INewsProvider _provider;
        private IShareService _share;
        private IGeneralSettings _generalSettings;
        private IMessageBoxService _messageBox;

        public ArticleViewTemplateEnum ArticleViewTemplate
        {
            get { return _generalSettings.ArticleListTemplate; }
            set
            {
                if (_generalSettings.ArticleListTemplate != value)
                {
                    _generalSettings.ArticleListTemplate = value;
                    NotifyChanged();
                }
            }
        }

        private INewsSource _selectedSource;
        public INewsSource SelectedSource
        {
            get { return _selectedSource; }
            set
            {
                if (_selectedSource != value)
                {
                    _selectedSource = value;
                    NotifyChanged();
                }
            }
        }
        
        private ObservableCollection<INewsSource> _sources;
        public ObservableCollection<INewsSource> Sources
        {
            get { return _sources; }
            set
            {
                if (_sources != value)
                {
                    _sources = value;
                    NotifyChanged();
                }
            }
        }
        
        private ObservableCollection<ArticleData> _articles;
        public ObservableCollection<ArticleData> Articles
        {
            get { return _articles; }
            set
            {
                if (_articles != value)
                {
                    _articles = value;
                    NotifyChanged();
                }
            }
        }

        private ICommand _selectSourceCommand;
        public ICommand SelectSourceCommand
        {
            get { return _selectSourceCommand; }
        }

        private ICommand _shareArticleCommand;
        public ICommand ShareArticleCommand
        {
            get { return _shareArticleCommand; }
        }

        private ICommand _toggleArticleReadCommand;
        public ICommand ToggleArticleReadCommand
        {
            get { return _toggleArticleReadCommand; }
        }

        private ICommand _toggleArticleSavedCommand;
        public ICommand ToggleArticleSavedCommand
        {
            get { return _toggleArticleSavedCommand; }
        }

        private ICommand _changeArticleViewTemplateCommand;
        public ICommand ChangeArticleViewTemplateCommand
        {
            get { return _changeArticleViewTemplateCommand; }
        }

        private ICommand _refreshNewsCommand;
        public ICommand RefreshNewsCommand
        {
            get { return _refreshNewsCommand; }
        }

        private ICommand _markAllReadCommand;
        public ICommand MarkAllReadCommand
        {
            get { return _markAllReadCommand; }
        }
        
        public MainViewModel(
            INewsProvider provider, 
            IShareService share, 
            IGeneralSettings generalSettings,
            IMessageBoxService messageBox)
        {
            _provider = provider;
            _share = share;
            _generalSettings = generalSettings;
            _messageBox = messageBox;

            _selectSourceCommand = new RelayCommandAsync<INewsSource>(o => ConnectionVerifier.Verify(SelectSource, o, OnCommandFail));
            _shareArticleCommand = new RelayCommandAsync<ArticleData>(o => ConnectionVerifier.Verify(ShareArticle, o, OnCommandFail));
            _toggleArticleReadCommand = new RelayCommandAsync<ArticleData>(o => ConnectionVerifier.Verify(ToggleArticleRead, o, OnCommandFail));
            _toggleArticleSavedCommand = new RelayCommandAsync<ArticleData>(o => ConnectionVerifier.Verify(ToggleArticleSaved, o, OnCommandFail));
            _changeArticleViewTemplateCommand = new RelayCommand(ChangeArticleViewTemplate);
            _refreshNewsCommand = new RelayCommandAsync(o => ConnectionVerifier.Verify(RefreshNews, o, OnCommandFail));
            _markAllReadCommand = new RelayCommandAsync(o => ConnectionVerifier.Verify(MarkAllRead, o, OnCommandFail));
        }

        private async Task MarkAllRead(object arg)
        {
            var markRead = await _messageBox.ConfirmationBox("MainPage_MarkAllReadConfirmation/Text");

            if (!markRead) return;

            IsBusy = true;

            await _provider.MarkAllArticlesRead(SelectedSource);

            // Articles = await _provider.LoadArticles(SelectedSource);
            throw new Exception("FIX THIS");
            //var newsSources = await _provider.LoadAllNewsSources();
            //Sources = new ObservableCollection<INewsSource>(newsSources);

            IsBusy = false;
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

            var result = await _provider.Refresh(SelectedSource);

            throw new Exception("FIX THIS");
            //if (result != null)
            //{
            //    Sources = result.Sources;
            //    Articles = result.Articles;
            //}

            IsBusy = false;
        }

        private void ChangeArticleViewTemplate(object obj)
        {
            if (ArticleViewTemplate == ArticleViewTemplateEnum.Cards)
                ArticleViewTemplate = ArticleViewTemplateEnum.Listing;
            else if (ArticleViewTemplate == ArticleViewTemplateEnum.Listing)
                ArticleViewTemplate = ArticleViewTemplateEnum.Cards;
        }

        private async Task ToggleArticleSaved(ArticleData article)
        {
            if (article.IsFavorite)
                await _provider.RemoveFromSaved(article);
            else
                await _provider.SaveArticleForLater(article);
        }

        private async Task ToggleArticleRead(ArticleData article)
        {
            if (article.IsRead)
                await _provider.UnmarkArticleRead(article);
            else
                await _provider.MarkArticleRead(article);

            if (article.Source != null)
                ChangeUnreadNumber(article.IsRead, article.Source.UrlID);
        }

        private Task ShareArticle(ArticleData arg)
        {
            throw new NotImplementedException();
            _share.Share(arg);
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

            await _provider.Initialization();

            var byId = true;

            //if (string.IsNullOrWhiteSpace(open))
            //    open = _generalSettings.CategoryToLoadSetting;

            var sources = await _provider.LoadNewsSources(); ;
            Sources = new ObservableCollection<INewsSource>(sources);

            //SelectedSource = null;

            //INewsSource source = null;

            //if (byId)
            //    source = GetSourceById((string)arg);
            //else
            //    source = GetSourceByName((string)arg);

            //var result = await _provider.Refresh(source);

            var result = await _provider.DownloadNewsSources(Sources);

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
