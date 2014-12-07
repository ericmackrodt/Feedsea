using Cimbalino.Phone.Toolkit.Services;
using feedsea.Common.Providers;
using feedsea.Resources;
using feedsea.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace feedsea.Common.Controls
{
    public class XAuthLogin : IXAuthLogin
    {
        GenericLoginScreen loginControl;
        Popup popup;
        FullSizeLoadingPanel loadingControl;
        IProvider provider;
        IMessageBoxService messageBox;

        public bool IsOpen
        {
            get { return popup != null && popup.IsOpen; }
        }

        public XAuthLogin(IMessageBoxService messageBoxService)
        {
            messageBox = messageBoxService;
        }

        public async Task<bool> Login(IProvider provider)
        {
            this.provider = provider;
            BuildLoginScreen();
            var result = await ExecuteLogin();
            popup.IsOpen = false;
            return result;
        }

        public void Cancel()
        {
            loginControl.Cancel();
        }

        private Task<bool> ExecuteLogin()
        {
            var tcs = new TaskCompletionSource<bool>();

            loginControl.OnLoginClick += async (username, password) =>
            {
                if (tcs.Task.IsCompleted) return;

                if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
                {
                    loadingControl.Open();
                    var status = await provider.Login(username, password);
                    loadingControl.Close();
                    if (status == LoginStatus.Ok)
                        tcs.SetResult(true);
                    else
                        messageBox.Show(AppResources.Msg_WrongUsernameOrPassword);
                }
            };

            loginControl.OnCanceled += (s, e) =>
            {
                if (tcs.Task.IsCompleted) return;

                tcs.SetResult(false);
            };

            return tcs.Task;
        }

        private void BuildLoginScreen()
        {
            popup = new Popup();
            loginControl = new GenericLoginScreen(provider.ServiceName);
            var grid = new Grid();
            loadingControl = new FullSizeLoadingPanel();
            loadingControl.Visibility = Visibility.Collapsed;
            popup.Child = grid;
            grid.Children.Add(loginControl);
            grid.Children.Add(loadingControl);

            grid.Width = Application.Current.Host.Content.ActualWidth;
            grid.Height = Application.Current.Host.Content.ActualHeight;
            popup.IsOpen = true;
        }
    }
}
