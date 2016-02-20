using Feedsea.Common.Providers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Providers
{
    public interface IProviderStorage
    {
        Task SaveSources(IEnumerable<INewsSource> sources);
        Task UpdateSources(IEnumerable<INewsSource> sources);
        Task SaveSubscriptions(IEnumerable<SubscriptionData> subscriptions);
        Task SaveCategories(IEnumerable<CategoryData> categories);
        Task SaveArticles(IEnumerable<ArticleData> articles);
        Task<IEnumerable<ArticleData>> LoadArticles(INewsSource source);
        Task<IEnumerable<ArticleData>> LoadArticles(string[] articleIds);
        Task<IEnumerable<ArticleData>> LoadMoreArticles(INewsSource source, ArticleData previousArticle);

        Task<IEnumerable<INewsSource>> LoadNewsSources();
        Task<IEnumerable<SubscriptionData>> LoadSubscriptions();
        Task<IEnumerable<CategoryData>> LoadCategories();
        Task ClearNewsSources();
        Task<SubscriptionData> GetSubscription(string id);
        Task UpdateSubscription(SubscriptionData subscription);
        Task Initialize();
    }
}
