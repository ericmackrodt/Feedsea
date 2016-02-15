using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feedsea.Common.Providers.Helpers;
using System.Net;
using Feedsea.Common.Providers.Feedly;
using System.Net.Http;
using Feedsea.Common.Api.Feedly;
using Feedsea.Common.Api;
using Feedsea.Common.Providers.Data;

namespace Feedsea.Common.Providers.Feedly
{
    public partial class FeedlyProvider : INewsProvider
    {
        private string continuationString = null;
        
        private readonly IFeedlySettings settings;
        private readonly IProviderStorage storage;
        private readonly IFeedlyClient client;

        public FeedlyProvider(IFeedlyClient feedlyClient, IFeedlySettings providerSettings, IProviderStorage providerStorage)
        {
            client = feedlyClient;
            settings = providerSettings;
            storage = providerStorage;
        }

        private string GetStreamID(INewsSource source)
        {
            var ident = ApiConstants.GlobalCategory_All;
            if (source != null)
                ident = source.UrlID;

            return ident;
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

        public async Task<ObservableCollection<ArticleData>> LoadMostEngagingArticles(INewsSource source = null)
        {
            return null; // await LoadArticles(() => DownloadMixesArticles(source));
        }

        public async Task<IEnumerable<ArticleData>> LoadMoreArticles(INewsSource source = null)
        {
            if (continuationString == null)
                return null;

            return await Task.Run(async () =>
            {
                try
                {
                    var items = await DownloadArticles(source, continuationString);

                    var subscriptions = await storage.LoadSubscriptions();

                    if (subscriptions == null)
                        subscriptions = new SubscriptionData[0];

                    return items.ToArticleCollection();
                }
                catch (HttpRequestException ex)
                {
                    if (ex.InnerException != null && ex.InnerException is WebException && (ex.InnerException as WebException).Status == WebExceptionStatus.RequestCanceled)
                    {
                        return null;
                    }
                    else
                        throw new ProviderException(ExceptionReason.NoInternetConnection);
                }
            });
        }

        public async Task<IEnumerable<ArticleData>> LoadTileArticles(INewsSource source = null)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    continuationString = null;
                    var ident = GetStreamID(source);

                    var stream = await client.Streams.GetContent(ident.Replace(ApiConstants.FormatKey_UserId, settings.UserID), count: 2);

                    var subscriptions = await storage.LoadSubscriptions();

                    if (subscriptions == null)
                        subscriptions = new SubscriptionData[0];

                    var articles = stream.Items.ToArticleCollection();
                    return new ObservableCollection<ArticleData>(articles);
                }
                catch (HttpRequestException ex)
                {
                    if (ex.InnerException != null && ex.InnerException is WebException && (ex.InnerException as WebException).Status == WebExceptionStatus.RequestCanceled)
                    {
                        return null;
                    }
                    else
                        throw new ProviderException(ExceptionReason.NoInternetConnection);
                }
            });
        }

        [Obsolete("UPDATE THIS AND CHECK COUNTS TO NOT DOWNLOAD SUBSCRIPTIONS UNNECESSARELY")]
        public async Task<RefreshResult> Refresh(INewsSource source = null)
        {
            try
            {
                continuationString = null;

                var counts = await client.Markers.GetCounts();
                var subscriptions = await client.Subscriptions.Get();
                var articles = await DownloadArticles(source);

                var sources = GetSourceTree(counts, subscriptions);

                //await _storage.SaveSources(sources);

                return new RefreshResult()
                {
                    Sources = sources,
                    //Articles = articles.ToArticleCollection()
                };
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException is WebException && (ex.InnerException as WebException).Status == WebExceptionStatus.RequestCanceled)
                {
                    return new RefreshResult();
                }
                else
                    throw new ProviderException(ExceptionReason.NoInternetConnection);
            }
        }

        public async Task<IEnumerable<CategoryData>> LoadCategories()
        {
            var categories = await storage.LoadCategories();

            if (categories == null)
                return new List<CategoryData>();

            return categories.OrderBy(o => o.Name).ToList();
        }

        public async Task<SubscriptionData> GetSource(string id)
        {
            return await storage.GetSubscription(id);
        }

        public async Task<IEnumerable<SearchResultData>> SearchSources(string query)
        {
            try
            {
                var results = await client.Search.Feeds(query);
                return results.Results.Select(o => o.ToSearchResult());
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException is WebException && (ex.InnerException as WebException).Status == WebExceptionStatus.RequestCanceled)
                {
                    return null;
                }
                else
                    throw new ProviderException(ExceptionReason.NoInternetConnection);
            }
        }

        public async Task<INewsSource> AddNewSource(SearchResultData result, List<CategoryData> categories, string newCategory = null)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var cats = categories.Select(o => new FeedCategory()
                    {
                        Id = o.UrlID,
                        Label = o.Name
                    }).ToList();

                    if (!string.IsNullOrWhiteSpace(newCategory))
                    {
                        var cat = new FeedCategory()
                        {
                            Id = ApiConstants.CategoryFormat
                                .Replace(ApiConstants.FormatKey_UserId, settings.UserID)
                                .Replace(ApiConstants.FormatKey_Category, newCategory.OnlyLetterOrDigits()),
                            Label = string.Join("", newCategory.FormatCategoryLabel())
                        };
                        cats.Add(cat);
                    }

                    await client.Subscriptions.Subscribe(result.Id, result.Title, cats.ToArray());

                    return result.ToSubscription();
                }
                catch (HttpRequestException ex)
                {
                    if (ex.InnerException != null && ex.InnerException is WebException && (ex.InnerException as WebException).Status == WebExceptionStatus.RequestCanceled)
                    {
                        return null;
                    }
                    else
                        throw new ProviderException(ExceptionReason.NoInternetConnection);
                }
            });
        }

        public async Task<INewsSource> EditSource(SearchResultData result, List<CategoryData> categories, string newCategory = null)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var cats = categories.Select(o => new FeedCategory()
                    {
                        Id = o.UrlID,
                        Label = o.Name
                    }).ToList();

                    if (!string.IsNullOrWhiteSpace(newCategory))
                    {
                        var cat = new FeedCategory()
                        {
                            Id = ApiConstants.CategoryFormat
                                .Replace(ApiConstants.FormatKey_UserId, settings.UserID)
                                .Replace(ApiConstants.FormatKey_Category, newCategory.OnlyLetterOrDigits()),
                            Label = string.Join("", newCategory.FormatCategoryLabel())
                        };
                        cats.Add(cat);
                    }

                    await client.Subscriptions.Subscribe(result.Id, result.Title, cats.ToArray());

                    return result.ToSubscription();
                }
                catch (HttpRequestException ex)
                {
                    if (ex.InnerException != null && ex.InnerException is WebException && (ex.InnerException as WebException).Status == WebExceptionStatus.RequestCanceled)
                    {
                        return null;
                    }
                    else
                        throw new ProviderException(ExceptionReason.NoInternetConnection);
                }
            });
        }

        public async Task<bool> MarkMultipleArticlesRead(IEnumerable<ArticleData> articles)
        {
            try
            {
                if (articles.Any())
                {
                    await client.Markers.MarkRead(new MarkerInputEntries(articles.Select(o => o.UniqueID).ToArray()));
                    foreach (var art in articles)
                        art.IsRead = true;

                    return true;
                }

                return false;
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException is WebException && (ex.InnerException as WebException).Status == WebExceptionStatus.RequestCanceled)
                {
                    return false;
                }
                else
                    throw new ProviderException(ExceptionReason.NoInternetConnection);
            }
        }

        [Obsolete("UPDATE THIS, CHECK COUNTS")]
        public async Task MarkAllArticlesRead(INewsSource source)
        {
            await Task.Run(async () =>
            {
                try
                {
                    var url = ApiConstants.GlobalCategory_All;

                    if (source != null)
                        url = source.UrlID;

                    var toMarkAsRead = new string[] { url.Replace(ApiConstants.FormatKey_UserId, settings.UserID) };
                    IMarkerInput input = null;

                    if (source is SubscriptionData)
                        input = new MarkerInputFeed(toMarkAsRead);
                    else
                        input = new MarkerInputCategories(toMarkAsRead);

                    await client.Markers.MarkRead(input);
                }

                catch (HttpRequestException ex)
                {
                    if (ex.InnerException != null && ex.InnerException is WebException && (ex.InnerException as WebException).Status == WebExceptionStatus.RequestCanceled)
                    {
                        return;
                    }
                    else
                        throw new ProviderException(ExceptionReason.NoInternetConnection);
                }
            });
        }

        public async Task<RefreshResult> RemoveSource(INewsSource source)
        {
            try
            {
                var id = Uri.EscapeDataString(source.UrlID.Replace(ApiConstants.FormatKey_UserId, settings.UserID));
                await client.Subscriptions.Delete(id);
                return await Refresh();
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException is WebException && (ex.InnerException as WebException).Status == WebExceptionStatus.RequestCanceled)
                {
                    return new RefreshResult();
                }
                else
                    throw new ProviderException(ExceptionReason.NoInternetConnection);
            }
        }

        public async Task<RefreshResult> RemoveCategory(CategoryData category)
        {
            try
            {
                var id = Uri.EscapeDataString(category.UrlID.Replace(ApiConstants.FormatKey_UserId, settings.UserID));
                await client.Categories.Delete(id);
                return await Refresh();
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException is WebException && (ex.InnerException as WebException).Status == WebExceptionStatus.RequestCanceled)
                {
                    return new RefreshResult();
                }
                else
                    throw new ProviderException(ExceptionReason.NoInternetConnection);
            }
        }

        public async Task<ArticleData> GetArticleContent(ArticleData article)
        {
            try
            {
                var content = await client.Entries.GetContent(article.UniqueID);
                return content.ToArticle();
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException is WebException && (ex.InnerException as WebException).Status == WebExceptionStatus.RequestCanceled)
                {
                    return null;
                }
                else
                    throw new ProviderException(ExceptionReason.NoInternetConnection);
            }
        }

        public async Task SaveArticleForLater(ArticleData article)
        {
            try
            {
                await client.Markers.SaveEntry(new TagEntryInput()
                {
                    EntryId = article.UniqueID,
                    UserId = settings.UserID
                });
                article.IsFavorite = true;
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException is WebException && (ex.InnerException as WebException).Status == WebExceptionStatus.RequestCanceled)
                {
                    return;
                }
                else
                    throw new ProviderException(ExceptionReason.NoInternetConnection);
            }
        }

        public async Task RemoveFromSaved(ArticleData article)
        {
            try
            {
                await client.Markers.UnsaveEntry(new TagEntryInput()
                {
                    EntryId = article.UniqueID,
                    UserId = settings.UserID
                });
                article.IsFavorite = false;
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException is WebException && (ex.InnerException as WebException).Status == WebExceptionStatus.RequestCanceled)
                {
                    return;
                }
                else
                    throw new ProviderException(ExceptionReason.NoInternetConnection);
            }
        }

        public async Task MarkArticleRead(ArticleData article)
        {
            try
            {
                await client.Markers.MarkRead(new MarkerInputEntries(new string[] { article.UniqueID }));
                article.IsRead = true;
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException is WebException && (ex.InnerException as WebException).Status == WebExceptionStatus.RequestCanceled)
                {
                    return;
                }
                else
                    throw new ProviderException(ExceptionReason.NoInternetConnection);
            }
        }

        public async Task UnmarkArticleRead(ArticleData article)
        {
            try
            {
                await client.Markers.KeepUnread(new MarkerInputEntries(new string[] { article.UniqueID }));
                article.IsRead = false;
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException is WebException && (ex.InnerException as WebException).Status == WebExceptionStatus.RequestCanceled)
                {
                    return;
                }
                else
                    throw new ProviderException(ExceptionReason.NoInternetConnection);
            }
        }

        public CategoryData GetSavedArticlesCategoryItem()
        {
            return new CategoryData()
            {
                Name = "SavedForLater",
                URL = ApiConstants.GlobalTag_Saved,
                UrlID = ApiConstants.GlobalTag_Saved,
                Own = true
            };
        }

        public async Task<string> GetMobilizedWebPage(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, string.Format("http://www.readability.com/m?url={0}", url));
                    var result = await client.SendAsync(request);
                    result.EnsureSuccessStatusCode();
                    var data = await result.Content.ReadAsStringAsync();
                    return data;
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException is WebException && (ex.InnerException as WebException).Status == WebExceptionStatus.RequestCanceled)
                {
                    return null;
                }
                else
                    throw new ProviderException(ExceptionReason.NoInternetConnection);
            }
        }

        #region Private_Methods

        

        private async Task<IEnumerable<Entry>> DownloadStreamArticles(INewsSource source, string continuation = null)
        {
            return await DownloadArticles(source, continuation, async (ident, ranked, unreadOnly) =>
            {
                var stream = await client.Streams.GetContent(ident.Replace(ApiConstants.FormatKey_UserId, settings.UserID), continuation, ranked: ranked, unreadOnly: unreadOnly);
                if (!stream.Items.Any() && settings.ShowReadIfNoUnread)
                    stream = await client.Streams.GetContent(ident, continuation, unreadOnly: false, ranked: ranked);
                return stream;
            });
        }

        private async Task<IEnumerable<Entry>> DownloadMixesArticles(INewsSource source, string continuation = null)
        {
            return await DownloadArticles(source, continuation, async (ident, ranked, unreadOnly) =>
            {
                var stream = await client.Mixes.Get(ident.Replace(ApiConstants.FormatKey_UserId, settings.UserID), continuation, unreadOnly: unreadOnly, count: 20);
                if (!stream.Items.Any() && settings.ShowReadIfNoUnread)
                    stream = await client.Mixes.Get(ident, continuation, unreadOnly: false, count: 20);
                return stream;
            });
        }

        private async Task<IEnumerable<Entry>> DownloadArticles(INewsSource source, string continuation, Func<string, Ranked,  bool, Task<FeedStream>> streamDownload)
        {
            var ident = GetStreamID(source);

            var ranked = settings.ArticlesFromOldestToNewest ? Ranked.Oldest : Ranked.Newest;
            var unreadOnly = !settings.ShowRead;

            var stream = await streamDownload(ident, ranked, unreadOnly);

            continuationString = stream.Continuation;

            return stream.Items;
        }

        private async Task<IEnumerable<Entry>> DownloadArticles(INewsSource source, string continuation = null)
        {
            var ident = GetStreamID(source);

            var ranked = settings.ArticlesFromOldestToNewest ? Ranked.Oldest : Ranked.Newest;
            var unreadOnly = !settings.ShowRead;

            var stream = await client.Streams.GetContent(ident.Replace(ApiConstants.FormatKey_UserId, settings.UserID), continuation, ranked: ranked, unreadOnly: unreadOnly);
            if (!stream.Items.Any() && settings.ShowReadIfNoUnread)
                stream = await client.Streams.GetContent(ident, continuation, unreadOnly: false, ranked: ranked);

            continuationString = stream.Continuation;

            return stream.Items;
        }

        private async Task<IEnumerable<ArticleData>> LoadArticles(Func<Task<IEnumerable<Entry>>> downloadArticlesFunction)
        {
            try
            {
                continuationString = null;
                var items = await downloadArticlesFunction();

                var subscriptions = await storage.LoadSubscriptions();

                if (subscriptions == null)
                    subscriptions = new SubscriptionData[0];

                var articles = items.ToArticleCollection();
                return articles;
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException is WebException && (ex.InnerException as WebException).Status == WebExceptionStatus.RequestCanceled)
                {
                    return null;
                }
                else
                    throw new ProviderException(ExceptionReason.NoInternetConnection);
            }
        }

        #endregion Private_Methods
    }
}
