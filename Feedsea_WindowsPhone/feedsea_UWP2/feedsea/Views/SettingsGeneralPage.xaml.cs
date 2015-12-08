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

   public partial class SettingsGeneralPage
      : Windows.UI.Xaml.Controls.Page
   {

      public SettingsGeneralViewModel ViewModel
      {
         get
         {
            return (DataContext as SettingsGeneralViewModel);
         }
      }


      public SettingsGeneralPage()
      {
         InitializeComponent();
      }

      protected override async void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
      {
         if ( e.NavigationMode == NavigationMode.New )
         {
            await ViewModel.LoadDataAsync();
         }
      }

   }

}