using feedsea.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Settings
{

   public interface IThirdPartySettings
   {

      LinkNavigationBrowsers LinkNavigationSetting { get; set; }

      YouTubeClients YoutubeClientSetting { get; set; }

      bool PocketShareEnabledSetting { get; set; }

      bool IsPocketEnabledSetting { get; set; }

   }

}