using feedsea.Common.Providers.OneNote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Settings
{
    public class OneNoteSettings : SettingsBase, IOneNoteSettings
    {
        public string OAuthClientID { get { return "0000000044119117"; } }
        public string OAuthClientSecret { get { return "kfsmay4gphq6uRyTZ1LYVFuwBN3ZMsnn"; } }
        public string OAuthRedirectUrl { get { return "http://feedsea.news/"; } }

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
                if (AddOrUpdateValue(OAuthTokenSettingKeyName, value))
                {
                    Save();
                }
            }
        }
        
        const string OAuthRefreshTokenSettingKeyName = "OAuthRefreshTokenSetting";
        const string OAuthRefreshTokenSettingDefault = null;
        public string OAuthRefreshTokenSetting
        {
            get
            {
                return GetValueOrDefault<string>(OAuthRefreshTokenSettingKeyName, OAuthRefreshTokenSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(OAuthRefreshTokenSettingKeyName, value))
                {
                    Save();
                }
            }
        }
        
        const string OAuthTokenLimitSettingKeyName = "OAuthTokenLimitSetting";
        DateTime OAuthTokenLimitSettingDefault = DateTime.MinValue;
        public DateTime OAuthTokenLimitSetting
        {
            get
            {
                return GetValueOrDefault<DateTime>(OAuthTokenLimitSettingKeyName, OAuthTokenLimitSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(OAuthTokenLimitSettingKeyName, value))
                {
                    Save();
                }
            }
        }
        
        const string IsLoggedInSettingKeyName = "IsLoggedInSetting";
        const bool IsLoggedInSettingDefault = false;
        public bool IsLoggedInSetting
        {
            get
            {
                return GetValueOrDefault<bool>(IsLoggedInSettingKeyName, IsLoggedInSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(IsLoggedInSettingKeyName, value))
                {
                    Save();
                }
            }
        }
    }
}
