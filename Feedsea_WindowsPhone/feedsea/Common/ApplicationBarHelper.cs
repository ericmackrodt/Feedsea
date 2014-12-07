using Cimbalino.Phone.Toolkit.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace feedsea.Common
{
    public static class ApplicationBarHelper
    {
        public static string GetIconTrue(DependencyObject obj)
        {
            return (string)obj.GetValue(IconTrueProperty);
        }

        public static void SetIconTrue(DependencyObject obj, string value)
        {
            obj.SetValue(IconTrueProperty, value);
        }

        // Using a DependencyProperty as the backing store for IconTrue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconTrueProperty =
            DependencyProperty.RegisterAttached("IconTrue", typeof(string), typeof(ApplicationBarHelper), new PropertyMetadata(""));

        public static string GetTextTrue(DependencyObject obj)
        {
            return (string)obj.GetValue(TextTrueProperty);
        }

        public static void SetTextTrue(DependencyObject obj, string value)
        {
            obj.SetValue(TextTrueProperty, value);
        }

        // Using a DependencyProperty as the backing store for TextTrue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextTrueProperty =
            DependencyProperty.RegisterAttached("TextTrue", typeof(string), typeof(ApplicationBarHelper), new PropertyMetadata(""));

        public static string GetIconFalse(DependencyObject obj)
        {
            return (string)obj.GetValue(IconFalseProperty);
        }

        public static void SetIconFalse(DependencyObject obj, string value)
        {
            obj.SetValue(IconFalseProperty, value);
        }

        // Using a DependencyProperty as the backing store for IconFalse.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconFalseProperty =
            DependencyProperty.RegisterAttached("IconFalse", typeof(string), typeof(ApplicationBarHelper), new PropertyMetadata(""));

        public static string GetTextFalse(DependencyObject obj)
        {
            return (string)obj.GetValue(TextFalseProperty);
        }

        public static void SetTextFalse(DependencyObject obj, string value)
        {
            obj.SetValue(TextFalseProperty, value);
        }

        // Using a DependencyProperty as the backing store for TextFalse.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextFalseProperty =
            DependencyProperty.RegisterAttached("TextFalse", typeof(string), typeof(ApplicationBarHelper), new PropertyMetadata(""));
        
        public static readonly DependencyProperty ConditionProperty = DependencyProperty.RegisterAttached(
            "Condition", typeof(bool?), typeof(WebBrowserHelper), new PropertyMetadata(null, OnConditionChanged));

        public static bool? GetCondition(DependencyObject dependencyObject)
        {
            return (bool?)dependencyObject.GetValue(ConditionProperty);
        }

        public static void SetCondition(DependencyObject dependencyObject, bool? value)
        {
            dependencyObject.SetValue(ConditionProperty, value);
        }

        private static void OnConditionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bar = d as ApplicationBarIconButton;

            if (bar == null || e.NewValue == null)
                return;
            
            bar.IconUri = new Uri((bool)e.NewValue ? GetIconTrue(bar) : GetIconFalse(bar), UriKind.Relative);
            bar.Text = (bool)e.NewValue ? GetTextTrue(bar) : GetTextFalse(bar);
        }
    }
}
