using Cimbalino.Phone.Toolkit.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace feedsea.Common
{

   public static class ApplicationBarHelper
   {

      public static string GetIconTrue(Windows.UI.Xaml.DependencyObject obj)
      {
         return (string)obj.GetValue(IconTrueProperty);
      }

      public static void SetIconTrue(Windows.UI.Xaml.DependencyObject obj, string value)
      {
         obj.SetValue(IconTrueProperty, value);
      }

      // Using a DependencyProperty as the backing store for IconTrue.  This enables animation, styling, binding, etc...
      public static readonly Windows.UI.Xaml.DependencyProperty IconTrueProperty = Windows.UI.Xaml.DependencyProperty.RegisterAttached("IconTrue", typeof(string), typeof(ApplicationBarHelper), new PropertyMetadata(""));

      public static string GetTextTrue(Windows.UI.Xaml.DependencyObject obj)
      {
         return (string)obj.GetValue(TextTrueProperty);
      }

      public static void SetTextTrue(Windows.UI.Xaml.DependencyObject obj, string value)
      {
         obj.SetValue(TextTrueProperty, value);
      }

      // Using a DependencyProperty as the backing store for TextTrue.  This enables animation, styling, binding, etc...
      public static readonly Windows.UI.Xaml.DependencyProperty TextTrueProperty = Windows.UI.Xaml.DependencyProperty.RegisterAttached("TextTrue", typeof(string), typeof(ApplicationBarHelper), new PropertyMetadata(""));

      public static string GetIconFalse(Windows.UI.Xaml.DependencyObject obj)
      {
         return (string)obj.GetValue(IconFalseProperty);
      }

      public static void SetIconFalse(Windows.UI.Xaml.DependencyObject obj, string value)
      {
         obj.SetValue(IconFalseProperty, value);
      }

      // Using a DependencyProperty as the backing store for IconFalse.  This enables animation, styling, binding, etc...
      public static readonly Windows.UI.Xaml.DependencyProperty IconFalseProperty = Windows.UI.Xaml.DependencyProperty.RegisterAttached("IconFalse", typeof(string), typeof(ApplicationBarHelper), new PropertyMetadata(""));

      public static string GetTextFalse(Windows.UI.Xaml.DependencyObject obj)
      {
         return (string)obj.GetValue(TextFalseProperty);
      }

      public static void SetTextFalse(Windows.UI.Xaml.DependencyObject obj, string value)
      {
         obj.SetValue(TextFalseProperty, value);
      }

      // Using a DependencyProperty as the backing store for TextFalse.  This enables animation, styling, binding, etc...
      public static readonly Windows.UI.Xaml.DependencyProperty TextFalseProperty = Windows.UI.Xaml.DependencyProperty.RegisterAttached("TextFalse", typeof(string), typeof(ApplicationBarHelper), new PropertyMetadata(""));
      public static readonly Windows.UI.Xaml.DependencyProperty ConditionProperty = Windows.UI.Xaml.DependencyProperty.RegisterAttached("Condition", typeof(bool?), typeof(WebBrowserHelper), new PropertyMetadata(null, OnConditionChanged));

      public static bool? GetCondition(Windows.UI.Xaml.DependencyObject dependencyObject)
      {
         return (bool?)dependencyObject.GetValue(ConditionProperty);
      }

      public static void SetCondition(Windows.UI.Xaml.DependencyObject dependencyObject, bool? value)
      {
         dependencyObject.SetValue(ConditionProperty, value);
      }

      private static void OnConditionChanged(Windows.UI.Xaml.DependencyObject d, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e)
      {
         var bar = d as ApplicationBarIconButton;
         if ( bar == null || e.NewValue == null )
            return ;
         bar.IconUri = new Uri(new Uri("ms-appx://"), (bool)e.NewValue ? GetIconTrue(bar) : GetIconFalse(bar));
         bar.Text = (bool)e.NewValue ? GetTextTrue(bar) : GetTextFalse(bar);
      }

   }

}