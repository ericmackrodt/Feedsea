using Feedsea.Common.Providers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Providers
{
    public interface IArticleProvider
    {
        Task<IEnumerable<ArticleData>> LoadArticles(INewsSource source = null);
        Task<IEnumerable<ArticleData>> DownloadArticles(INewsSource source);
        Task<IEnumerable<ArticleData>> DownloadArticles(IEnumerable<ArticleData> currentArticles, INewsSource source);
    }
}
