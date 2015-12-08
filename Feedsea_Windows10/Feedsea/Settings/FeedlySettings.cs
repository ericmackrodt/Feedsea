using Feedsea.Common.Providers.Feedly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Feedsea.Common.Helpers;

namespace Feedsea.Settings
{
    public class FeedlySettings : SettingsBase, IFeedlySettings
    {
        public string OAuthClientID { get { return "feedsea"; } }
        public string OAuthClientSecret { get { return "FE01QO4J0VS0CM1SZFCWXKY170MS"; } }
        public string OAuthRedirectUrl { get { return "http://localhost"; } }

        public bool ArticlesFromOldestToNewest
        {
            get { return GetValueOrDefault<bool>(false); }
            set { AddOrUpdateValue(value); }
        }

        public string LoggedInService
        {
            get { return GetValueOrDefault<string>(null); }
            set { AddOrUpdateValue(value); }
        }
        
        public string LoginEmail
        {
            get { return GetValueOrDefault<string>(null); }
            set { AddOrUpdateValue(value); }
        }
        
        public string OAuthRefreshToken
        {
            get { return GetValueOrDefault<string>(null); }
            set { AddOrUpdateValue(value); }
        }

        public string OAuthToken
        {
            get { return GetValueOrDefault<string>(null); }
            set { AddOrUpdateValue(value); }
        }
        
        public DateTime OAuthTokenExpiration
        {
            get { return GetValueOrDefault<long>(0).FromUnixTime(); }
            set { AddOrUpdateValue(value.ToUnixTime()); }
        }

        public string ProfilePicture
        {
            get { return GetValueOrDefault<string>(null); }
            set { AddOrUpdateValue(value); }
        }

        public bool ShowRead
        {
            get { return GetValueOrDefault<bool>(true); }
            set { AddOrUpdateValue(value); }
        }


        public bool ShowReadIfNoUnread
        {
            get { return GetValueOrDefault<bool>(false); }
            set { AddOrUpdateValue(value); }
        }

        public string UserID
        {
            get { return GetValueOrDefault<string>(null); }
            set { AddOrUpdateValue(value); }
        }

        public string UserName
        {
            get { return GetValueOrDefault<string>(null); }
            set { AddOrUpdateValue(value); }
        }

        public bool IsLoggedIn
        {
            get { return !string.IsNullOrWhiteSpace(OAuthToken) && !string.IsNullOrWhiteSpace(OAuthRefreshToken) && OAuthTokenExpiration > DateTime.Now; }
        }

    }
}
