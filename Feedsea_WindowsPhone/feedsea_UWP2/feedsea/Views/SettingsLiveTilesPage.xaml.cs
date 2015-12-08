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

   public partial class SettingsLiveTilesPage
      : Windows.UI.Xaml.Controls.Page
   {

      public SettingsLiveTilesViewModel ViewModel
      {
         get
         {
            return (DataContext as SettingsLiveTilesViewModel);
         }
      }


      public SettingsLiveTilesPage()
      {
         InitializeComponent();
      }

      protected override async void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
      {
         base.OnBackKeyPress(e);
         await ViewModel.ExitSettingsActions();
      }

   }

}