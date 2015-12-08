using feedsea.Common.Providers.Instapaper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Settings
{

   public class InstapaperSettings
      : SettingsBase, IInstapaperSettings
   {
      const string IsEnabledSettingKeyName = "IsEnabledSetting";
      const bool IsEnabledSettingDefault = false;

      public bool IsEnabledSetting
      {
         get
         {
            return GetValueOrDefault<bool>(IsEnabledSettingKeyName, IsEnabledSettingDefault);
         }
         set
         {
            if ( AddOrUpdateValue(IsEnabledSettingKeyName, value) )
            {
               Save();
            }
         }
      }

      const string OAuthTokenSettingKeyName = "OAuthTokenSetting";
      const string OAuthTokenSettingDefault = null;

      public string OAuthTokenSetting
      {
         get
         {
            return GetValueOrDefault<string>(OAuthTokenSettingKeyName, OAuthTokenSettingDefault);
         }
         set
         {
            if ( AddOrUpdateValue(OAuthTokenSettingKeyName, value) )
            {
               Save();
            }
         }
      }

      public string OAuthClientID
      {
         get
         {
            throw new NotImplementedException();
         }
      }

      public string OAuthClientSecret
      {
         get
         {
            throw new NotImplementedException();
         }
      }

      public string OAuthRedirectUrl
      {
         get
         {
            throw new NotImplementedException();
         }
      }

      public string OAuthRefreshTokenSetting
      {
         get
         {
            throw new NotImplementedException();
         }
         set
         {
            throw new NotImplementedException();
         }
      }

      public DateTime OAuthTokenLimitSetting
      {
         get
         {
            throw new NotImplementedException();
         }
         set
         {
            throw new NotImplementedException();
         }
      }

   }

}