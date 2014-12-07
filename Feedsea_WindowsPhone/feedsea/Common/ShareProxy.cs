using feedsea.Resources;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace feedsea.Common
{
    public class ShareProxy : IShareProxy
    {
        public void SendToClipboard(string value)
        {
            Clipboard.SetText(value);
        }

        public void SendToSocialNetworks(string title, string url)
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask();
            shareLinkTask.LinkUri = new Uri(url, UriKind.Absolute);
            var msg = title.Length > 140 ? title.Substring(0, 125) : title;
            shareLinkTask.Message = string.Format(AppResources.Share_Link_Message, msg);
            shareLinkTask.Title = title;
            shareLinkTask.Show();
        }

        public void SendToEmail(string title, string url)
        {
            EmailComposeTask shareEmailTask = new EmailComposeTask();
            shareEmailTask.Subject = title;
            shareEmailTask.Body = string.Format(AppResources.Share_Email_Body, title, url);
            shareEmailTask.Show();
        }

        public void SendToTextMessage(string title, string url)
        {
            SmsComposeTask shareSmsTask = new SmsComposeTask();
            shareSmsTask.Body = url;
            shareSmsTask.Show();
        }

        public void SendToNFC(string url)
        {
            throw new NotImplementedException();
        }
    }
}
