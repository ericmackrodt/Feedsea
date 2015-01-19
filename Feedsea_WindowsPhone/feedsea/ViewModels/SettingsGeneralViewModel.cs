using feedsea.Common.MVVM;
using feedsea.Common.Providers;
using feedsea.Common.Providers.Data;
using feedsea.Common.Providers.MobilizerProvider;
using feedsea.Resources;
using feedsea.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.ViewModels
{
    public class SettingsGeneralViewModel : AViewModel<SettingsGeneralViewModel>
    {
        private IGeneralSettings _settings;
        private INewsProvider _provider;

        #region Settings_Properties

        [IgnoreDataMember]
        public bool ShowArticlesFromOldestToNewest
        {
            get { return _settings.ArticlesFromOldestToNewestSetting; }
            set
            {
                _settings.ArticlesFromOldestToNewestSetting = value;
                NotifyChanged(o => o.ShowArticlesFromOldestToNewest);
            }
        }

        [IgnoreDataMember]
        public bool MarkArticlesReadWhenOpened
        {
            get { return _settings.MarkArticlesReadWhenOpenedSetting; }
            set
            {
                _settings.MarkArticlesReadWhenOpenedSetting = value;
                NotifyChanged(o => o.MarkArticlesReadWhenOpened);
            }
        }

        [IgnoreDataMember]
        public bool ShowReadArticles
        {
            get { return _settings.ShowReadSetting; }
            set
            {
                _settings.ShowReadSetting = value;

                if (value)
                    ShowReadIfNoUnread = false;

                NotifyChanged(o => o.ShowReadArticles);
            }
        }

        [IgnoreDataMember]
        public bool DownloadArticleIfNoContent
        {
            get { return _settings.DownloadArticleIfNoContentSetting; }
            set
            {
                _settings.DownloadArticleIfNoContentSetting = value;
                NotifyChanged(o => o.DownloadArticleIfNoContent);
            }
        }

        private ObservableCollection<INewsSource> sources;
        [DataMember]
        public ObservableCollection<INewsSource> Sources
        {
            get { return sources; }
            set
            {
                sources = value;
                NotifyChanged(o => o.Sources);
            }
        }

        private INewsSource selectedSource;
        [IgnoreDataMember]
        public INewsSource SelectedSource
        {
            get { return Sources.FirstOrDefault(o => o.UrlID == _settings.CategoryToLoadSetting); }
            set
            {
                var src = value;
                _settings.CategoryToLoadSetting = src == null ? null : src.UrlID;
                selectedSource = src;
                NotifyChanged(o => o.SelectedSource);
            }
        }

        [IgnoreDataMember]
        public bool MarkReadOnFeedScroll
        {
            get { return _settings.MarkReadOnFeedScrollSetting; }
            set
            {
                _settings.MarkReadOnFeedScrollSetting = value;
                NotifyChanged(o => o.MarkReadOnFeedScroll);
            }
        }

        [IgnoreDataMember]
        public bool ShowReadIfNoUnread
        {
            get { return _settings.ShowReadIfNoUnreadSetting; }
            set
            {
                _settings.ShowReadIfNoUnreadSetting = value;
                NotifyChanged(o => o.ShowReadIfNoUnread);
            }
        }

        public bool AskBeforeMarkingAllRead
        {
            get { return _settings.AskConfirmationMarkReadSetting; }
            set
            {
                _settings.AskConfirmationMarkReadSetting = value;
                NotifyChanged(o => o.AskBeforeMarkingAllRead);
            }
        }

        #endregion Settings_Properties

        public SettingsGeneralViewModel(IGeneralSettings settings, INewsProvider provider)
        {
            _settings = settings;
            _provider = provider;
        }

        public async Task LoadDataAsync()
        {
            if (Sources != null)
                return;

            Sources = new ObservableCollection<INewsSource>();

            Sources.Add(new SubscriptionData()
            {
                UrlID = null,
                Name = AppResources.AllArticles
            });
            var sources = await _provider.LoadCategories();
            foreach (var source in sources)
            {
                Sources.Add(source);
            }

            SelectedSource = Sources.FirstOrDefault(o => o.UrlID == _settings.CategoryToLoadSetting);
        }
    }
}
