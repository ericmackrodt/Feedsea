﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Feedsea.Common.Helpers
{
    public static class TitleBar
    {
        public static readonly DependencyProperty IsChromelessProperty =
            DependencyProperty.RegisterAttached("IsChromeless", typeof(bool),
                typeof(TitleBar),
                new PropertyMetadata(null, OnIsChromelessChanged));

        public static bool GetIsChromeless(DependencyObject d)
        {
            return (bool)d.GetValue(IsChromelessProperty);
        }

        public static void SetIsChromeless(DependencyObject d, bool value)
        {
            d.SetValue(IsChromelessProperty, value);
        }

        private static void OnIsChromelessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var isChromeless = (bool)e.NewValue;
            var titleBar = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar;
            titleBar.ExtendViewIntoTitleBar = isChromeless;
        }

        public static readonly DependencyProperty ForegroundColorProperty =
            DependencyProperty.RegisterAttached("ForegroundColor", typeof(Color),
            typeof(TitleBar),
            new PropertyMetadata(null, OnForegroundColorPropertyChanged));

        public static Color GetForegroundColor(DependencyObject d)
        {
            return (Color)d.GetValue(ForegroundColorProperty);
        }

        public static void SetForegroundColor(DependencyObject d, Color value)
        {
            d.SetValue(ForegroundColorProperty, value);
        }

        private static void OnForegroundColorPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var color = (Color)e.NewValue;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ForegroundColor = color;
        }

        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.RegisterAttached("BackgroundColor", typeof(Color),
            typeof(TitleBar),
            new PropertyMetadata(null, OnBackgroundColorPropertyChanged));

        public static Color GetBackgroundColor(DependencyObject d)
        {
            return (Color)d.GetValue(BackgroundColorProperty);
        }

        public static void SetBackgroundColor(DependencyObject d, Color value)
        {
            d.SetValue(BackgroundColorProperty, value);
        }

        private static void OnBackgroundColorPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var color = (Color)e.NewValue;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = color;
        }

        public static readonly DependencyProperty ButtonForegroundColorProperty =
            DependencyProperty.RegisterAttached("ButtonForegroundColor", typeof(Color),
            typeof(TitleBar),
            new PropertyMetadata(null, OnButtonForegroundColorPropertyChanged));

        public static Color GetButtonForegroundColor(DependencyObject d)
        {
            return (Color)d.GetValue(ButtonForegroundColorProperty);
        }

        public static void SetButtonForegroundColor(DependencyObject d, Color value)
        {
            d.SetValue(ButtonForegroundColorProperty, value);
        }

        private static void OnButtonForegroundColorPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var color = (Color)e.NewValue;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonForegroundColor = color;
        }

        public static readonly DependencyProperty ButtonBackgroundColorProperty =
            DependencyProperty.RegisterAttached("ButtonBackgroundColor", typeof(Color),
            typeof(TitleBar),
            new PropertyMetadata(null, OnButtonBackgroundColorPropertyChanged));

        public static Color GetButtonBackgroundColor(DependencyObject d)
        {
            return (Color)d.GetValue(ButtonBackgroundColorProperty);
        }

        public static void SetButtonBackgroundColor(DependencyObject d, Color value)
        {
            d.SetValue(ButtonBackgroundColorProperty, value);
        }

        private static void OnButtonBackgroundColorPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var color = (Color)e.NewValue;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = color;
        }
    }
}
