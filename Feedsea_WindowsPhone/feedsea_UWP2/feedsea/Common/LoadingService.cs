using feedsea.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace feedsea.Common
{

   public class FullLoadingService
      : IFullLoadingService
   {
      Windows.UI.Xaml.Controls.Primitives.Popup popup;
      FullSizeLoadingPanel control;

      public bool IsLoading
      {
         get
         {
            return popup != null && popup.IsOpen;
         }
      }

      public void StartLoading()
      {
         StartLoading(null);
      }

      public void StartLoading(string message)
      {
         if ( popup != null && popup.IsOpen )
            return ;
         popup = new Windows.UI.Xaml.Controls.Primitives.Popup();
         control = new FullSizeLoadingPanel();
         if ( !string.IsNullOrWhiteSpace(message) )
            control.SetMessage(message);
         popup.Child = control;
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Application.Host was not upgraded
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Application.Host was not upgraded
         control.Width = Windows.UI.Xaml.Application.Current.Host.Content.ActualWidth;
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Application.Host was not upgraded
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Application.Host was not upgraded
         control.Height = Windows.UI.Xaml.Application.Current.Host.Content.ActualHeight;
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Application.Host was not upgraded
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Application.Host was not upgraded
         popup.Width = Windows.UI.Xaml.Application.Current.Host.Content.ActualWidth;
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Application.Host was not upgraded
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Application.Host was not upgraded
         popup.Height = Windows.UI.Xaml.Application.Current.Host.Content.ActualHeight;
         popup.IsOpen = true;
         control.Open();
      }

      public void EndLoading()
      {
         control.Close();
         if ( popup != null && popup.IsOpen )
            popup.IsOpen = false;
      }

   }

   public class LoadingService
      : ILoadingService
   {
      Windows.UI.Xaml.Controls.Primitives.Popup popup;
      LoadingIndicatorControl control;

      public bool IsLoading
      {
         get
         {
            return popup != null && popup.IsOpen;
         }
      }

      public void StartLoading()
      {
         StartLoading(null);
      }

      public void StartLoading(string message)
      {
         if ( popup != null && popup.IsOpen )
            return ;
         popup = new Windows.UI.Xaml.Controls.Primitives.Popup();
         control = new LoadingIndicatorControl();
         popup.Child = control;
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Application.Host was not upgraded
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Application.Host was not upgraded
         control.Width = Windows.UI.Xaml.Application.Current.Host.Content.ActualWidth;
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Application.Host was not upgraded
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Application.Host was not upgraded
         popup.Width = Windows.UI.Xaml.Application.Current.Host.Content.ActualWidth;
         popup.IsOpen = true;
         if ( !string.IsNullOrWhiteSpace(message) )
            control.StartLoading(message);
         else
            control.StartLoading();
      }

      public void EndLoading()
      {
         control.EndLoading(() =>
            {
               popup.IsOpen = false;
            });
      }

      public void EndLoading(string message)
      {
         control.EndLoading(message, () =>
            {
               popup.IsOpen = false;
            });
      }

   }

}