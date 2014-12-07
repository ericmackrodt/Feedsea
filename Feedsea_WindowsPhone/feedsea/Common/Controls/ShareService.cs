using feedsea.Common.Providers;
using feedsea.Common.Providers.Data;
using feedsea.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace feedsea.Common.Controls
{
    public class ShareService : IShareService
    {
        public event EventHandler OnShareFinished;

        private Popup sharePopup;
        private SharingControl shareControl;

        public bool IsShareOpen
        {
            get { return sharePopup != null && sharePopup.IsOpen; }
        }

        public ShareService()
        {
            var ver = Environment.OSVersion.Version;
        }

        public void Share(ArticleData article)
        {
            if (article == null || (sharePopup != null && sharePopup.IsOpen))
                return;

            sharePopup = new Popup();
            shareControl = new SharingControl();
            shareControl.Width = Application.Current.Host.Content.ActualWidth;
            shareControl.Height = Application.Current.Host.Content.ActualHeight;
            sharePopup.Width = Application.Current.Host.Content.ActualWidth;
            sharePopup.Height = Application.Current.Host.Content.ActualHeight;
            sharePopup.Child = shareControl;
            shareControl.PopupOpenEvent();  
            sharePopup.IsOpen = true;
            shareControl.Load(article);
            shareControl.OnClose += ShareControl_OnClose;
        }

        private void ShareControl_OnClose(object sender, EventArgs e)
        {
            shareControl.OnClose -= ShareControl_OnClose;
            if (OnShareFinished != null)
                OnShareFinished(this, new EventArgs());
        }

        public void CancelShare()
        {
            if (IsShareOpen)
            {
                sharePopup.IsOpen = false;
                shareControl.OnClose -= ShareControl_OnClose;

                if (OnShareFinished != null)
                    OnShareFinished(this, new EventArgs());
            }
        }

        #if DEBUG
        ~ShareService()
        {
            System.Diagnostics.Debug.WriteLine("ShareService was killed!");
        }
        #endif
    }
}
