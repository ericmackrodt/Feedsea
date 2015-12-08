using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using feedsea.Resources;
using feedsea.Common;

namespace feedsea.Views
{

   public partial class AboutPage
      : Windows.UI.Xaml.Controls.Page
   {

      public AboutPage()
      {
         InitializeComponent();
         TxtVersion.Text = " " + App.ApplicationVersion;
      }

      protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
      {
         base.OnNavigatedTo(e);
      }

      private async void HyperlinkButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
      {
         WindowsPhoneUWP.UpgradeHelpers.WebBrowserTaskHelper webBrowserTask = new WindowsPhoneUWP.UpgradeHelpers.WebBrowserTaskHelper
            {
               Uri = new Uri(InternalSettings.MyTwitterAccount)
            };
         webBrowserTask.Show();
      }

      private async void BtnEmail_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
      {
         Windows.ApplicationModel.Email.EmailMessage shareEmailTask = new Windows.ApplicationModel.Email.EmailMessage()
            {
               To =
               {
                  new Windows.ApplicationModel.Email.EmailRecipient()
               }
            };
         shareEmailTask.To[0].Address = InternalSettings.FeedbackEmail;
         await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(shareEmailTask);
      }

      private void BtnShare_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
      {
         var title = AppResources.ApplicationTitle;
         Windows.Foundation.TypedEventHandler<Windows.ApplicationModel.DataTransfer.DataTransferManager, Windows.ApplicationModel.DataTransfer.DataRequestedEventArgs> handler = null;
         handler = (Windows.ApplicationModel.DataTransfer.DataTransferManager s, Windows.ApplicationModel.DataTransfer.DataRequestedEventArgs arg) =>
            {
               arg.Request.Data.Properties.Title = title;
               arg.Request.Data.Properties.Description = AppResources.Msg_About_ShareMessage;
               arg.Request.Data.SetWebLink(new Uri(InternalSettings.AppTwitterAccount));
               s.DataRequested -= handler;
            };
         Windows.ApplicationModel.DataTransfer.DataTransferManager.GetForCurrentView().DataRequested += handler;
         Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
      }

      private async void BtnReview_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
      {
         await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:REVIEW?PFN=" + Windows.ApplicationModel.Package.Current.Id.Name));
      }

      private async void BtnTwitter_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
      {
         WindowsPhoneUWP.UpgradeHelpers.WebBrowserTaskHelper webBrowserTask = new WindowsPhoneUWP.UpgradeHelpers.WebBrowserTaskHelper
            {
               Uri = new Uri(InternalSettings.AppTwitterAccount)
            };
         webBrowserTask.Show();
      }

      private async void BtnReport_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
      {
         WindowsPhoneUWP.UpgradeHelpers.WebBrowserTaskHelper webBrowserTask = new WindowsPhoneUWP.UpgradeHelpers.WebBrowserTaskHelper
            {
               Uri = new Uri(InternalSettings.UserVoiceUrl)
            };
         webBrowserTask.Show();
      }

   }

}