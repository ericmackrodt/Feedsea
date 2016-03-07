using Broadcaster;
using Feedsea.Common;
using Feedsea.Common.Controls;
using Feedsea.Common.Events;
using Feedsea.Common.Helpers;
using Feedsea.Common.Providers;
using Feedsea.Common.Providers.Data;
using Feedsea.Common.Services;
using Feedsea.Settings;
using MVVMBasic;
using MVVMBasic.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Feedsea.ViewModels
{
    public class ArticleListViewModel : BaseViewModel
    {
        public event EventHandler ArticleLayoutChanged;
        
        private IMessageBoxService messageBox;
        private IArticleProvider provider;
        private IGeneralSettings generalSettings;
        private IBroadcaster broadcaster;
        private DateTime lastLoad = DateTime.MinValue;
        private string continuationString;

        public ArticleViewTemplateEnum ArticleViewTemplate
        {
            get { return generalSettings.ArticleListTemplate; }
            set
            {
                if (generalSettings.ArticleListTemplate != value)
                {
                    generalSettings.ArticleListTemplate = value;
                    NotifyChanged();
                }
            }
        }

        private INewsSource selectedSource;
        public INewsSource SelectedSource
        {
            get { return selectedSource; }
            set
            {
                selectedSource = value;
                NotifyChanged();
            }
        }

        private PaginatedArticlesCollection articles;
        public PaginatedArticlesCollection Articles
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

        private ICommand changeArticleViewTemplateCommand;
        public ICommand ChangeArticleViewTemplateCommand
        {
            get { return changeArticleViewTemplateCommand; }
        }

        private ICommand refreshArticlesCommand;
        public ICommand RefreshArticlesCommand
        {
            get { return refreshArticlesCommand; }
        }

        private ICommand markAllReadCommand;
        public ICommand MarkAllReadCommand
        {
            get { return markAllReadCommand; }
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

        public ArticleListViewModel(
            IArticleProvider provider, 
            IGeneralSettings generalSettings, 
            IBroadcaster broadcaster,
            IMessageBoxService messageBox)
        {
            this.provider = provider;
            this.generalSettings = generalSettings;
            this.broadcaster = broadcaster;
            this.messageBox = messageBox;

            changeArticleViewTemplateCommand = new RelayCommand(ChangeArticleViewTemplate);
            refreshArticlesCommand = new RelayCommandAsync(RefreshArticles);
            markAllReadCommand = new RelayCommandAsync(o => ConnectionVerifier.Verify(MarkAllRead, o, OnCommandFail));
            shareArticleCommand = new RelayCommandAsync<ArticleData>(o => ConnectionVerifier.Verify(ShareArticle, o, OnCommandFail));
            toggleArticleReadCommand = new RelayCommandAsync<ArticleData>(o => ConnectionVerifier.Verify(ToggleArticleRead, o, OnCommandFail));
            toggleArticleSavedCommand = new RelayCommandAsync<ArticleData>(o => ConnectionVerifier.Verify(ToggleArticleSaved, o, OnCommandFail));
        }

        private void OnCommandFail()
        {
            IsBusy = false;
        }

        private void ChangeArticleViewTemplate(object obj)
        {
            if (ArticleViewTemplate == ArticleViewTemplateEnum.Cards)
                ArticleViewTemplate = ArticleViewTemplateEnum.Listing;
            else if (ArticleViewTemplate == ArticleViewTemplateEnum.Listing)
                ArticleViewTemplate = ArticleViewTemplateEnum.Cards;

            if (ArticleLayoutChanged != null)
                ArticleLayoutChanged(this, new EventArgs());
        }

        private async Task LoadArticles()
        {
            var result = await provider.DownloadArticles(Articles, SelectedSource);

            if (!result.Articles.All(o => Articles.Any(x => x.UniqueID == o.UniqueID)))
                Articles = new PaginatedArticlesCollection(result.Articles, LoadMoreArticles);

            if (!string.IsNullOrWhiteSpace(result.Continuation))
                continuationString = result.Continuation;

            lastLoad = DateTime.Now;
        }

        private async Task RefreshArticles(object obj)
        {
            if (Articles.Any() && (DateTime.Now - lastLoad).Minutes < 2)
                return;

            IsBusy = true;

            await LoadArticles();

            IsBusy = false;
        }

        public override async Task LoadData(object arg)
        {
            IsBusy = true;

            SelectedSource = (INewsSource)arg;

            var articles = await provider.LoadArticles(SelectedSource);
            Articles = new PaginatedArticlesCollection(articles, LoadMoreArticles);

            await LoadArticles();

            IsBusy = false;
        }

        private async Task<IEnumerable<ArticleData>> LoadMoreArticles()
        {
            IsBusy = true;
            var result = await provider.DownloadMoreArticles(continuationString, Articles, SelectedSource);
            IsBusy = false;
            continuationString = result.Continuation;
            return result.Articles;
        }

        private async Task MarkAllRead(object arg)
        {
            var markRead = await messageBox.ConfirmationBox("ArticleListPage_MarkAllReadConfirmation/Text");

            if (!markRead) return;

            IsBusy = true;

            await provider.MarkAllArticlesRead(SelectedSource);

            foreach (var art in Articles)
            {
                art.IsRead = true;
            }

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
            if (article.IsRead)
                await provider.UnmarkArticleRead(article);
            else
                await provider.MarkArticleRead(article);

            broadcaster.Event<UpdateUnreadCountEvent>().Broadcast(new KeyValuePair<string, bool>(article.Source.UrlID, article.IsRead));
        }

        private async Task ShareArticle(ArticleData arg)
        {
            //throw new NotImplementedException();
            //share.Share(arg);
        }
    }
}