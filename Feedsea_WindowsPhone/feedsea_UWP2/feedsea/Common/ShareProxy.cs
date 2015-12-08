using feedsea.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace feedsea.Common
{

   public class ShareProxy
      : IShareProxy
   {

      public void SendToClipboard(string value)
      {
         WindowsPhoneUWP.UpgradeHelpers.Clipboard.SetText(value);
      }

      public void SendToSocialNetworks(string title, string url)
      {
         WindowsPhoneUWP.UpgradeHelpers.ShareLinkTaskHelper shareLinkTask = new WindowsPhoneUWP.UpgradeHelpers.ShareLinkTaskHelper();
         shareLinkTask.LinkUri = new Uri(url, UriKind.Absolute);
         var msg = title.Length > 140 ? title.Substring(0, 125) : title;
         shareLinkTask.Message = string.Format(AppResources.Share_Link_Message, msg);
         shareLinkTask.Title = title;
         shareLinkTask.Show();
      }

      public async void SendToEmail(string title, string url)
      {
         Windows.ApplicationModel.Email.EmailMessage shareEmailTask = new Windows.ApplicationModel.Email.EmailMessage()
            {
               To =
               {
                  new Windows.ApplicationModel.Email.EmailRecipient()
               }
            };
         shareEmailTask.Subject = title;
         shareEmailTask.Body = string.Format(AppResources.Share_Email_Body, title, url);
         await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(shareEmailTask);
      }

      public void SendToTextMessage(string title, string url)
      {
         Windows.ApplicationModel.Chat.ChatMessage shareSmsTask = new Windows.ApplicationModel.Chat.ChatMessage();
         shareSmsTask.Body = url;
         Windows.ApplicationModel.Chat.ChatMessageManager.ShowComposeSmsMessageAsync(shareSmsTask);
      }

      public void SendToNFC(string url)
      {
         throw new NotImplementedException();
      }

   }

}