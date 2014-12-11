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
    public class FullLoadingService : IFullLoadingService
    {
        Popup popup;
        FullSizeLoadingPanel control;

        public bool IsLoading
        {
            get { return popup != null && popup.IsOpen; }
        }

        public void StartLoading()
        {
            StartLoading(null);
        }

        public void StartLoading(string message)
        {
            if (popup != null && popup.IsOpen)
                return;

            popup = new Popup();
            control = new FullSizeLoadingPanel();

            if (!string.IsNullOrWhiteSpace(message))
                control.SetMessage(message);

            popup.Child = control;

            control.Width = Application.Current.Host.Content.ActualWidth;
            control.Height = Application.Current.Host.Content.ActualHeight;
            popup.Width = Application.Current.Host.Content.ActualWidth;
            popup.Height = Application.Current.Host.Content.ActualHeight;
            popup.IsOpen = true;

            control.Open();
        }

        public void EndLoading()
        {
            control.Close();
            if (popup != null && popup.IsOpen)
                popup.IsOpen = false;
        }
    }

    public class LoadingService : ILoadingService
    {
        Popup popup;
        LoadingIndicatorControl control;

        public bool IsLoading
        {
            get { return popup != null && popup.IsOpen; }
        }

        public void StartLoading()
        {
            StartLoading(null);
        }

        public void StartLoading(string message)
        {
            if (popup != null && popup.IsOpen)
                return;

            popup = new Popup();
            control = new LoadingIndicatorControl();

            popup.Child = control;

            control.Width = Application.Current.Host.Content.ActualWidth;
            popup.Width = Application.Current.Host.Content.ActualWidth;
            popup.IsOpen = true;

            if (!string.IsNullOrWhiteSpace(message))
                control.StartLoading(message);
            else
                control.StartLoading();
        }

        public void EndLoading()
        {
            control.EndLoading(() => { popup.IsOpen = false; });
        }

        public void EndLoading(string message)
        {
            control.EndLoading(message, () => { popup.IsOpen = false; });
        }
    }
}
