using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace feedsea.Common
{
    public static class WebBrowserHelper
    {
        public static readonly DependencyProperty UrlProperty = DependencyProperty.RegisterAttached(
            "Url", typeof(string), typeof(WebBrowserHelper), new PropertyMetadata(OnUrlChanged));

        public static string GetUrl(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(UrlProperty);
        }

        public static void SetUrl(DependencyObject dependencyObject, string value)
        {
            dependencyObject.SetValue(UrlProperty, value);
        }

        private static void OnUrlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var browser = d as WebBrowser;

            if (browser == null)
                return;

            var url = e.NewValue.ToString();

            browser.Navigate(new Uri(url));
        }

        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
            "Html", typeof(string), typeof(WebBrowserHelper), new PropertyMetadata(OnHtmlChanged));

        public static string GetHtml(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(HtmlProperty);
        }

        public static void SetHtml(DependencyObject dependencyObject, string value)
        {
            dependencyObject.SetValue(HtmlProperty, value);
        }

        private static void OnHtmlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var browser = d as WebBrowser;

            if (browser == null)
                return;

            if (e.NewValue != null)
            {
                var html = e.NewValue.ToString();

                browser.NavigateToString(html);
            }
        }

        public static readonly DependencyProperty AnimatedOpacityProperty = DependencyProperty.RegisterAttached(
            "AnimatedOpacity", typeof(bool), typeof(WebBrowserHelper), new PropertyMetadata(OnAnimatedOpacityPropertyChange));

        public static bool GetAnimatedOpacity(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(HtmlProperty);
        }

        public static void SetAnimatedOpacity(DependencyObject dependencyObject, string value)
        {
            dependencyObject.SetValue(HtmlProperty, value);
        }

        private static void OnAnimatedOpacityPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var browser = d as WebBrowser;

            if (browser == null || !(bool)e.NewValue) return;

            var sb = AnimateOpacity(browser);
            sb.Begin();
        }

        private static Storyboard AnimateOpacity(WebBrowser browser)
        {
            var storyboard = new Storyboard();
            var duration = new TimeSpan(0, 0, 0, 0, 500);

            var animation = new DoubleAnimation();
            animation.From = 0.0;
            animation.To = 1.0;
            animation.Duration = new Duration(duration);
            Storyboard.SetTarget(animation, browser);
            Storyboard.SetTargetProperty(animation, new PropertyPath(WebBrowser.OpacityProperty));
            storyboard.Children.Add(animation);
            return storyboard;
        }
    }
}
