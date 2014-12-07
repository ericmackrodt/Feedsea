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
using Coding4Fun.Toolkit.Controls;
using feedsea.Resources;

namespace feedsea.Views
{
    public partial class SettingsAppearancePage : PhoneApplicationPage
    {
        public SettingsAppearanceViewModel ViewModel { get { return (DataContext as SettingsAppearanceViewModel); } }

        public SettingsAppearancePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.ThemeChanged += ViewModel_ThemeChanged;
        }

        void ViewModel_ThemeChanged(object sender, EventArgs e)
        {
            try
            {
                var toast = new ToastPrompt();
                toast.Message = AppResources.Msg_YouHaveToRestartToApplyTheme;
                toast.TextWrapping = TextWrapping.Wrap;
                toast.Tap += (s, args) =>
                {
                    Application.Current.Terminate();
                };
                toast.Show();
            }
            catch
            {

            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.ThemeChanged -= ViewModel_ThemeChanged;
        }
    }
}