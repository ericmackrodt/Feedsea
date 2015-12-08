using feedsea.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Settings
{

   public class ThirdPartySettings
      : SettingsBase, IThirdPartySettings
   {
      const string LinkNavigationSettingKeyName = "LinkNavigationSetting";
      const LinkNavigationBrowsers LinkNavigationSettingDefault = LinkNavigationBrowsers.InternetExplorer;

      public LinkNavigationBrowsers LinkNavigationSetting
      {
         get
         {
            return GetValueOrDefault<LinkNavigationBrowsers>(LinkNavigationSettingKeyName, LinkNavigationSettingDefault);
         }
         set
         {
            if ( AddOrUpdateValue(LinkNavigationSettingKeyName, value) )
            {
               Save();
            }
         }
      }

      const string YoutubeClientSettingKeyName = "YoutubeClientSetting";
      const YouTubeClients YoutubeClientSettingDefault = YouTubeClients.Default;

      public YouTubeClients YoutubeClientSetting
      {
         get
         {
            return GetValueOrDefault<YouTubeClients>(YoutubeClientSettingKeyName, YoutubeClientSettingDefault);
         }
         set
         {
            if ( AddOrUpdateValue(YoutubeClientSettingKeyName, value) )
            {
               Save();
            }
         }
      }

      const string PocketShareEnabledSettingKeyName = "PocketShareEnabledSetting";
      const bool PocketShareEnabledSettingDefault = true;

      public bool PocketShareEnabledSetting
      {
         get
         {
            return GetValueOrDefault<bool>(PocketShareEnabledSettingKeyName, PocketShareEnabledSettingDefault);
         }
         set
         {
            if ( AddOrUpdateValue(PocketShareEnabledSettingKeyName, value) )
            {
               Save();
            }
         }
      }

      const string IsPocketEnabledSettingKeyName = "IsPocketEnabledSetting";
      const bool IsPocketEnabledSettingDefault = false;

      public bool IsPocketEnabledSetting
      {
         get
         {
            return GetValueOrDefault<bool>(IsPocketEnabledSettingKeyName, IsPocketEnabledSettingDefault);
         }
         set
         {
            if ( AddOrUpdateValue(IsPocketEnabledSettingKeyName, value) )
            {
               Save();
            }
         }
      }

   }

}