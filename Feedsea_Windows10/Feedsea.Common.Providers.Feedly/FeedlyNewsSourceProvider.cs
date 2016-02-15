using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feedsea.Common.Providers.Data;
using Feedsea.Common.Api.Feedly;

namespace Feedsea.Common.Providers.Feedly
{
    public class FeedlyNewsSourceProvider : INewsSourceProvider
    {
        private readonly IProviderStorage storage;
        private readonly IFeedlyClient client;

        public FeedlyNewsSourceProvider(IFeedlyClient feedlyClient, IFeedlySettings providerSettings, IProviderStorage providerStorage)
        {
            client = feedlyClient;
            storage = providerStorage;
        }

        private static bool ScrambledEquals<T>(IEnumerable<T> list1, IEnumerable<T> list2, IEqualityComparer<T> comparer)
        {
            var cnt = new Dictionary<T, int>(comparer);
            foreach (T s in list1)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]++;
                }
                else {
                    cnt.Add(s, 1);
                }
            }
            foreach (T s in list2)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]--;
                }
                else {
                    return false;
                }
            }
            return cnt.Values.All(c => c == 0);
        }

        private bool IsSourceTreeEqual(IEnumerable<INewsSource> currentCollection, IEnumerable<INewsSource> sources)
        {
            var comparer = new NewsSourceEqualityComparer();
            var rootEquals = ScrambledEquals<INewsSource>(currentCollection, sources, comparer);

            if (!rootEquals)
                return false;

            foreach (var item in currentCollection)
            {
                var cat = item as CategoryData;
                if (cat == null)
                    continue;

                var toCompare = sources.FirstOrDefault(o => o.UrlID == item.UrlID) as CategoryData;

                var isEqual = ScrambledEquals<INewsSource>(cat.Subscriptions, toCompare.Subscriptions, comparer);

                if (!isEqual)
                    return false;
            }

            return true;
        }

        private void SetCounts(IEnumerable<INewsSource> sources, IEnumerable<INewsSource> currentCollection)
        {
            foreach (var root in sources)
            {
                var destRoot = currentCollection.FirstOrDefault(o => o.UrlID == root.UrlID);
                destRoot.UnreadNumber = root.UnreadNumber;

                var cat = root as CategoryData;

                if (cat == null)
                    continue;

                var destCat = destRoot as CategoryData;

                if (cat.Subscriptions == null || !cat.Subscriptions.Any()) continue;

                foreach (var child in cat.Subscriptions)
                {
                    var destChild = destCat.Subscriptions.FirstOrDefault(o => o.UrlID == child.UrlID);
                    destChild.UnreadNumber = child.UnreadNumber;
                }
            }
        }

        private IEnumerable<INewsSource> GetSourceTree(CountsResponse counts, Subscription[] subscriptions)
        {
            if (subscriptions == null || subscriptions.Length == 0)
                return new CategoryData[0];

            var sources = new List<INewsSource>();

            foreach (var sub in subscriptions)
            {
                UnreadCount count = null;
                if (counts != null && counts.UnreadCounts != null)
                    count = counts.UnreadCounts.FirstOrDefault(o => o.Id == sub.Id);
                var subscription = sub.ToSubscription(count, false);

                if (sub.Categories == null || !sub.Categories.Any())
                {
                    sources.Add(subscription);
                    continue;
                }

                foreach (var cat in sub.Categories)
                {
                    var category = sources.FirstOrDefault(o => o.UrlID == cat.Id) as CategoryData;

                    if (category == null)
                    {
                        if (counts != null && counts.UnreadCounts != null)
                            count = counts.UnreadCounts.FirstOrDefault(o => o.Id == cat.Id);
                        category = cat.ToCategory(count);
                        sources.Add(category);
                    }

                    if (category.Subscriptions == null)
                        category.Subscriptions = new List<SubscriptionData>();

                    category.Subscriptions.Add(subscription);
                }
            }

            return sources.OrderBy(o => o.GetType().Name).ThenBy(o => o.Name);
        }

        public async Task<IEnumerable<INewsSource>> LoadNewsSources()
        {
            return await storage.LoadNewsSources();
        }

        public async Task<IEnumerable<INewsSource>> DownloadNewsSources()
        {
            //continuationString = null;

            var counts = await client.Markers.GetCounts();
            var subscriptions = await client.Subscriptions.Get();

            var sources = GetSourceTree(counts, subscriptions);

            return sources;
        }

        public async Task<KeyValuePair<bool, IEnumerable<INewsSource>>> DownloadNewsSources(IEnumerable<INewsSource> currentCollection)
        {
            var sources = await DownloadNewsSources();
            var equals = IsSourceTreeEqual(currentCollection, sources);

            if (!equals)
            {
                await storage.ClearNewsSources();
                await storage.SaveSources(sources);
            }
            else
            {
                await storage.UpdateSources(sources);
                SetCounts(sources, currentCollection);
            }

            return new KeyValuePair<bool, IEnumerable<INewsSource>>(equals, sources);
        }
    }

    public class NewsSourceEqualityComparer : IEqualityComparer<INewsSource>
    {
        public bool Equals(INewsSource x, INewsSource y)
        {
            return x.UrlID == y.UrlID;
        }

        public int GetHashCode(INewsSource obj)
        {
            return obj.UrlID.GetHashCode();
        }
    }
}
