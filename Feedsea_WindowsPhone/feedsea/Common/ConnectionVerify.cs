using Cimbalino.Phone.Toolkit.Services;
using Coding4Fun.Toolkit.Controls;
using feedsea.BackgroundAgent.Common;
using feedsea.Common.Api;
using feedsea.Common.MVVM;
using feedsea.Common.Providers;
using feedsea.Resources;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace feedsea.Common
{
    public class ConnectionVerify : FrameworkElement, IConnectionVerify
    {
        IMessageBoxService messageBox;

        public ConnectionVerify(IMessageBoxService messageBoxService)
        {
            messageBox = messageBoxService;
        }

        public bool HasInternetConnection()
        {
            var hasConnection = NetworkInterface.GetIsNetworkAvailable();
            if (!hasConnection)
                ShowNoConnectionMessage();
            return hasConnection;
        }

        public void ShowNoConnectionMessage()
        {
            try
            {
                messageBox.Show(AppResources.MSG_NoConnection);
            }
            catch { }
        }

        public bool VerifyConnectionException(Exception ex)
        {
            Dispatcher.BeginInvoke(() =>
            {
                if (ex != null && ex is ProviderException)
                {
                    var pex = ex as ProviderException;

                    if (pex.Reason == ExceptionReason.NoInternetConnection)
                    {
                        ShowNoConnectionMessage();
                        //return false;
                    }

                    //return true;
                }
                else if (ex != null && ex is TileCreationException)
                {
                    var pex = ex as TileCreationException;

                    if (pex.Type == ExceptionType.NoNetworkAccess)
                    {
                        ShowNoConnectionMessage();
                        //return false;
                    }
                }
                else if (ex != null && ex is TaskCanceledException)
                {
                    return;
                }
                else if (ex != null && ex is HtmlResponseException)
                {
                    messageBox.Show(AppResources.Msg_HtmlResponse_Message, AppResources.Msg_HtmlResponse_Title);
                    return;
                }
                else if (ex != null)
                    ExceptionDispatchInfo.Capture(ex).Throw();
            });

            return false;
        }
    }
}
