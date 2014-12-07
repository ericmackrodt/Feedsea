using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Input;
using feedsea.Common.Providers;
using feedsea.Resources;

namespace feedsea.UserControls
{
    public partial class FeedlyLoginControl : UserControl
    {
        #region DependencyProperties

        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.Register("IsVisible", typeof(bool), typeof(FeedlyLoginControl), new PropertyMetadata(false, IsVisibleChanged));

        private static void IsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FeedlyLoginControl).Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public IOauthLoginData LoginData
        {
            get { return (IOauthLoginData)GetValue(LoginDataProperty); }
            set { SetValue(LoginDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LoginData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoginDataProperty =
            DependencyProperty.Register("LoginData", typeof(IOauthLoginData), typeof(FeedlyLoginControl), new PropertyMetadata(null, LoginDataChanged));

        private static void LoginDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FeedlyLoginControl).Navigate();
        }

        public ICommand AuthenticatedCommand
        {
            get { return (ICommand)GetValue(AuthenticatedCommandProperty); }
            set { SetValue(AuthenticatedCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AuthenticatedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AuthenticatedCommandProperty =
            DependencyProperty.Register("AuthenticatedCommand", typeof(ICommand), typeof(FeedlyLoginControl), new PropertyMetadata(null));

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLoading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(FeedlyLoginControl), new PropertyMetadata(false));
        
        #endregion DependencyProperties

        public FeedlyLoginControl()
        {
            InitializeComponent();
        }

        private void LoginBrowser_Navigating(object sender, NavigatingEventArgs e)
        {
            IsLoading = true;

            if (e.Uri.ToString().StartsWith(LoginData.RedirectUrl) && AuthenticatedCommand.CanExecute(null))
            {
                AuthenticatedCommand.Execute(e.Uri);
                IsLoading = false;
            }
        }

        private void Navigate()
        {
            LoginBrowser.Navigate(new Uri(LoginData.LoginUrl));
        }

        private void LoginBrowser_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            LoginBrowser.NavigateToString(string.Format("<html><body><div>{0}</div></body></html>", AppResources.Msg_CouldntLoadLoginPage));
            IsLoading = false;
        }

        private void LoginBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            IsLoading = false;
        }
    }
}
