using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace feedsea.Common
{

   public static class WebBrowserHelper
   {
      public static readonly Windows.UI.Xaml.DependencyProperty UrlProperty = Windows.UI.Xaml.DependencyProperty.RegisterAttached("Url", typeof(string), typeof(WebBrowserHelper), new PropertyMetadata(OnUrlChanged));

      public static string GetUrl(Windows.UI.Xaml.DependencyObject dependencyObject)
      {
         return (string)dependencyObject.GetValue(UrlProperty);
      }

      public static void SetUrl(Windows.UI.Xaml.DependencyObject dependencyObject, string value)
      {
         dependencyObject.SetValue(UrlProperty, value);
      }

      private static void OnUrlChanged(Windows.UI.Xaml.DependencyObject d, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e)
      {
         var browser = d as Windows.UI.Xaml.Controls.WebView;
         if ( browser == null || e.NewValue == null )
            return ;
         var url = e.NewValue.ToString();
         browser.Navigate(new Uri(url));
      }

      public static readonly Windows.UI.Xaml.DependencyProperty HtmlProperty = Windows.UI.Xaml.DependencyProperty.RegisterAttached("Html", typeof(string), typeof(WebBrowserHelper), new PropertyMetadata(OnHtmlChanged));

      public static string GetHtml(Windows.UI.Xaml.DependencyObject dependencyObject)
      {
         return (string)dependencyObject.GetValue(HtmlProperty);
      }

      public static void SetHtml(Windows.UI.Xaml.DependencyObject dependencyObject, string value)
      {
         dependencyObject.SetValue(HtmlProperty, value);
      }

      private static void OnHtmlChanged(Windows.UI.Xaml.DependencyObject d, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e)
      {
         var browser = d as Windows.UI.Xaml.Controls.WebView;
         if ( browser == null )
            return ;
         if ( e.NewValue != null )
         {
            var html = e.NewValue.ToString();
            browser.NavigateToString(html);
         }
      }

      public static readonly Windows.UI.Xaml.DependencyProperty AnimatedOpacityProperty = Windows.UI.Xaml.DependencyProperty.RegisterAttached("AnimatedOpacity", typeof(bool), typeof(WebBrowserHelper), new PropertyMetadata(OnAnimatedOpacityPropertyChange));

      public static bool GetAnimatedOpacity(Windows.UI.Xaml.DependencyObject dependencyObject)
      {
         return (bool)dependencyObject.GetValue(HtmlProperty);
      }

      public static void SetAnimatedOpacity(Windows.UI.Xaml.DependencyObject dependencyObject, string value)
      {
         dependencyObject.SetValue(HtmlProperty, value);
      }

      private static void OnAnimatedOpacityPropertyChange(Windows.UI.Xaml.DependencyObject d, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e)
      {
         var browser = d as Windows.UI.Xaml.Controls.WebView;
         if ( browser == null )
            return ;
         if ( (bool)e.NewValue )
         {
            var sb = AnimateOpacity(browser);
            sb.Begin();
         }
         else
            browser.Opacity = 0;
      }

      private static Windows.UI.Xaml.Media.Animation.Storyboard AnimateOpacity(Windows.UI.Xaml.Controls.WebView browser)
      {
         var storyboard = new Windows.UI.Xaml.Media.Animation.Storyboard();
         var duration = new TimeSpan(0, 0, 0, 0, 500);
         var animation = new Windows.UI.Xaml.Media.Animation.DoubleAnimation()
            {
               EnableDependentAnimation = true
            };
         animation.From = 0.0;
         animation.To = 1.0;
         animation.Duration = new Windows.UI.Xaml.Duration(duration);
         Windows.UI.Xaml.Media.Animation.Storyboard.SetTarget(animation, browser);
         Windows.UI.Xaml.Media.Animation.Storyboard.SetTargetProperty(animation, new PropertyPath(Windows.UI.Xaml.Controls.WebView.OpacityProperty).Path);
         storyboard.Children.Add(animation);
         return storyboard;
      }

   }

}