using feedsea.Common.Providers.Pocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Settings
{
    public class PocketSettings : SettingsBase, IPocketSettings
    {
        public string OAuthRedirectUrl { get { return "http://localhost/auth"; } }

        public string OAuthClientID { get { return "26943-d8fce22873a217f3e32c5e6b"; } }

        public string OAuthClientSecret { get { return null; } }

        const string LoginRequestTokenSettingKeyName = "LoginRequestTokenSetting";
        const string LoginRequestTokenSettingDefault = null;
        public string LoginRequestTokenSetting
        {
            get
            {
                return GetValueOrDefault<string>(LoginRequestTokenSettingKeyName, LoginRequestTokenSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(LoginRequestTokenSettingKeyName, value))
                {
                    Save();
                }
            }
        }


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
                if (AddOrUpdateValue(IsEnabledSettingKeyName, value))
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
    }
}
