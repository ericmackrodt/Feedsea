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
        Task<ObservableCollection<ArticleData>> LoadArticles(INewsSource source = null);
        Task<IEnumerable<ArticleData>> LoadMoreArticles(INewsSource source = null);
        Task<IEnumerable<ArticleData>> LoadTileArticles(INewsSource source = null);
        Task<RefreshResult> Refresh(INewsSource source = null);
        Task<IEnumerable<CategoryData>> LoadCategories();
        Task<ObservableCollection<CategoryData>> LoadCategoriesWithSources();
        Task<SubscriptionData> GetSource(string id);
        Task<IEnumerable<SearchResultData>> SearchSources(string query);
        Task<INewsSource> AddNewSource(SearchResultData result, List<CategoryData> categories, string newCategory = null);
        Task<INewsSource> EditSource(SearchResultData result, List<CategoryData> categories, string newCategory = null);
        Task MarkArticleRead(ArticleData article);
        Task UnmarkArticleRead(ArticleData article);
        Task MarkAllArticlesRead(INewsSource source);
        Task<RefreshResult> RemoveSource(INewsSource source);
        Task<RefreshResult> RemoveCategory(CategoryData category);
        Task<ArticleData> GetArticleContent(ArticleData article);
        Task SaveArticleForLater(ArticleData article);
        Task RemoveFromSaved(ArticleData article);
        Task<bool> MarkMultipleArticlesRead(IEnumerable<ArticleData> articles);
        CategoryData GetSavedArticlesCategoryItem();
        void ClearProviderData();
    }

    public enum LoginStatus
    {
        Ok,
        Pending
    }
}
