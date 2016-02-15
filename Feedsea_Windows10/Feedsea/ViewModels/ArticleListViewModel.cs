﻿using Broadcaster;
using Feedsea.Common;
using Feedsea.Common.Events;
using Feedsea.Common.Providers;
using Feedsea.Common.Providers.Data;
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

        private IArticleProvider provider;
        private IGeneralSettings generalSettings;
        private IBroadcaster broadcaster;
        private DateTime lastLoad = DateTime.MinValue;

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

        private ICommand changeArticleViewTemplateCommand;
        public ICommand ChangeArticleViewTemplateCommand
        {
            get { return changeArticleViewTemplateCommand; }
        }

        public ArticleListViewModel(IArticleProvider provider, IGeneralSettings generalSettings, IBroadcaster broadcaster)
        {
            this.provider = provider;
            this.generalSettings = generalSettings;
            this.broadcaster = broadcaster;

            changeArticleViewTemplateCommand = new RelayCommand(ChangeArticleViewTemplate);
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

        public override async Task LoadData(object arg)
        {
            if ((DateTime.Now - lastLoad).Minutes < 2)
                return;

            IsBusy = true;

            SelectedSource = (INewsSource)arg;

            var articles = await provider.LoadArticles(SelectedSource);
            Articles = new ObservableCollection<ArticleData>(articles);

            var source = (INewsSource)arg;

            var result = await provider.DownloadArticles(Articles, source);

            if (!result.All(o => Articles.Any(x => x.UniqueID == o.UniqueID)))
                Articles = new ObservableCollection<ArticleData>(result);

            IsBusy = false;
        }
    }
}
