using feedsea.Common.Providers;
using feedsea.Common.Providers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Controls
{
    public interface IArticleHtmlBuilder
    {
        Task<string> BuildHtml(ArticleData article, YouTubeClients ytClient);
    }
}
