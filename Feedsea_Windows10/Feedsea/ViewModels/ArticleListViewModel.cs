using Feedsea.Common.Providers;
using Feedsea.Common.Providers.Data;
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
        private INewsProvider _provider;

        private INewsSource _selectedSource;
        public INewsSource SelectedSource
        {
            get { return _selectedSource; }
            set
            {
                _selectedSource = value;
                NotifyChanged();
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

        public ArticleListViewModel(INewsProvider provider)
        {
            _provider = provider;
        }

        public override async Task LoadData(object arg)
        {
            IsBusy = true;

            SelectedSource = (INewsSource)arg;

            var articles = await _provider.LoadArticles(SelectedSource);
            Articles = new ObservableCollection<ArticleData>(articles);

            IsBusy = false;
        }

        public async Task Refresh(object arg)
        {
            IsBusy = true;

            var source = (INewsSource)arg;

            var result = await _provider.DownloadArticles(Articles.FirstOrDefault(), source);

            if (result != null && result.Count() > 0)
            {
                Articles = new ObservableCollection<ArticleData>(result);
            }

            IsBusy = false;
        }
    }
}
