using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using feedsea.Resources;
using feedsea.Common;

namespace feedsea.Views
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();
            TxtVersion.Text = " " + App.ApplicationVersion;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask { Uri = new Uri(InternalSettings.MyTwitterAccount) };
            webBrowserTask.Show();
        }

        private void BtnEmail_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask shareEmailTask = new EmailComposeTask();
            shareEmailTask.To = InternalSettings.FeedbackEmail;
            shareEmailTask.Show();
        }

        private void BtnShare_Click(object sender, RoutedEventArgs e)
        {
            var title = AppResources.ApplicationTitle;
            ShareLinkTask shareLinkTask = new ShareLinkTask();
            shareLinkTask.Message = AppResources.Msg_About_ShareMessage;
            shareLinkTask.Title = title;
            shareLinkTask.LinkUri = new Uri(InternalSettings.AppTwitterAccount);
            shareLinkTask.Show();
        }

        private void BtnReview_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask rateTask = new MarketplaceReviewTask();
            rateTask.Show();
        }

        private void BtnTwitter_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask { Uri = new Uri(InternalSettings.AppTwitterAccount) };
            webBrowserTask.Show();
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask { Uri = new Uri(InternalSettings.UserVoiceUrl) };
            webBrowserTask.Show();
        }
    }
}