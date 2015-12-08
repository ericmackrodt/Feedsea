using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using feedsea.ViewModels;
using Windows.UI.Xaml.Controls.Primitives;
using feedsea.UserControls;
using feedsea.Common.Providers;
using Windows.System;
using System.Threading.Tasks;
using feedsea.Settings;
using feedsea.Common.MVVM.Tombstone;
using feedsea.Common.Controls;
using feedsea.Common;

namespace feedsea.Views
{

   public partial class ArticlePage
      : Windows.UI.Xaml.Controls.Page
   {
      private ThirdPartySettings settings;
      private IShareService shareService;
      private MobilizerPopup mobilizerPopup;

      public ArticleViewModel ViewModel
      {
         get
         {
            return (DataContext as ArticleViewModel);
         }
      }


      public ArticlePage()
      {
         InitializeComponent();
         settings = new ThirdPartySettings();
#if DEBUG
         //System.Diagnostics.Debug.WriteLine("ArticlePage opened!");
         System.Diagnostics.Debug.WriteLine((((long)Windows.System.MemoryManager.AppMemoryUsage) / 1024 / 1024).ToString());
         #endif
      }

      protected async override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
      {
         base.OnNavigatedTo(e);
         TombstoneHelper.page_OnNavigatedTo(this, e);
         string id = null;
         if ( e.NavigationMode == NavigationMode.New && NavigationContext.QueryString.TryGetValue("id", out id) )
         {
            await ViewModel.LoadDataAsync(id);
         }
      }

      void ViewModel_OnShareChanged(object sender, bool e)
      {
         appBar.IsVisible = !e;
      }

      protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
      {
         base.OnNavigatedFrom(e);
         ViewModel.Unload();
         TombstoneHelper.page_OnNavigatedFrom(this, e);
      }

      private async void ArticleView_ScriptNotify(object sender, NotifyEventArgs e)
      {
         if ( new string[] { "metrotube", "toib", "mytube", "vnd.youtube" }.Any(o => e.Value.StartsWith(o)) )
         {
            await Launcher.LaunchUriAsync(new Uri(e.Value));
            return ;
         }
         await OpenBrowser(e.Value);
      }

      public async Task OpenBrowser(string link)
      {
         switch ( settings.LinkNavigationSetting )
         {
            case Common.LinkNavigationBrowsers.InternetExplorer:
               this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () => await OpenIE(link));
               break;
            //case Common.LinkNavigationBrowsers.MaxthonBrowser:
            //    await Launcher.LaunchUriAsync(new Uri(string.Format("maxthon:{0}", link)));
            //    break;
            case Common.LinkNavigationBrowsers.NokiaXpress:
               await Launcher.LaunchUriAsync(new Uri(string.Format("nokia-xpress:Pageload?Url={0}", link)));
               break;
            case Common.LinkNavigationBrowsers.UCBrowser:
               await Launcher.LaunchUriAsync(new Uri(string.Format("uc-url:{0}", link)));
               break;
         }
      }

      public static async System.Threading.Tasks.Task OpenIE(string url)
      {
         if ( string.IsNullOrWhiteSpace(url) || !new Uri(url).IsAbsoluteUri )
            return ;
         WindowsPhoneUWP.UpgradeHelpers.WebBrowserTaskHelper webBrowserTask = new WindowsPhoneUWP.UpgradeHelpers.WebBrowserTaskHelper
            {
               Uri = new Uri(url, UriKind.Absolute)
            };
         webBrowserTask.Show();
      }

      private async void MniOpenInIE_Click(object sender, EventArgs e)
      {
         if ( ViewModel.Article != null && !string.IsNullOrWhiteSpace(ViewModel.Article.URL) )
            await OpenBrowser(ViewModel.Article.URL);
      }

      protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
      {
         if ( mobilizerPopup != null && mobilizerPopup.IsOpen )
         {
            e.Cancel = true;
            mobilizerPopup.Close();
            return ;
         }
         if ( shareService != null && shareService.IsShareOpen )
         {
            e.Cancel = true;
            shareService.CancelShare();
            return ;
         }
         base.OnBackKeyPress(e);
      }

      private void AppBarBtnShare_Click(object sender, EventArgs e)
      {
         if ( shareService == null )
            shareService = new ShareService();
         if ( shareService.IsShareOpen )
            return ;
         shareService.OnShareFinished += shareService_OnShareFinished;
         shareService.Share(ViewModel.Article);
         appBar.IsVisible = false;
      }

      void shareService_OnShareFinished(object sender, EventArgs e)
      {
         shareService.OnShareFinished -= shareService_OnShareFinished;
         appBar.IsVisible = true;
      }

      private void AppBarBtnChooseView_Click(object sender, EventArgs e)
      {
         if ( mobilizerPopup == null )
            mobilizerPopup = new MobilizerPopup();
         if ( mobilizerPopup.IsOpen )
            return ;
         mobilizerPopup.MobilizerSelected += mobilizerPopup_MobilizerSelected;
         mobilizerPopup.OnClose += mobilizerPopup_OnClose;
         mobilizerPopup.Open();
         appBar.IsVisible = false;
      }

      private void mobilizerPopup_OnClose(object sender, EventArgs e)
      {
         mobilizerPopup.MobilizerSelected -= mobilizerPopup_MobilizerSelected;
         mobilizerPopup.OnClose -= mobilizerPopup_OnClose;
         appBar.IsVisible = true;
      }

      private void mobilizerPopup_MobilizerSelected(object sender, Mobilizers e)
      {
         ViewModel.SetMobilizer(e);
      }

      private void ArticleView_Navigating(object sender, Windows.UI.Xaml.Controls.WebViewNavigationStartingEventArgs e)
      {
         SetLoading(true);
      }

      private void ArticleView_Navigated(object sender, Windows.UI.Xaml.Controls.WebViewNavigationCompletedEventArgs e)
      {
         SetLoading(false);
      }

      private void ArticleView_NavigationFailed(object sender, Windows.UI.Xaml.Controls.WebViewNavigationFailedEventArgs e)
      {
         SetLoading(false);
      }

      private void SetLoading(bool isLoading)
      {
         if ( ViewModel == null || ViewModel.Article == null || !ViewModel.Article.IsDataLoaded )
            return ;
         if ( isLoading )
            LoadingControl.StartLoading();
         else
            LoadingControl.EndLoading(null);
      }


#if DEBUG
      ~ ArticlePage()
      {
         System.Diagnostics.Debug.WriteLine("ArticlePage was killed!");
      }
      #endif
   }

}