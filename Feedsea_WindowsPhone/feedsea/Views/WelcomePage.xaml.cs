using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Animation;
using System.Windows.Media;
using feedsea.Common.Helpers;
using feedsea.ViewModels;

namespace feedsea.Views
{
    public partial class WelcomePage : PhoneApplicationPage
    {
        private bool loaded = false;
        public LoginViewModel ViewModel { get { return (LoginViewModel)this.DataContext; } }

        public WelcomePage()
        {
            InitializeComponent();
            Loaded += WelcomePage_Loaded;
            ViewModel.LoginCanceled += ViewModel_LoginCanceled;
            ViewModel.OnAuthenticated += ViewModel_OnAuthenticated;
        }

        void WelcomePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (loaded)
                return;

            AccelerometerHelper.Instance.Calibrate(true, true);
            AccelerometerHelper.Instance.ReadingChanged += Instance_ReadingChanged;
            AccelerometerHelper.Instance.Active = true;

            AnimateLogo();
            Storyboard.Begin();

            loaded = true;
        }

        void ViewModel_OnAuthenticated(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/MainPage.xaml", UriKind.Relative));
        }

        void ViewModel_LoginCanceled(object sender, EventArgs e)
        {

        }

        void Instance_ReadingChanged(object sender, AccelerometerHelperReadingEventArgs e)
        {
            var x = e.AverageAcceleration.X * -64.0;
            var y = e.AverageAcceleration.Y * 64.0;

            Dispatcher.BeginInvoke(() =>
            {
                BackgroundTransform.X = Math.Max(-24, Math.Min(24, x));
                BackgroundTransform.Y = Math.Max(-24, Math.Min(24, y));
            });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            while (this.NavigationService.BackStack.Any())
            {
                this.NavigationService.RemoveBackEntry();
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
            if (ViewModel.CancelLogin())
            {
                e.Cancel = true;
                return;
            }

            base.OnBackKeyPress(e);
        }
    }
}