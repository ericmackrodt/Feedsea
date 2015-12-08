using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using feedsea.ViewModels;

namespace feedsea.Views
{

   public partial class SettingsThirdPartyPage
      : Windows.UI.Xaml.Controls.Page
   {

      public SettingsThirdPartyViewModel ViewModel
      {
         get
         {
            return (DataContext as SettingsThirdPartyViewModel);
         }
      }


      public SettingsThirdPartyPage()
      {
         InitializeComponent();
      }

      protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
      {
         if ( ViewModel.OAuthLogin.IsOpen )
         {
            ViewModel.OAuthLogin.Cancel();
            e.Cancel = true;
            return ;
         }
         if ( ViewModel.XAuthLogin.IsOpen )
         {
            ViewModel.XAuthLogin.Cancel();
            e.Cancel = true;
            return ;
         }
         base.OnBackKeyPress(e);
      }

   }

}