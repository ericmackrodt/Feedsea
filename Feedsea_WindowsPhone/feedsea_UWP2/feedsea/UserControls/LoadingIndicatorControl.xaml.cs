using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;

namespace feedsea.UserControls
{

   public partial class LoadingIndicatorControl
      : Windows.UI.Xaml.Controls.UserControl
   {
      private bool _loadingState = false;

      public LoadingIndicatorControl()
      {
         InitializeComponent();
      }

      public bool IsBusy
      {
         get
         {
            return (bool)GetValue(IsBusyProperty);
         }
         set
         {
            SetValue(IsBusyProperty, value);
         }
      }

      public static readonly Windows.UI.Xaml.DependencyProperty IsBusyProperty = Windows.UI.Xaml.DependencyProperty.Register("IsBusy", typeof(bool), typeof(LoadingIndicatorControl), new PropertyMetadata(false, OnIsBusyChanged));

      private static void OnIsBusyChanged(Windows.UI.Xaml.DependencyObject d, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e)
      {
         if ( e.NewValue == e.OldValue )
            return ;
         var control = (d as LoadingIndicatorControl);
         if ( (bool)e.NewValue )
         {
            control.StartLoading();
         }
         else
         {
            control.EndLoading(null);
         }
      }

      public void StartLoading()
      {
         if ( _loadingState )
            return ;
         _loadingState = true;
         Visibility = Windows.UI.Xaml.Visibility.Visible;
         SpinningAnimation.Begin();
         OpeningAnimation.Begin();
      }

      public void StartLoading(string message)
      {
         txtLoading.Text = message;
         StartLoading();
      }

      public void EndLoading(Action done)
      {
         if ( !_loadingState )
            return ;
         ClosingAnimation.Begin();
         ClosingAnimation.Completed += (s, ev) =>
            {
               SpinningAnimation.Stop();
               Visibility = Windows.UI.Xaml.Visibility.Collapsed;
               _loadingState = false;
               if ( done != null )
                  done();
            };
      }

      public void EndLoading(string message, Action done)
      {
         DoneAnimation.Begin();
         txtLoading.Text = message;
         ClosingAnimation.Completed += (s, ev) =>
            {
               SpinningAnimation.Stop();
               Visibility = Windows.UI.Xaml.Visibility.Collapsed;
               if ( done != null )
                  done();
            };
      }

   }

}