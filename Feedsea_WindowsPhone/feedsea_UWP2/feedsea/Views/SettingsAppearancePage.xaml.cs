using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using feedsea.ViewModels;
using Coding4Fun.Toolkit.Controls;
using feedsea.Resources;

namespace feedsea.Views
{

   public partial class SettingsAppearancePage
      : Windows.UI.Xaml.Controls.Page
   {

      public SettingsAppearanceViewModel ViewModel
      {
         get
         {
            return (DataContext as SettingsAppearanceViewModel);
         }
      }


      public SettingsAppearancePage()
      {
         InitializeComponent();
      }

      protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
      {
         ViewModel.ThemeChanged += ViewModel_ThemeChanged;
      }

      void ViewModel_ThemeChanged(object sender, EventArgs e)
      {
         try
         {
            var toast = new ToastPrompt();
            toast.Message = AppResources.Msg_YouHaveToRestartToApplyTheme;
            toast.TextWrapping = Windows.UI.Xaml.TextWrapping.Wrap;
            toast.Tapped += (s, args) =>
               {
                  Windows.UI.Xaml.Application.Current.Exit();
               };
            toast.Show();
         }
         catch
         {
         }
      }

      protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
      {
         ViewModel.ThemeChanged -= ViewModel_ThemeChanged;
      }

   }

}