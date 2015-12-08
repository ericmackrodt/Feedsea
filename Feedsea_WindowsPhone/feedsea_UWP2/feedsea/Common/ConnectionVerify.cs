using Cimbalino.Phone.Toolkit.Services;
using Coding4Fun.Toolkit.Controls;
using feedsea.BackgroundAgent.Common;
using feedsea.Common.Api;
using feedsea.Common.MVVM;
using feedsea.Common.Providers;
using feedsea.Resources;
using System.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace feedsea.Common
{

   public class ConnectionVerify
      : Windows.UI.Xaml.FrameworkElement, IConnectionVerify
   {
      IMessageBoxService messageBox;

      public ConnectionVerify(IMessageBoxService messageBoxService)
      {
         messageBox = messageBoxService;
      }

      public bool HasInternetConnection()
      {
         var hasConnection = NetworkInterface.GetIsNetworkAvailable();
         if ( !hasConnection )
            ShowNoConnectionMessage();
         return hasConnection;
      }

      public void ShowNoConnectionMessage()
      {
         try
         {
            messageBox.Show(AppResources.MSG_NoConnection);
         }
         catch
         {
         }
      }

      public bool VerifyConnectionException(Exception ex)
      {
         Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
               if ( ex != null && ex is ProviderException )
               {
                  var pex = ex as ProviderException;
                  if ( pex.Reason == ExceptionReason.NoInternetConnection )
                  {
                     ShowNoConnectionMessage();
                  //return false;
                  }
               //return true;
               }
               else if ( ex != null && ex is TileCreationException )
               {
                  var pex1 = ex as TileCreationException;
                  if ( pex1.Type == ExceptionType.NoNetworkAccess )
                  {
                     ShowNoConnectionMessage();
                  //return false;
                  }
               }
               else if ( ex != null && ex is TaskCanceledException )
               {
                  return ;
               }
               else if ( ex != null && ex is HtmlResponseException )
               {
                  messageBox.Show(AppResources.Msg_HtmlResponse_Message, AppResources.Msg_HtmlResponse_Title);
                  return ;
               }
               else if ( ex != null )
                  ExceptionDispatchInfo.Capture(ex).Throw();
            });
         return false;
      }

   }

}