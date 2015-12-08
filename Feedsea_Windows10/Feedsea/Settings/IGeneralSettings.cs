using Feedsea.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Settings
{
    public interface IGeneralSettings
    {
        ArticleViewTemplateEnum ArticleListTemplate { get; set; }
    }
}
