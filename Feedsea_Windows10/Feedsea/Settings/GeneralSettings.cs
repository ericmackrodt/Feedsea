using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feedsea.Common;

namespace Feedsea.Settings
{
    public class GeneralSettings : SettingsBase, IGeneralSettings
    {
        public ArticleViewTemplateEnum ArticleListTemplate
        {
            get { return (ArticleViewTemplateEnum)GetValueOrDefault<int>((int)ArticleViewTemplateEnum.Cards); }
            set { AddOrUpdateValue((int)value); }
        }
    }
}
