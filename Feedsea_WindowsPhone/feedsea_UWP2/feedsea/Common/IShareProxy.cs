using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common
{

   public interface IShareProxy
   {

      void SendToClipboard(string value);

      void SendToSocialNetworks(string title, string url);

      void SendToEmail(string title, string url);

      void SendToTextMessage(string title, string url);

      void SendToNFC(string url);

   }

}