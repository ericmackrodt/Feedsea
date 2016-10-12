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
        Task<ContinuedArticles> DownloadArticles(INewsSource source);
        Task<ContinuedArticles> DownloadMoreArticles(string continuation, INewsSource source);
        Task MarkAllArticlesRead(INewsSource source = null);
        Task MarkArticleRead(ArticleData article);
        Task UnmarkArticleRead(ArticleData article);
        Task FavoriteArticle(ArticleData article);
        Task UnfavoriteArticle(ArticleData article);
    }
}
