using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Cimbalino.Phone.Toolkit.Helpers;
using feedsea.Common;
using Windows.UI.Xaml.Media.Imaging;
using System.IO.IsolatedStorage;
using feedsea.Resources;
using feedsea.ViewModels;
using Windows.UI.Xaml.Controls.Primitives;
using feedsea.UserControls;
using feedsea.Common.Controls;
using feedsea.Common.MVVM.Tombstone;
using System.Text.RegularExpressions;
using feedsea.Models;
using feedsea.Common.Providers;
using feedsea.Common.Providers.Data;

namespace feedsea.Views
{

   public partial class MainPage
      : Windows.UI.Xaml.Controls.Page
   {
      private bool isScrolling;
      private IShareService shareService;
      private List<ArticleData> articlesToMarkRead;

      public MainViewModel ViewModel
      {
         get
         {
            return (MainViewModel)this.DataContext;
         }
      }


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
      protected async override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
      {
         TombstoneHelper.page_OnNavigatedTo(this, e);
         isScrolling = false;
         if ( e.NavigationMode == NavigationMode.Back )
         {
            if ( MainSlideView.SelectedIndex == 1 )
               MainSlideView.SelectedIndex = 0;
            GC.Collect();
            GC.WaitForPendingFinalizers();
         }
         if ( e.NavigationMode != NavigationMode.Back && e.NavigationMode != NavigationMode.Reset )
         {
            while ( this.NavigationService.BackStack.Any() )
            {
               //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Navigation.NavigationService.BackStack was not upgraded
               //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Navigation.NavigationService.BackStack was not upgraded
               ((Windows.UI.Xaml.Controls.Frame)Windows.UI.Xaml.Window.Current.Content).BackStack.RemoveAt(((Windows.UI.Xaml.Controls.Frame)Windows.UI.Xaml.Window.Current.Content).BackStack.Count - 1);
            }
            var authenticate = await ViewModel.Authenticate();
            if ( authenticate )
            {
               Frame.Navigate(typeof(feedsea.Views.WelcomePage));
               return ;
            }
            var bac = new BackgroundAgentController();
            bac.StartPeriodicAgent();
            string open = null;
            if ( NavigationContext.QueryString.TryGetValue("open", out open) )
            {
               await ViewModel.LoadDataAsync(open);
               return ;
            }
            string command = null;
            var vcResult = InterpretVoiceCommand(out command);
            if ( vcResult != VoiceCommandType.None )
            {
               if ( vcResult == VoiceCommandType.OpenNews )
               {
                  await ViewModel.SemanticLoadDataAsync(command);
               }
               else if ( vcResult == VoiceCommandType.SearchFeed )
               {
                  WP8SL_TO_UWP.NavigationHelper.NavigateTo(new Uri(new Uri("ms-appx://"), string.Format("/Views/AddSource.xaml?searchTerm={0}", command)));
                  await ViewModel.LoadDataAsync();
               }
               return ;
            }
            if ( !ViewModel.IsDataLoaded )
            {
               await ViewModel.LoadDataAsync();
            }
         }
      }

      protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
      {
         TombstoneHelper.page_OnNavigatedFrom(this, e);
         base.OnNavigatedFrom(e);
      }

      protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
      {
         if ( shareService != null && shareService.IsShareOpen )
         {
            e.Cancel = true;
            shareService.CancelShare();
            return ;
         }
         //if (ViewModel.CancelShare())
         //{
         //    e.Cancel = true;
         //    return;
         //}
         if ( MainSlideView.SelectedIndex == 1 )
         {
            e.Cancel = true;
            MainSlideView.SelectedIndex = 0;
            return ;
         }
         base.OnBackKeyPress(e);
      }
#endregion Page_Event_Handlers

#region ViewModel_Event_Handlers
      private void ViewModel_AddSource(object sender, EventArgs e)
      {
         Frame.Navigate(typeof(feedsea.Views.AddSource));
      }

      void ViewModel_SelectedSourceLoading(object sender, EventArgs e)
      {
         if ( articlesToMarkRead != null && articlesToMarkRead.Any() )
            articlesToMarkRead.Clear();
         MainSlideView.SelectedIndex = 0;
      //MainPivot.SelectedIndex = 0;
      }

#endregion ViewModel_Event_Handlers
#region Controls_Event_Handlers
      private void ConditionalMessageCommandControl_ButtonClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
      {
         Frame.Navigate(typeof(feedsea.Views.AddSource));
      }

      private void BackToTop_Click(object sender, EventArgs e)
      {
         if ( ViewModel.Articles != null && ViewModel.Articles.Any() )
            ArticleList.BringIntoView(ViewModel.Articles.FirstOrDefault());
      }

      private void ArticleList_ItemStateChanged(object sender, Telerik.Windows.Controls.ItemStateChangedEventArgs e)
      {
         if ( e.State == Telerik.Windows.Controls.ItemState.Realized && isScrolling )
         {
            var item = ArticleList.ViewportItems.FirstOrDefault();
            if ( item.DataContext is ArticleData && e.DataItem is ArticleData )
            {
               var top = item.DataContext as ArticleData;
               var currentArticleIndex = ViewModel.Articles.IndexOf(((ArticleData)e.DataItem));
               var priorArticleIndex = ViewModel.Articles.IndexOf(top);
               var priors = ViewModel.Articles.TakeWhile(x => x.UniqueID != top.UniqueID).Where(o => !o.IsRead);
               if ( currentArticleIndex > priorArticleIndex && priors != null && priors.Any() )
               {
                  if ( articlesToMarkRead == null )
                     articlesToMarkRead = new List<ArticleData>();
                  var toAdd = priors.Where(o => !articlesToMarkRead.Any(x => x.UniqueID == o.UniqueID));
                  articlesToMarkRead.AddRange(toAdd);
                  if ( articlesToMarkRead.Count(o => !o.IsRead) >= 5 )
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
         if ( e.NewState == Telerik.Windows.Controls.ScrollState.BottomStretch )
         {
            var item = ArticleList.ViewportItems.FirstOrDefault();
            if ( item != null && item.DataContext is ArticleData )
            {
               var top = item.DataContext as ArticleData;
               var priors = ViewModel.Articles.TakeWhile(x => x.UniqueID != top.UniqueID).Where(o => !o.IsRead);
               if ( priors != null && priors.Any() )
               {
                  if ( articlesToMarkRead == null )
                     articlesToMarkRead = new List<ArticleData>();
                  var toAdd = priors.Where(o => !articlesToMarkRead.Any(x => x.UniqueID == o.UniqueID));
                  articlesToMarkRead.AddRange(toAdd);
                  if ( articlesToMarkRead.Any() )
                  {
                     ViewModel.MarkMultipleArticlesReadCommand.Execute(articlesToMarkRead.Where(o => !o.IsRead).ToList());
                     articlesToMarkRead.Clear();
                  }
               }
            }
         }
      }

      private void BtnSidebar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
      {
         if ( MainSlideView.SelectedIndex == 0 )
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
         Frame.Navigate(typeof(feedsea.Views.AddSource));
      }

      private void MnuSettings_Click(object sender, EventArgs e)
      {
         Frame.Navigate(typeof(feedsea.Views.SettingsPage));
      }

      private void ArticleList_RefreshRequested(object sender, EventArgs e)
      {
         ArticleList.StopPullToRefreshLoading(true);
         if ( ViewModel.RefreshCommand != null && ViewModel.RefreshCommand.CanExecute(null) )
            ViewModel.RefreshCommand.Execute(null);
      }

      private void ArticleItemTemplateSelector_Tap(object sender, System.Windows.Input.GestureEventArgs e)
      {
         var control = (sender as Windows.UI.Xaml.Controls.ContentControl);
         if ( control == null )
            return ;
         var article = control.DataContext as ArticleData;
         if ( article == null )
            return ;
         App.RootFrame.Navigate(new Uri(new Uri("ms-appx://"), "/Views/ArticlePage.xaml?id=" + System.Net.WebUtility.UrlEncode(article.UniqueID)));
      }

      private void BtnShareArticle_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
      {
         if ( shareService == null )
            shareService = new ShareService();
         if ( shareService.IsShareOpen )
            return ;
         shareService.OnShareFinished += shareService_OnShareFinished;
         shareService.Share((sender as Windows.UI.Xaml.Controls.Button).DataContext as ArticleData);
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
         switch ( voiceCommandName ) // What command launched the app?

         {
            case "openNews":
               if ( NavigationContext.QueryString.TryGetValue("feed", out result) )
                  commandType = VoiceCommandType.OpenNews;
               else
                  commandType = VoiceCommandType.None;
               break;
            case "searchFeed":
               if ( NavigationContext.QueryString.TryGetValue("searchQuery", out result) )
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