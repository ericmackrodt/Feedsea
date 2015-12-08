using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media;
using feedsea.Common.Helpers;
using feedsea.ViewModels;

namespace feedsea.Views
{

   public partial class WelcomePage
      : Windows.UI.Xaml.Controls.Page
   {
      private bool loaded = false;

      public LoginViewModel ViewModel
      {
         get
         {
            return (LoginViewModel)this.DataContext;
         }
      }


      public WelcomePage()
      {
         InitializeComponent();
         Loaded += WelcomePage_Loaded;
         ViewModel.LoginCanceled += ViewModel_LoginCanceled;
         ViewModel.OnAuthenticated += ViewModel_OnAuthenticated;
      }

      void WelcomePage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
      {
         if ( loaded )
            return ;
         AccelerometerHelper.Instance.Calibrate(true, true);
         AccelerometerHelper.Instance.ReadingChanged += Instance_ReadingChanged;
         AccelerometerHelper.Instance.Active = true;
         AnimateLogo();
         Windows.UI.Xaml.Media.Animation.Storyboard.Begin();
         loaded = true;
      }

      void ViewModel_OnAuthenticated(object sender, EventArgs e)
      {
         Frame.Navigate(typeof(feedsea.Views.MainPage));
      }

      void ViewModel_LoginCanceled(object sender, EventArgs e)
      {
      }

      void Instance_ReadingChanged(object sender, AccelerometerHelperReadingEventArgs e)
      {
         var x = e.AverageAcceleration.X * -64.0;
         var y = e.AverageAcceleration.Y * 64.0;
         Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
               BackgroundTransform.X = Math.Max(-24, Math.Min(24, x));
               BackgroundTransform.Y = Math.Max(-24, Math.Min(24, y));
            });
      }

      protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
      {
         while ( this.NavigationService.BackStack.Any() )
         {
            //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Navigation.NavigationService.BackStack was not upgraded
            //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Navigation.NavigationService.BackStack was not upgraded
            ((Windows.UI.Xaml.Controls.Frame)Windows.UI.Xaml.Window.Current.Content).BackStack.RemoveAt(((Windows.UI.Xaml.Controls.Frame)Windows.UI.Xaml.Window.Current.Content).BackStack.Count - 1);
         }
      }

      private void AnimateLogo()
      {
         double height = LayoutRoot.ActualHeight;
         double logoSize = 48;
         var logoMidTop = (height / 2) - (logoSize / 2);
         var logoTopTop = (height / 4) - (logoSize / 2);
         LogoAnimation.From = 0;
         LogoAnimation.To = (logoMidTop - logoTopTop) * -1;
      }

      protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
      {
         if ( ViewModel.CancelLogin() )
         {
            e.Cancel = true;
            return ;
         }
         base.OnBackKeyPress(e);
      }

   }

}