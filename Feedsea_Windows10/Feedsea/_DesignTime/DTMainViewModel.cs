using Feedsea.Common.Providers;
using Feedsea.Common.Providers.Data;
using Feedsea.Common.Services;
using Feedsea.ViewModels;
using MVVMBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Feedsea._DesignTime
{
    public class DTMainViewModel : BaseViewModel
    {
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

        public DTMainViewModel()
        {
            Articles = new ObservableCollection<ArticleData>()
            {
                new ArticleData()
                {
                    Title = "Test Title",
                    Summary = "Test Summary",

                }
            };

            Sources = new ObservableCollection<INewsSource>()
            {
                new CategoryData()
                {
                    Name = "Category"
                },
                new SubscriptionData()
                {
                    Name = "Subscription"
                }
            };
        }
    }
}
