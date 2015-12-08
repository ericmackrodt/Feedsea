using feedsea.Common.Providers;
using feedsea.Common.Providers.Data;
using feedsea.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace feedsea.Common.Controls
{

   public class ShareService
      : IShareService
   {
      public event EventHandler OnShareFinished;
      private Windows.UI.Xaml.Controls.Primitives.Popup sharePopup;
      private SharingControl shareControl;

      public bool IsShareOpen
      {
         get
         {
            return sharePopup != null && sharePopup.IsOpen;
         }
      }


      public ShareService()
      {
         var ver = new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation().OperatingSystem.Version;
      }

      public void Share(ArticleData article)
      {
         if ( article == null || (sharePopup != null && sharePopup.IsOpen) )
            return ;
         sharePopup = new Windows.UI.Xaml.Controls.Primitives.Popup();
         shareControl = new SharingControl();
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Application.Host was not upgraded
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Application.Host was not upgraded
         shareControl.Width = Windows.UI.Xaml.Application.Current.Host.Content.ActualWidth;
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Application.Host was not upgraded
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Application.Host was not upgraded
         shareControl.Height = Windows.UI.Xaml.Application.Current.Host.Content.ActualHeight;
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Application.Host was not upgraded
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Application.Host was not upgraded
         sharePopup.Width = Windows.UI.Xaml.Application.Current.Host.Content.ActualWidth;
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Application.Host was not upgraded
         //WINDOWS_PHONE_SL_TO_UWP: (1101) System.Windows.Application.Host was not upgraded
         sharePopup.Height = Windows.UI.Xaml.Application.Current.Host.Content.ActualHeight;
         sharePopup.Child = shareControl;
         shareControl.PopupOpenEvent();
         sharePopup.IsOpen = true;
         shareControl.Load(article);
         shareControl.OnClose += ShareControl_OnClose;
      }

      private void ShareControl_OnClose(object sender, EventArgs e)
      {
         shareControl.OnClose -= ShareControl_OnClose;
         if ( OnShareFinished != null )
            OnShareFinished(this, new EventArgs());
      }

      public void CancelShare()
      {
         if ( IsShareOpen )
         {
            sharePopup.IsOpen = false;
            shareControl.OnClose -= ShareControl_OnClose;
            if ( OnShareFinished != null )
               OnShareFinished(this, new EventArgs());
         }
      }


#if DEBUG
      ~ ShareService()
      {
         System.Diagnostics.Debug.WriteLine("ShareService was killed!");
      }
      #endif
   }

}