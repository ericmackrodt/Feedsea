using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace feedsea.UserControls
{
    public partial class AdControl : UserControl
    {
        public AdControl()
        {
            InitializeComponent();
            Unloaded += AdControl_Unloaded;
        }

        void AdControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= AdControl_Unloaded;
        }
        
        public bool IsAdsEnabled
        {
            get { return (bool)GetValue(IsAdsEnabledProperty); }
            set { SetValue(IsAdsEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsAdsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAdsEnabledProperty =
            DependencyProperty.Register("IsAdsEnabled", typeof(bool), typeof(AdControl), new PropertyMetadata(true, AdsEnabledChanged));

        private static void AdsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as AdControl;
            if ((bool)e.NewValue)
            {
                ctrl.Visibility = Visibility.Visible;
                ctrl.AdDuplex.IsEnabled = false;
                ctrl.AdDuplex.Visibility = Visibility.Visible;
                //ctrl.AdRotator.AutoStartAds = true;
            }
            else
            {
                ctrl.Visibility = Visibility.Collapsed;
                ctrl.AdDuplex.IsEnabled = false;
                ctrl.AdDuplex.Visibility = Visibility.Collapsed;
                //ctrl.AdRotator.IsAdRotatorEnabled = false;
                //ctrl.AdRotator.AutoStartAds = false;
            }
        }
    }
}
