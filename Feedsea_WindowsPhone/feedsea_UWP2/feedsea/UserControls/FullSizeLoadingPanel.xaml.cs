using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace feedsea.UserControls
{

   public partial class FullSizeLoadingPanel
      : Windows.UI.Xaml.Controls.UserControl
   {

      public bool IsOpen { get; set; }


      public FullSizeLoadingPanel()
      {
         InitializeComponent();
      }

      public void SetMessage(string message)
      {
         txtLoading.Text = message;
      }

      public void Open()
      {
         IsOpen = true;
         Visibility = Windows.UI.Xaml.Visibility.Visible;
         OpeningAnimation.Begin();
         SpinningAnimation.Begin();
      }

      public void Close()
      {
         IsOpen = false;
         ClosingAnimation.Begin();
         ClosingAnimation.Completed += (s, ev) =>
            {
               SpinningAnimation.Stop();
               Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            };
      }

   }

}