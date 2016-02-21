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
        Task<IEnumerable<ArticleData>> LoadMoreArticles(ArticleData lastArticle, INewsSource source = null);
        Task<ContinuedArticles> DownloadArticles(INewsSource source);
        Task<ContinuedArticles> DownloadArticles(IEnumerable<ArticleData> currentArticles, INewsSource source);
        Task<ContinuedArticles> DownloadMoreArticles(string continuation);
        Task<ContinuedArticles> DownloadMoreArticles(string continuation, ArticleData lastArticle);
        Task<ContinuedArticles> DownloadMoreArticles(string continuation, IEnumerable<ArticleData> currentArticles);
        Task<ContinuedArticles> DownloadMoreArticles(string continuation, INewsSource source);
        Task<ContinuedArticles> DownloadMoreArticles(string continuation, ArticleData lastArticle, INewsSource source);
        Task<ContinuedArticles> DownloadMoreArticles(string continuation, IEnumerable<ArticleData> currentArticles, INewsSource source);
        Task MarkAllArticlesRead(INewsSource source = null);
    }
}
