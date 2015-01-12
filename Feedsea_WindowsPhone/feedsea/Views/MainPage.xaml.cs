using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Cimbalino.Phone.Toolkit.Helpers;
using feedsea.Common;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using feedsea.Resources;
using feedsea.ViewModels;
using System.Windows.Controls.Primitives;
using feedsea.UserControls;
using feedsea.Common.Controls;
using feedsea.Common.MVVM.Tombstone;
using System.Text.RegularExpressions;
using feedsea.Models;
using feedsea.Common.Providers;
using feedsea.Common.Providers.Data;

namespace feedsea.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        private bool isScrolling;
        private IShareService shareService;
        private List<ArticleData> articlesToMarkRead;

        public MainViewModel ViewModel { get { return (MainViewModel)this.DataContext; } }

        #region Constructor

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            ViewModel.SelectedSourceLoading += ViewModel_SelectedSourceLoading;
            ViewModel.AddSource += ViewModel_AddSource;
        }

        #endregion Constructor

        #region Page_Event_Handlers

        // Load data for the ViewModel Items
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            TombstoneHelper.page_OnNavigatedTo(this, e);

            isScrolling = false;

            if (e.NavigationMode == NavigationMode.Back)
            {
                if (MainSlideView.SelectedIndex == 1)
                    MainSlideView.SelectedIndex = 0;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            if (e.NavigationMode != NavigationMode.Back && e.NavigationMode != NavigationMode.Reset)
            {
                while (this.NavigationService.BackStack.Any())
                {
                    this.NavigationService.RemoveBackEntry();
                }

                var authenticate = await ViewModel.Authenticate();

                if (authenticate)
                {
                    NavigationService.Navigate(new Uri("/Views/WelcomePage.xaml", UriKind.Relative));
                    return;
                }

                foreach (var t in ShellTile.ActiveTiles)
                {
                    var tile = new FlipTileData()
                    {
                        Count = 0
                    };

                    t.Update(tile);
                }

                var bac = new BackgroundAgentController();
                bac.StartPeriodicAgent();

                string open = null;
                if (NavigationContext.QueryString.TryGetValue("open", out open))
                {
                    await ViewModel.LoadDataAsync(open);
                    return;
                }

                string command = null;
                var vcResult = InterpretVoiceCommand(out command);
                if (vcResult != VoiceCommandType.None)
                {
                    if (vcResult == VoiceCommandType.OpenNews)
                    {
                        await ViewModel.SemanticLoadDataAsync(command);
                    }
                    else if (vcResult == VoiceCommandType.SearchFeed)
                    {
                        NavigationService.Navigate(new Uri(string.Format("/Views/AddSource.xaml?searchTerm={0}", command), UriKind.Relative));
                        await ViewModel.LoadDataAsync();
                    }

                    return;
                }
            }

            if (!ViewModel.IsDataLoaded)
            {
                await ViewModel.LoadDataAsync();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            TombstoneHelper.page_OnNavigatedFrom(this, e);
            base.OnNavigatedFrom(e);
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (shareService != null && shareService.IsShareOpen)
            {
                e.Cancel = true;
                shareService.CancelShare();
                return;
            }

            //if (ViewModel.CancelShare())
            //{
            //    e.Cancel = true;
            //    return;
            //}

            if (MainSlideView.SelectedIndex == 1)
            {
                e.Cancel = true;
                MainSlideView.SelectedIndex = 0;
                return;
            }

            base.OnBackKeyPress(e);
        }

        #endregion Page_Event_Handlers

        #region ViewModel_Event_Handlers

        private void ViewModel_AddSource(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/AddSource.xaml", UriKind.Relative));
        }

        void ViewModel_SelectedSourceLoading(object sender, EventArgs e)
        {
            if (articlesToMarkRead != null && articlesToMarkRead.Any())
                articlesToMarkRead.Clear();

            MainSlideView.SelectedIndex = 0;
            //MainPivot.SelectedIndex = 0;
        }

        #endregion ViewModel_Event_Handlers

        #region Controls_Event_Handlers

        private void ConditionalMessageCommandControl_ButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/AddSource.xaml", UriKind.Relative));
        }

        private void BackToTop_Click(object sender, EventArgs e)
        {
            if (ViewModel.Articles != null && ViewModel.Articles.Any())
                ArticleList.BringIntoView(ViewModel.Articles.FirstOrDefault());
        }

        private void ArticleList_ItemStateChanged(object sender, Telerik.Windows.Controls.ItemStateChangedEventArgs e)
        {
            if (e.State == Telerik.Windows.Controls.ItemState.Realized && isScrolling)
            {
                var item = ArticleList.ViewportItems.FirstOrDefault();
                if (item.DataContext is ArticleData && e.DataItem is ArticleData)
                {
                    var top = item.DataContext as ArticleData;

                    var currentArticleIndex = ViewModel.Articles.IndexOf(((ArticleData)e.DataItem));
                    var priorArticleIndex = ViewModel.Articles.IndexOf(top);

                    var priors = ViewModel.Articles.TakeWhile(x => x.UniqueID != top.UniqueID).Where(o => !o.IsRead);

                    if (currentArticleIndex > priorArticleIndex && priors != null && priors.Any())
                    {
                        if (articlesToMarkRead == null)
                            articlesToMarkRead = new List<ArticleData>();

                        var toAdd = priors.Where(o => !articlesToMarkRead.Any(x => x.UniqueID == o.UniqueID));
                        articlesToMarkRead.AddRange(toAdd);

                        if (articlesToMarkRead.Count(o => !o.IsRead) >= 5)
                        {
                            ViewModel.MarkMultipleArticlesReadCommand.Execute(articlesToMarkRead.Where(o => !o.IsRead).ToList());
                            articlesToMarkRead.Clear();
                        }
                    }
                }
            }
        }

        private void ArticleList_ScrollStateChanged(object sender, Telerik.Windows.Controls.ScrollStateChangedEventArgs e)
        {
            isScrolling = e.NewState == Telerik.Windows.Controls.ScrollState.Scrolling || e.NewState == Telerik.Windows.Controls.ScrollState.Flicking;

            if (e.NewState == Telerik.Windows.Controls.ScrollState.BottomStretch)
            {
                var item = ArticleList.ViewportItems.FirstOrDefault();
                if (item != null && item.DataContext is ArticleData)
                {
                    var top = item.DataContext as ArticleData;

                    var priors = ViewModel.Articles.TakeWhile(x => x.UniqueID != top.UniqueID).Where(o => !o.IsRead);

                    if (priors != null && priors.Any())
                    {
                        if (articlesToMarkRead == null)
                            articlesToMarkRead = new List<ArticleData>();

                        var toAdd = priors.Where(o => !articlesToMarkRead.Any(x => x.UniqueID == o.UniqueID));
                        articlesToMarkRead.AddRange(toAdd);

                        if (articlesToMarkRead.Any())
                        {
                            ViewModel.MarkMultipleArticlesReadCommand.Execute(articlesToMarkRead.Where(o => !o.IsRead).ToList());
                            articlesToMarkRead.Clear();
                        }
                    }
                }
            }
        }

        private void BtnSidebar_Click(object sender, RoutedEventArgs e)
        {
            if (MainSlideView.SelectedIndex == 0)
                MainSlideView.SelectedIndex = 1;
            else
                MainSlideView.SelectedIndex = 0;
        }

        private void LayoutRoot_SelectionChanged(object sender, EventArgs e)
        {
            appBar.IsVisible = MainSlideView.SelectedIndex == 0;
        }

        private void MnuAddSource_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/AddSource.xaml", UriKind.Relative));
        }

        private void MnuSettings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SettingsPage.xaml", UriKind.Relative));
        }

        private void ArticleList_RefreshRequested(object sender, EventArgs e)
        {
            ArticleList.StopPullToRefreshLoading(true);
            if (ViewModel.RefreshCommand != null && ViewModel.RefreshCommand.CanExecute(null))
                ViewModel.RefreshCommand.Execute(null);
        }

        private void ArticleItemTemplateSelector_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var control = (sender as ContentControl);
            if (control == null) return;
            var article = control.DataContext as ArticleData;
            if (article == null) return;
            App.RootFrame.Navigate(new Uri("/Views/ArticlePage.xaml?id=" + HttpUtility.UrlEncode(article.UniqueID), UriKind.Relative));
        }

        private void BtnShareArticle_Click(object sender, RoutedEventArgs e)
        {
            if (shareService == null)
                shareService = new ShareService();

            if (shareService.IsShareOpen) return;

            shareService.OnShareFinished += shareService_OnShareFinished;
            shareService.Share((sender as Button).DataContext as ArticleData);
            appBar.IsVisible = false;
        }

        void shareService_OnShareFinished(object sender, EventArgs e)
        {
            shareService.OnShareFinished -= shareService_OnShareFinished;
            appBar.IsVisible = true;
        }

        #endregion Controls_Event_Handlers

        #region Private_Methods

        private VoiceCommandType InterpretVoiceCommand(out string command)
        {
            //string recoText = null; // What did the user say? e.g. MSDN, "Find Windows Phone Voice Commands"
            //NavigationContext.QueryString.TryGetValue("reco", out recoText);

            string voiceCommandName = null; // Which command was recognized in the VCD.XML file? e.g. "FindText"
            NavigationContext.QueryString.TryGetValue("voiceCommandName", out voiceCommandName);

            string searchTerms = null; // What did the user say, for named phrase topic or list "slots"? e.g. "Windows Phone Voice Commands"
            NavigationContext.QueryString.TryGetValue("dictatedSearchTerms", out searchTerms);

            var commandType = VoiceCommandType.None;
            string result = null;

            switch (voiceCommandName) // What command launched the app?
            {
                case "openNews":
                    if (NavigationContext.QueryString.TryGetValue("feed", out result))
                        commandType = VoiceCommandType.OpenNews;
                    else
                        commandType = VoiceCommandType.None;
                    break;
                case "searchFeed":
                    if (NavigationContext.QueryString.TryGetValue("searchQuery", out result))
                        commandType = VoiceCommandType.SearchFeed;
                    else
                        commandType = VoiceCommandType.None;
                    break;
            }

            command = result;
            return commandType;
        }

        #endregion Private_Methods
    }
}