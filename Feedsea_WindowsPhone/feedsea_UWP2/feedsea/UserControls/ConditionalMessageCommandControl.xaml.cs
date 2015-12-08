using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;
using System.Windows.Input;

namespace feedsea.UserControls
{

   public partial class ConditionalMessageCommandControl
      : Windows.UI.Xaml.Controls.UserControl
   {
      public event Windows.UI.Xaml.RoutedEventHandler ButtonClick;

      public ConditionalMessageCommandControl()
      {
         InitializeComponent();
         btnAction.Click += BtnAction_Click;
      }

      public string Message
      {
         get
         {
            return (string)GetValue(MessageProperty);
         }
         set
         {
            SetValue(MessageProperty, value);
         }
      }

      // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
      public static readonly Windows.UI.Xaml.DependencyProperty MessageProperty = Windows.UI.Xaml.DependencyProperty.Register("Message", typeof(string), typeof(ConditionalMessageCommandControl), new PropertyMetadata(null, MessageChanged));

      private static void MessageChanged(Windows.UI.Xaml.DependencyObject d, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e)
      {
         if ( e.NewValue != e.OldValue )
            (d as ConditionalMessageCommandControl).txtMessage.Text = (string)e.NewValue;
      }

      public bool Condition
      {
         get
         {
            return (bool)GetValue(ConditionProperty);
         }
         set
         {
            SetValue(ConditionProperty, value);
         }
      }

      // Using a DependencyProperty as the backing store for Condition.  This enables animation, styling, binding, etc...
      public static readonly Windows.UI.Xaml.DependencyProperty ConditionProperty = Windows.UI.Xaml.DependencyProperty.Register("Condition", typeof(bool), typeof(ConditionalMessageCommandControl), new PropertyMetadata(true, ConditionChanged));

      private static void ConditionChanged(Windows.UI.Xaml.DependencyObject d, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e)
      {
         if ( e.NewValue != e.OldValue )
            (d as ConditionalMessageCommandControl).Visibility = (bool)e.NewValue ? Windows.UI.Xaml.Visibility.Visible : Windows.UI.Xaml.Visibility.Collapsed;
      }

      public bool SecondaryCondition
      {
         get
         {
            return (bool)GetValue(SecondaryConditionProperty);
         }
         set
         {
            SetValue(SecondaryConditionProperty, value);
         }
      }

      // Using a DependencyProperty as the backing store for SecondaryCondition.  This enables animation, styling, binding, etc...
      public static readonly Windows.UI.Xaml.DependencyProperty SecondaryConditionProperty = Windows.UI.Xaml.DependencyProperty.Register("SecondaryCondition", typeof(bool), typeof(ConditionalMessageCommandControl), new PropertyMetadata(true, SecondaryConditionChanged));

      private static void SecondaryConditionChanged(Windows.UI.Xaml.DependencyObject d, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e)
      {
         if ( (e.NewValue != e.OldValue) && (d as ConditionalMessageCommandControl).Condition )
            (d as ConditionalMessageCommandControl).Visibility = (bool)e.NewValue ? Windows.UI.Xaml.Visibility.Visible : Windows.UI.Xaml.Visibility.Collapsed;
      }

      public ImageSource ImageSource
      {
         get
         {
            return (ImageSource)GetValue(ImageSourceProperty);
         }
         set
         {
            SetValue(ImageSourceProperty, value);
         }
      }

      // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
      public static readonly Windows.UI.Xaml.DependencyProperty ImageSourceProperty = Windows.UI.Xaml.DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ConditionalMessageCommandControl), new PropertyMetadata(null, ImageSourceChanged));

      private static void ImageSourceChanged(Windows.UI.Xaml.DependencyObject d, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e)
      {
         if ( e.NewValue != e.OldValue )
         {
            (d as ConditionalMessageCommandControl).imgButton.Source = (ImageSource)e.NewValue;
         }
      }

      public ICommand ButtonCommand
      {
         get
         {
            return (ICommand)GetValue(ButtonCommandProperty);
         }
         set
         {
            SetValue(ButtonCommandProperty, value);
         }
      }

      // Using a DependencyProperty as the backing store for ButtonCommand.  This enables animation, styling, binding, etc...
      public static readonly Windows.UI.Xaml.DependencyProperty ButtonCommandProperty = Windows.UI.Xaml.DependencyProperty.Register("ButtonCommand", typeof(ICommand), typeof(ConditionalMessageCommandControl), new PropertyMetadata(null, ClickCommandChanged));

      public bool DisableButton
      {
         get
         {
            return (bool)GetValue(DisableButtonProperty);
         }
         set
         {
            SetValue(DisableButtonProperty, value);
         }
      }

      // Using a DependencyProperty as the backing store for DisableButton.  This enables animation, styling, binding, etc...
      public static readonly Windows.UI.Xaml.DependencyProperty DisableButtonProperty = Windows.UI.Xaml.DependencyProperty.Register("DisableButton", typeof(bool), typeof(ConditionalMessageCommandControl), new PropertyMetadata(false, DisableButtonChanged));

      private static void DisableButtonChanged(Windows.UI.Xaml.DependencyObject d, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e)
      {
         if ( e.NewValue != e.OldValue )
            (d as ConditionalMessageCommandControl).btnAction.IsEnabled = !(bool)e.NewValue;
      }

      private static void ClickCommandChanged(Windows.UI.Xaml.DependencyObject d, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e)
      {
         if ( e.NewValue != e.OldValue )
            (d as ConditionalMessageCommandControl).btnAction.Command = (ICommand)e.NewValue;
      }

      private void BtnAction_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
      {
         if ( ButtonClick != null )
            ButtonClick(sender, e);
      }

   }

}