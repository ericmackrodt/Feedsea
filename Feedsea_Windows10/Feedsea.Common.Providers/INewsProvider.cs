using Feedsea.Common.Providers.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Providers
{
    public interface INewsProvider : IProvider 
    {
        Task<IEnumerable<INewsSource>> LoadNewsSources();
        Task<IEnumerable<INewsSource>> DownloadNewsSources();
        Task<KeyValuePair<bool, IEnumerable<INewsSource>>> DownloadNewsSources(IEnumerable<INewsSource> currentCollection);
        Task<IEnumerable<ArticleData>> LoadArticles(INewsSource source = null);
        Task<IEnumerable<ArticleData>> DownloadArticles(ArticleData lastArticle);
        Task<IEnumerable<ArticleData>> DownloadArticles(INewsSource source);
        Task<IEnumerable<ArticleData>> DownloadArticles(ArticleData lastArticle, INewsSource source);


        [Obsolete("Don't know if i'm going to keep this thing")]
        Task<ObservableCollection<ArticleData>> LoadMostEngagingArticles(INewsSource source = null);

        [Obsolete("Maybe this should be part of the other download articles, or maybe this should take an ArticleResponse class with continuation string and other info")]
        Task<IEnumerable<ArticleData>> LoadMoreArticles(INewsSource source = null);

        [Obsolete("Do I even need this?")]
        Task<IEnumerable<ArticleData>> LoadTileArticles(INewsSource source = null);

        [Obsolete("This will be removed")]
        Task<RefreshResult> Refresh(INewsSource source = null);

        [Obsolete("This will probably be eliminated in favor of the load sources methods")]
        Task<IEnumerable<CategoryData>> LoadCategories();

        Task<SubscriptionData> GetSource(string id);
        Task<IEnumerable<SearchResultData>> SearchSources(string query);
        Task<INewsSource> AddNewSource(SearchResultData result, List<CategoryData> categories, string newCategory = null);
        Task<INewsSource> EditSource(SearchResultData result, List<CategoryData> categories, string newCategory = null);

        [Obsolete("Use some sort of builk action and caching not to make requests without any need")]
        Task MarkArticleRead(ArticleData article);
        [Obsolete("Use some sort of builk action and caching not to make requests without any need")]
        Task UnmarkArticleRead(ArticleData article);
        [Obsolete("Use some sort of builk action and caching not to make requests without any need")]
        Task MarkAllArticlesRead(INewsSource source);

        Task<RefreshResult> RemoveSource(INewsSource source);
        Task<RefreshResult> RemoveCategory(CategoryData category);

        [Obsolete("How this method is going to work from now on is a big question")]
        Task<ArticleData> GetArticleContent(ArticleData article);

        [Obsolete("Use some sort of builk action and caching not to make requests without any need")]
        Task SaveArticleForLater(ArticleData article);
        [Obsolete("Use some sort of builk action and caching not to make requests without any need")]
        Task RemoveFromSaved(ArticleData article);
        [Obsolete("Use some sort of builk action and caching not to make requests without any need")]
        Task<bool> MarkMultipleArticlesRead(IEnumerable<ArticleData> articles);

        [Obsolete("WUT?", true)]
        CategoryData GetSavedArticlesCategoryItem();
    }

    public enum LoginStatus
    {
        Ok,
        Pending
    }
}
