using feedsea.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Settings
{
    public class AppearanceSettings : SettingsBase, IAppearanceSettings
    {
        const string HideAppBarOnMainPageSettingKeyName = "HideAppBarOnMainPageSetting";
        const bool HideAppBarOnMainPageSettingDefault = false;
        public bool HideAppBarOnMainPageSetting
        {
            get
            {
                return GetValueOrDefault<bool>(HideAppBarOnMainPageSettingKeyName, HideAppBarOnMainPageSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(HideAppBarOnMainPageSettingKeyName, value))
                {
                    Save();
                }
            }
        }

        const string HideAppBarOnArticlePageSettingKeyName = "HideAppBarOnArticlePageSetting";
        const bool HideAppBarOnArticlePageSettingDefault = false;
        public bool HideAppBarOnArticlePageSetting
        {
            get
            {
                return GetValueOrDefault<bool>(HideAppBarOnArticlePageSettingKeyName, HideAppBarOnArticlePageSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(HideAppBarOnArticlePageSettingKeyName, value))
                {
                    Save();
                }
            }
        }

        const string ShowTrayOnTopSettingKeyName = "ShowTrayOnTopSetting";
        const bool ShowTrayOnTopSettingDefault = false;
        public bool ShowTrayOnTopSetting
        {
            get
            {
                return GetValueOrDefault<bool>(ShowTrayOnTopSettingKeyName, ShowTrayOnTopSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(ShowTrayOnTopSettingKeyName, value))
                {
                    Save();
                }
            }
        }

        const string CurrentThemeSettingKeyName = "CurrentThemeSetting";
        const AppTheme CurrentThemeSettingDefault = AppTheme.Default;
        public AppTheme CurrentThemeSetting
        {
            get
            {
                return GetValueOrDefault<AppTheme>(CurrentThemeSettingKeyName, CurrentThemeSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(CurrentThemeSettingKeyName, value))
                {
                    Save();
                }
            }
        }

        const string ArticleItemsTemplateTypeSettingKeyName = "ArticleItemsTemplateTypeSetting";
        const ArticleTemplateType ArticleItemsTemplateTypeSettingDefault = ArticleTemplateType.NormalTemplate;
        public ArticleTemplateType ArticleItemsTemplateTypeSetting
        {
            get
            {
                return GetValueOrDefault<ArticleTemplateType>(ArticleItemsTemplateTypeSettingKeyName, ArticleItemsTemplateTypeSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(ArticleItemsTemplateTypeSettingKeyName, value))
                {
                    Save();
                }
            }
        }
    }
}
