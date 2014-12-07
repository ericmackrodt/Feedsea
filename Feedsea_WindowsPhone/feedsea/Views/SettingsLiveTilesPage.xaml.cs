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
    public partial class SettingsLiveTilesPage : PhoneApplicationPage
    {
        public SettingsLiveTilesViewModel ViewModel { get { return (DataContext as SettingsLiveTilesViewModel); } }

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