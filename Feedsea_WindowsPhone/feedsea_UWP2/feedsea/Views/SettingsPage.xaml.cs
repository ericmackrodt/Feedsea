using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using feedsea.ViewModels;
using feedsea.Common.MVVM.Tombstone;
using Coding4Fun.Toolkit.Controls;
using feedsea.Resources;
using feedsea.Models;
using feedsea.Common;

namespace feedsea.Views
{

   public partial class SettingsPage
      : Windows.UI.Xaml.Controls.Page
   {

      public SettingsViewModel ViewModel
      {
         get
         {
            return (DataContext as SettingsViewModel);
         }
      }


      public SettingsPage()
      {
         InitializeComponent();
         ViewModel.LoggedOut += ViewModel_LoggedOut;
      }

      protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
      {
         base.OnNavigatedTo(e);
         var settingsItems = GetSettingsItems();
         ViewModel.LoadData(settingsItems);
      }

      private IEnumerable<SettingItemModel> GetSettingsItems()
      {
         var items = new List<SettingItemModel>();
         items.Add(new SettingItemModel()
            {
               Title = AppResources.Lbl_SettingsPage_General, Description = AppResources.Lbl_SettingsPage_General_Description, NavigationUri = "/Views/SettingsGeneralPage.xaml", Type = ItemType.AppPage
            });
         items.Add(new SettingItemModel()
            {
               Title = AppResources.Lbl_SettingsPage_Appearance, Description = AppResources.Lbl_SettingsPage_Appearance_Description, NavigationUri = "/Views/SettingsAppearancePage.xaml", Type = ItemType.AppPage
            });
         items.Add(new SettingItemModel()
            {
               Title = AppResources.Lbl_SettingsPage_ThirdParty, Description = AppResources.Lbl_SettingsPage_ThirdParty_Description, NavigationUri = "/Views/SettingsThirdPartyPage.xaml", Type = ItemType.AppPage
            });
         items.Add(new SettingItemModel()
            {
               Title = AppResources.Lbl_SettingsPage_LiveTiles, Description = AppResources.Lbl_SettingsPage_LiveTiles_Description, NavigationUri = "/Views/SettingsLiveTilesPage.xaml", Type = ItemType.AppPage
            });
         if ( ViewModel.IsAdsEnabled )
         {
            items.Add(new SettingItemModel()
               {
                  Title = AppResources.Lbl_SettingsPage_DisableAds, Description = AppResources.Lbl_SettingsPage_DisableAds_Description, NavigationUri = "", Type = ItemType.Purchase
               });
         }
         items.Add(new SettingItemModel()
            {
               Title = AppResources.Lbl_SettingsPage_About, Description = string.Format(AppResources.Lbl_SettingsPage_About_Description, App.ApplicationVersion), NavigationUri = "/Views/AboutPage.xaml", Type = ItemType.AppPage
            });
         items.Add(new SettingItemModel()
            {
               Title = AppResources.Lbl_SettingsPage_Feedback, Description = AppResources.Lbl_SettingsPage_Feedback_Description, NavigationUri = InternalSettings.FeedbackEmail, Type = ItemType.Email
            });
         items.Add(new SettingItemModel()
            {
               Title = AppResources.Lbl_SettingsPage_Uservoice, Description = AppResources.Lbl_SettingsPage_Uservoice_Description, NavigationUri = InternalSettings.UserVoiceUrl, Type = ItemType.WebPage
            });
         items.Add(new SettingItemModel()
            {
               Title = AppResources.Lbl_SettingsPage_Logout, Description = AppResources.Lbl_SettingsPage_Logout_Description, NavigationUri = "", Type = ItemType.Logoff
            });
         return items;
      }

      private async void ListBox_SelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
      {
         var item = LbSettings.SelectedItem as SettingItemModel;
         if ( item == null )
            return ;
         LbSettings.SelectedIndex = -1;
         switch ( item.Type )
         {
            case ItemType.Email:
               await SendEmail(item);
               return ;
            case ItemType.Logoff:
               Logoff(item);
               return ;
            case ItemType.Purchase:
               PurchaseApp(item);
               return ;
            case ItemType.WebPage:
               await NavigateToWebPage(item);
               return ;
            case ItemType.AppPage:
            default:
               NavigateToPage(item);
               return ;
         }
      }

      private void NavigateToPage(SettingItemModel item)
      {
         WP8SL_TO_UWP.NavigationHelper.NavigateTo(new Uri(new Uri("ms-appx://"), item.NavigationUri));
      }

      private async System.Threading.Tasks.Task SendEmail(SettingItemModel item)
      {
         Windows.ApplicationModel.Email.EmailMessage shareEmailTask = new Windows.ApplicationModel.Email.EmailMessage()
            {
               To =
               {
                  new Windows.ApplicationModel.Email.EmailRecipient()
               }
            };
         shareEmailTask.To[0].Address = item.NavigationUri;
         await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(shareEmailTask);
      }

      private void Logoff(SettingItemModel item)
      {
         if ( ViewModel.LogoutCommand.CanExecute(null) )
            ViewModel.LogoutCommand.Execute(null);
      }

      private void PurchaseApp(SettingItemModel item)
      {
         if ( ViewModel.DisableAdsCommand.CanExecute(null) )
            ViewModel.DisableAdsCommand.Execute(null);
      }

      private async System.Threading.Tasks.Task NavigateToWebPage(SettingItemModel item)
      {
         WindowsPhoneUWP.UpgradeHelpers.WebBrowserTaskHelper webBrowserTask = new WindowsPhoneUWP.UpgradeHelpers.WebBrowserTaskHelper
            {
               Uri = new Uri(item.NavigationUri)
            };
         webBrowserTask.Show();
      }

      private void ViewModel_LoggedOut(object sender, EventArgs e)
      {
         Frame.Navigate(typeof(feedsea.Views.WelcomePage));
      }

   }

}