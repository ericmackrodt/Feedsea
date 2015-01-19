using System;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Collections.Generic;
using feedsea.Common;
using feedsea.Common.Providers;
using feedsea.BackgroundAgent.Common;
using feedsea.Common.Providers.MobilizerProvider;

namespace feedsea.Settings
{
    public class GeneralSettings : SettingsBase, IGeneralSettings    
    {
        public string OAuthClientID { get { return "feedsea"; } }
        public string OAuthClientSecret { get { return "FE01QO4J0VS0CM1SZFCWXKY170MS"; } }
        public string OAuthRedirectUrl { get { return "http://localhost"; } }

        /// <summary>
        /// Constructor that gets the application settings.
        /// </summary>
        public GeneralSettings()
            : base()
        {
        }

        const string ShowReadSettingKeyName = "ShowReadSetting";
        const bool ShowReadSettingDefault = true;
        public bool ShowReadSetting
        {
            get
            {
                return GetValueOrDefault<bool>(ShowReadSettingKeyName, ShowReadSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(ShowReadSettingKeyName, value))
                {
                    Save();
                }
            }
        }

        const string FirstLoadSettingKeyName = "FirstLoadSetting";
        const bool FirstLoadSettingDefault = true;
        public bool FirstLoadSetting
        {
            get
            {
                return GetValueOrDefault<bool>(FirstLoadSettingKeyName, FirstLoadSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(FirstLoadSettingKeyName, value))
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

        const string UserIdSettingKeyName = "UserIdSetting";
        const string UserIdSettingDefault = null;
        public string UserIdSetting
        {
            get
            {
                return GetValueOrDefault<string>(UserIdSettingKeyName, UserIdSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(UserIdSettingKeyName, value))
                {
                    Save();
                }
            }
        }

        const string MarkArticlesReadWhenOpenedSettingKeyName = "MarkArticlesReadWhenOpenedSetting";
        const bool MarkArticlesReadWhenOpenedSettingDefault = true;
        public bool MarkArticlesReadWhenOpenedSetting
        {
            get
            {
                return GetValueOrDefault<bool>(MarkArticlesReadWhenOpenedSettingKeyName, MarkArticlesReadWhenOpenedSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(MarkArticlesReadWhenOpenedSettingKeyName, value))
                {
                    Save();
                }
            }
        }

        const string ArticlesFromOldestToNewestSettingKeyName = "ArticlesFromOldestToNewestSetting";
        const bool ArticlesFromOldestToNewestSettingDefault = false;
        public bool ArticlesFromOldestToNewestSetting
        {
            get
            {
                return GetValueOrDefault<bool>(ArticlesFromOldestToNewestSettingKeyName, ArticlesFromOldestToNewestSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(ArticlesFromOldestToNewestSettingKeyName, value))
                {
                    Save();
                }
            }
        }

        const string CategoryToLoadSettingKeyName = "CategoryToLoadSetting";
        const string CategoryToLoadSettingDefault = null;
        public string CategoryToLoadSetting
        {
            get
            {
                return GetValueOrDefault<string>(CategoryToLoadSettingKeyName, CategoryToLoadSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(CategoryToLoadSettingKeyName, value))
                {
                    Save();
                }
            }
        }
        
        const string DownloadArticleIfNoContentSettingKeyName = "DownloadArticleIfNoContentSetting";
        const bool DownloadArticleIfNoContentSettingDefault = false;
        public bool DownloadArticleIfNoContentSetting
        {
            get
            {
                return GetValueOrDefault<bool>(DownloadArticleIfNoContentSettingKeyName, DownloadArticleIfNoContentSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(DownloadArticleIfNoContentSettingKeyName, value))
                {
                    Save();
                }
            }
        }
        
        const string ShowReadIfNoUnreadSettingKeyName = "ShowReadIfNoUnreadSetting";
        const bool ShowReadIfNoUnreadSettingDefault = false;
        public bool ShowReadIfNoUnreadSetting
        {
            get
            {
                return GetValueOrDefault<bool>(ShowReadIfNoUnreadSettingKeyName, ShowReadIfNoUnreadSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(ShowReadIfNoUnreadSettingKeyName, value))
                {
                    Save();
                }
            }
        }

        const string MarkReadOnFeedScrollSettingKeyName = "MarkReadOnFeedScrollSetting";
        const bool MarkReadOnFeedScrollSettingDefault = false;
        public bool MarkReadOnFeedScrollSetting
        {
            get
            {
                return GetValueOrDefault<bool>(MarkReadOnFeedScrollSettingKeyName, MarkReadOnFeedScrollSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(MarkReadOnFeedScrollSettingKeyName, value))
                {
                    Save();
                }
            }
        }
        
        const string FirstRefreshSettingKeyName = "FirstRefreshSetting";
        const bool FirstRefreshSettingDefault = true;
        public bool FirstRefreshSetting
        {
            get
            {
                return GetValueOrDefault<bool>(FirstRefreshSettingKeyName, FirstRefreshSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(FirstRefreshSettingKeyName, value))
                {
                    Save();
                }
            }
        }
        
        const string IsAdsDisabledSettingKeyName = "IsAdsDisabledSetting";
        const bool IsAdsDisabledSettingDefault = false;
        public bool IsAdsDisabledSetting
        {
            get
            {
                return GetValueOrDefault<bool>(IsAdsDisabledSettingKeyName, IsAdsDisabledSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(IsAdsDisabledSettingKeyName, value))
                {
                    Save();
                }
            }
        }

        const string AskConfirmationMarkReadSettingKeyName = "AskConfirmationMarkReadSetting";
        const bool AskConfirmationMarkReadSettingDefault = true;
        public bool AskConfirmationMarkReadSetting
        {
            get
            {
                return GetValueOrDefault<bool>(AskConfirmationMarkReadSettingKeyName, AskConfirmationMarkReadSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(AskConfirmationMarkReadSettingKeyName, value))
                {
                    Save();
                }
            }
        }


        const string LoginEmailSettingKeyName = "LoginEmailSetting";
        const string LoginEmailSettingDefault = null;
        public string LoginEmailSetting
        {
            get
            {
                return GetValueOrDefault<string>(LoginEmailSettingKeyName, LoginEmailSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(LoginEmailSettingKeyName, value))
                {
                    Save();
                }
            }
        }

        const string ProfilePictureSettingKeyName = "ProfilePictureSetting";
        const string ProfilePictureSettingDefault = null;
        public string ProfilePictureSetting
        {
            get
            {
                return GetValueOrDefault<string>(ProfilePictureSettingKeyName, ProfilePictureSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(ProfilePictureSettingKeyName, value))
                {
                    Save();
                }
            }
        }

        const string UserNameSettingKeyName = "UserNameSettingSetting";
        const string UserNameSettingDefault = null;
        public string UserNameSetting
        {
            get
            {
                return GetValueOrDefault<string>(UserNameSettingKeyName, UserNameSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(UserNameSettingKeyName, value))
                {
                    Save();
                }
            }
        }


        const string LoggedInServiceSettingKeyName = "LoggedInServiceSetting";
        const string LoggedInServiceSettingDefault = "";
        public string LoggedInServiceSetting
        {
            get
            {
                return GetValueOrDefault<string>(LoggedInServiceSettingKeyName, LoggedInServiceSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(LoggedInServiceSettingKeyName, value))
                {
                    Save();
                }
            }
        }        
    }
}