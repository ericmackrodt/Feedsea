using feedsea.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Settings
{
    public interface IAppearanceSettings
    {
        bool HideAppBarOnMainPageSetting { get; set; }
        bool HideAppBarOnArticlePageSetting { get; set; }
        bool ShowTrayOnTopSetting { get; set; }
        AppTheme CurrentThemeSetting { get; set; }
        ArticleTemplateType ArticleItemsTemplateTypeSetting { get; set; }
    }
}
