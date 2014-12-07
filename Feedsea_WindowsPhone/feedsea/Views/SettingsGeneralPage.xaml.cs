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

namespace feedsea.Views
{
    public partial class SettingsGeneralPage : PhoneApplicationPage
    {
        public SettingsGeneralViewModel ViewModel { get { return (DataContext as SettingsGeneralViewModel); } }

        public SettingsGeneralPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                await ViewModel.LoadDataAsync();
            }
        }
    }
}