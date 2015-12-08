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

   public class MobilizerPopup
   {
      public event EventHandler<Mobilizers> MobilizerSelected;
      public event EventHandler OnClose;
      Windows.UI.Xaml.Controls.Primitives.Popup popup;
      MobilizerMenuControl control;

      public bool IsOpen
      {
         get
         {
            return popup != null && popup.IsOpen;
         }
      }

      public void Open()
      {
         if ( IsOpen )
            return ;
         popup = new Windows.UI.Xaml.Controls.Primitives.Popup();
         control = new MobilizerMenuControl();
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
         popup.Child = control;
         control.PopupOpenEvent();
         popup.IsOpen = true;
         control.MobilizerSelected += control_MobilizerSelected;
      }

      public void Close()
      {
         if ( IsOpen )
         {
            control.Close();
            if ( OnClose != null )
               OnClose(this, new EventArgs());
         }
      }

      private void control_MobilizerSelected(object sender, Mobilizers e)
      {
         control.MobilizerSelected -= control_MobilizerSelected;
         control.Close();
         if ( MobilizerSelected != null )
            MobilizerSelected(this, e);
         if ( OnClose != null )
            OnClose(this, new EventArgs());
      }

   }

}