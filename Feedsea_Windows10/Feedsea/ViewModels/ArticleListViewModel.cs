using Broadcaster;
using Feedsea.Common;
using Feedsea.Common.Events;
using Feedsea.Common.Providers;
using Feedsea.Common.Providers.Data;
using Feedsea.Settings;
using MVVMBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.ViewModels
{
    public class ArticleListViewModel : BaseViewModel
    {
        private INewsProvider provider;
        private IGeneralSettings generalSettings;
        private IBroadcaster broadcaster;

        public ArticleViewTemplateEnum ArticleViewTemplate
        {
            get { return generalSettings.ArticleListTemplate; }
            set
            {
                //if (generalSettings.ArticleListTemplate != value)
                //{
                    generalSettings.ArticleListTemplate = value;
                    NotifyChanged();
                //}
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

        public ArticleListViewModel(INewsProvider provider, IGeneralSettings generalSettings, IBroadcaster broadcaster)
        {
            this.provider = provider;
            this.generalSettings = generalSettings;
            this.broadcaster = broadcaster;

            this.broadcaster.Event<ArticleViewTemplateChangedEvent>().Subscribe(OnArticleViewTemplateChanged);
        }

        private void OnArticleViewTemplateChanged(ArticleViewTemplateEnum obj)
        {
            ArticleViewTemplate = obj;
        }

        public override async Task LoadData(object arg)
        {
            IsBusy = true;

            SelectedSource = (INewsSource)arg;

            var articles = await provider.LoadArticles(SelectedSource);
            Articles = new ObservableCollection<ArticleData>(articles);

            IsBusy = false;
        }

        public async Task Refresh(object arg)
        {
            IsBusy = true;

            var source = (INewsSource)arg;

            var result = await provider.DownloadArticles(Articles.FirstOrDefault(), source);

            if (result != null && result.Count() > 0)
            {
                Articles = new ObservableCollection<ArticleData>(result);
            }

            IsBusy = false;
        }
    }
}
