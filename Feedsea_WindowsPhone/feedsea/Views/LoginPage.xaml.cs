using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using feedsea.ViewModels;
using feedsea.Common.MVVM.Tombstone;

namespace feedsea.Views
{
    public partial class LoginPage : PhoneApplicationPage
    {
        public LoginViewModel ViewModel { get { return (LoginViewModel)this.DataContext; } }

        public LoginPage()
        {
            InitializeComponent();
            ViewModel.OnAuthenticated += ViewModel_OnAuthenticated;
        }

        void ViewModel_OnAuthenticated(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/MainPage.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            TombstoneHelper.page_OnNavigatedTo(this, e);
            if (e.NavigationMode == NavigationMode.New)
            {
                ViewModel.LoadData();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            TombstoneHelper.page_OnNavigatedFrom(this, e);
        }
    }
}