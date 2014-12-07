using feedsea.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text.RegularExpressions;
using feedsea.Resources;
using Cimbalino.Phone.Toolkit.Services;
using System.IO;
using GalaSoft.MvvmLight.Messaging;
using System.Net;
using feedsea.Common.Providers;
using System.Collections.ObjectModel;
using feedsea.Common.MVVM;
using feedsea.Common.MVVM.Commands;
using feedsea.Common.Providers.Helpers;
using feedsea.Common.Providers.Feedly;
using System.Runtime.Serialization;
using feedsea.Common.Providers.Data;
using feedsea.Services;

namespace feedsea.ViewModels
{
    [DataContract]
    public class AddSourceViewModel : AViewModel<AddSourceViewModel>
    {
        #region Members

        INewsProvider provider;
        INewsService service;

        #endregion Members

        #region Events

        public event EventHandler SaveComplete;

        #endregion Events

        #region Properties

        private bool editMode;
        [DataMember]
        public bool EditMode
        {
            get { return editMode; }
            set
            {
                editMode = value;
                NotifyChanged(o => o.EditMode);
            }
        }

        private string searchQuery;
        public string SearchQuery
        {
            get { return searchQuery; }
            set
            {
                searchQuery = value;
                NotifyChanged(o => o.SearchQuery);
            }
        }

        private ObservableCollection<SearchResultData> searchResults;
        [DataMember]
        public ObservableCollection<SearchResultData> SearchResults
        {
            get { return searchResults; }
            set
            {
                searchResults = value;
                NotifyChanged(o => o.SearchResults);
            }
        }

        private ObservableCollection<CategoryData> categories;
        [DataMember]
        public ObservableCollection<CategoryData> Categories
        {
            get { return categories; }
            set
            {
                categories = value;
                NotifyChanged(o => o.Categories);
            }
        }

        private SearchResultData selectedResult;
        [DataMember]
        public SearchResultData SelectedResult
        {
            get { return selectedResult; }
            set
            {
                selectedResult = value;
                NotifyChanged(o => o.SelectedResult);
            }
        }

        private string newCategoryName;
        [DataMember]
        public string NewCategoryName
        {
            get { return newCategoryName; }
            set
            {
                newCategoryName = value;
                NotifyChanged(o => o.NewCategoryName);
            }
        }

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
                EnableDisableCommands(!value);
            }
        }
        
        #endregion Properties

        #region Commands

        private ICommand searchCommand;
        public ICommand SearchCommand
        {
            get { return searchCommand; }
        }

        private ICommand saveCommand;
        public ICommand SaveCommand
        {
            get { return saveCommand; }
        }

        private ICommand selectResultCommand;
        public ICommand SelectResultCommand
        {
            get { return selectResultCommand; }
        }

        #endregion Commands

        #region Constructors

        public AddSourceViewModel()
        {
            searchCommand = new AsyncDelegateCommand<string>(o => ConnectionVerifyCall(Search, o, OnConnectionFail));
            selectResultCommand = new ParameterRelayCommand<SearchResultData>(SelectResult) { IsEnabled = true };
            saveCommand = new AsyncDelegateCommand(o => ConnectionVerifyCall(SaveSource, o, OnConnectionFail)) { IsEnabled = false };
        }

        public AddSourceViewModel(IConnectionVerify connectionVerify, INewsProvider newsProvider, INewsService newsService)
            : this()
        {
            connection = connectionVerify;
            provider = newsProvider;
            service = newsService;
        }

        #endregion Constructors

        #region Methods

        public async Task LoadDataAndSearch(string query)
        {
            SearchQuery = query;
            await Search(query);
        }

        public void LoadData(string id)
        {
            IsBusy = true;

            EditMode = !string.IsNullOrWhiteSpace(id);

            if (EditMode)
            {
                var source = service.GetSubscription(id);

                var result = new SearchResultData()
                {
                    Id = source.UrlID,
                    Title = source.Name,
                    Url = source.Link
                };

                SelectResult(result);

                foreach (var c in Categories)
                {
                    c.IsSelected = c.Subscriptions != null && c.Subscriptions.Any(o => o.UrlID == source.UrlID);
                }
            }

            IsBusy = false;
        }

        private async Task Search(string arg)
        {
            if (string.IsNullOrWhiteSpace(arg)) return;

            IsBusy = true;

            SelectedResult = null;
            var results = await provider.SearchSources(arg);
            SearchResults = new ObservableCollection<SearchResultData>(results);

            IsBusy = false;
        }

        private void SelectResult(SearchResultData arg)
        {
            IsBusy = true;

            Categories = new ObservableCollection<CategoryData>(service.Sources.Where(o => !o.Own));
            SearchResults = null;
            SelectedResult = arg;

            IsBusy = false;
        }

        private async Task SaveSource(object arg)
        {
            IsBusy = true;

            var categories = Categories.Where(o => o.IsSelected).ToList();

            INewsSource source = null;

            if (EditMode)
                source = await provider.EditSource(SelectedResult, categories, NewCategoryName);
            else
                source = await provider.AddNewSource(SelectedResult, categories, NewCategoryName);

            IsBusy = false;

            var refresh = service.RefreshNews(source);

            if (SaveComplete != null)
                SaveComplete(this, new EventArgs());

            await refresh;
            service.SelectedSource = source;
        }

        public void EnableDisableCommands(bool enable)
        {
            if (SearchCommand == null || SelectResultCommand == null || saveCommand == null)
                return;

            (SearchCommand as AsyncDelegateCommand<string>).IsEnabled = enable;
            (SelectResultCommand as ParameterRelayCommand<SearchResultData>).IsEnabled = enable;
            (SaveCommand as AsyncDelegateCommand).IsEnabled = enable && SelectedResult != null;
        }

        private void OnConnectionFail()
        {
            IsBusy = false;
        }

        #endregion Methods
    }
}
