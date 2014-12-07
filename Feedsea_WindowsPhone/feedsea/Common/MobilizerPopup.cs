using feedsea.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace feedsea.Common
{
    public class MobilizerPopup
    {
        public event EventHandler<Mobilizers> MobilizerSelected;
        public event EventHandler OnClose;

        Popup popup;
        MobilizerMenuControl control;

        public bool IsOpen
        {
            get { return popup != null && popup.IsOpen; }
        }

        public void Open() 
        {
            if (IsOpen)
                return;

            popup = new Popup();
            control = new MobilizerMenuControl();
            control.Width = Application.Current.Host.Content.ActualWidth;
            control.Height = Application.Current.Host.Content.ActualHeight;
            popup.Width = Application.Current.Host.Content.ActualWidth;
            popup.Height = Application.Current.Host.Content.ActualHeight;
            popup.Child = control;
            control.PopupOpenEvent();
            popup.IsOpen = true;
            control.MobilizerSelected += control_MobilizerSelected;
        }

        public void Close()
        {
            if (IsOpen)
            {
                control.Close();

                if (OnClose != null)
                    OnClose(this, new EventArgs());
            }
        }

        private void control_MobilizerSelected(object sender, Mobilizers e)
        {
            control.MobilizerSelected -= control_MobilizerSelected;
            control.Close();

            if (MobilizerSelected != null)
                MobilizerSelected(this, e);

            if (OnClose != null)
                OnClose(this, new EventArgs());
        }
    }
}
