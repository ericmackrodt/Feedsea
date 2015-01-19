﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using feedsea.Common.Providers.Helpers;
using System.Net;
using feedsea.Common.Providers.Feedly;
using System.Net.Http;
using feedsea.Common.Api.Feedly;
using feedsea.Common.Api;
using feedsea.Common.Providers.Data;
using MoreLinq;

namespace feedsea.Common.Providers.Feedly
{
    public class FeedlyProvider : INewsProvider
    {
        private const string SubscriptionsFileName = "subscriptions.json";
        private const string CountsFileName = "counts.json";

        private IProviderSettings settings;
        private IProviderStorage storage;
        private string continuationString;

        public string ServiceName
        {
            get { return "Feedly"; }
        }

        public IOauthLoginData LoginData
        {
            get
            {
                return new LoginData()
                {
                    LoginUrl = new FeedlyWebClient().GetLoginUrl(settings.OAuthClientID, settings.OAuthRedirectUrl, ""),
                    RedirectUrl = ApiConstants.LoginDefaultRedirectUrl
                };
            }
        }

        public FeedlyProvider(IProviderSettings providerSettings, IProviderStorage providerStorage)
        {
            settings = providerSettings;
            storage = providerStorage;
        }

        public async Task<LoginStatus> Login()
        {
            if (string.IsNullOrWhiteSpace(settings.OAuthRefreshTokenSetting) && string.IsNullOrWhiteSpace(settings.OAuthTokenSetting))
                return LoginStatus.Pending;

            if (DateTime.Now < settings.OAuthTokenLimitSetting)
                return LoginStatus.Ok;
            try
            {
                if (string.IsNullOrWhiteSpace(settings.OAuthRefreshTokenSetting))
                    return LoginStatus.Pending;

                var cli = new FeedlyWebClient(settings.OAuthTokenSetting);
                var refreshTokenRequest = new RefreshTokenRequest()
                {
                    ClientId = settings.OAuthClientID,
                    ClientSecret = settings.OAuthClientSecret,
                    RefreshToken = settings.OAuthRefreshTokenSetting
                };
                var response = await cli.RefreshToken(refreshTokenRequest);

                settings.OAuthTokenSetting = response.AccessToken;
                settings.OAuthTokenLimitSetting = DateTime.Now.AddSeconds(response.ExpiresIn);
                settings.UserIdSetting = response.Id;

                await GetUserData(cli);

                return LoginStatus.Ok;
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException is WebException && (ex.InnerException as WebException).Status == WebExceptionStatus.RequestCanceled)
                {
                    return LoginStatus.Pending;
                }
                else
                    throw new ProviderException(ExceptionReason.NoInternetConnection);
            }
        }

        public async Task<LoginStatus> Login(object loginData)
        {
            try
            {
                var code = ((string)loginData);
                var cli = new FeedlyWebClient(settings.OAuthTokenSetting);
                var tokenRequest = new AuthTokenRequest()
                {
                    ClientId = settings.OAuthClientID,
                    ClientSecret = settings.OAuthClientSecret,
                    Code = code,
                    RedirectUri = ApiConstants.LoginDefaultRedirectUrl
                };
                var response = await cli.RequestAccessToken(tokenRequest);
                settings.OAuthTokenSetting = response.AccessToken;
                settings.OAuthTokenLimitSetting = DateTime.Now.AddSeconds(response.ExpiresIn);
                settings.UserIdSetting = response.Id;
                settings.OAuthRefreshTokenSetting = response.RefreshToken;

                cli = new FeedlyWebClient(settings.OAuthTokenSetting);
                await GetUserData(cli);

                return LoginStatus.Ok;
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException is WebException && (ex.InnerException as WebException).Status == WebExceptionStatus.RequestCanceled)
                {
                    return LoginStatus.Pending;
                }
                else
                    throw new ProviderException(ExceptionReason.NoInternetConnection);
            }
        }

        public async Task<ObservableCollection<ArticleData>> LoadArticles(INewsSource source = null)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    continuationString = null;
                    var items = await DownloadArticles(source);

                    var subscriptions = storage.DeserializeFromStorage<Subscription[]>(SubscriptionsFileName);

                    if (subscriptions == null)
                        subscriptions = new Subscription[0];

                    var articles = GetArticlesWithSubscriptions(items, subscriptions);
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

        public async Task<IEnumerable<ArticleData>> LoadMoreArticles(INewsSource source = null)
        {
            if (continuationString == null)
                return null;

            return await Task.Run(async () =>
            {
                try
                {
                    var items = await DownloadArticles(source, continuationString);
                    
                    var subscriptions = storage.DeserializeFromStorage<Subscription[]>(SubscriptionsFileName);

                    if (subscriptions == null)
                        subscriptions = new Subscription[0];

                    return GetArticlesWithSubscriptions(items, subscriptions);
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

                    var cli = new FeedlyWebClient(settings.OAuthTokenSetting);

                    var stream = await cli.GetStream(ident.Replace(ApiConstants.FormatKey_UserId, settings.UserIdSetting), count: 2);

                    var subscriptions = storage.DeserializeFromStorage<Subscription[]>(SubscriptionsFileName);

                    if (subscriptions == null)
                        subscriptions = new Subscription[0];

                    var articles = GetArticlesWithSubscriptions(stream.Items, subscriptions);
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

        public async Task<RefreshResult> Refresh(INewsSource source = null)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    continuationString = null;

                    var client = new FeedlyWebClient(settings.OAuthTokenSetting);
                    var counts = await client.GetCounts();
                    var subscriptions = await client.GetSubscriptions();
                    var articles = await DownloadArticles(source);

                    var sources = GetSources(counts, subscriptions);
                    var articlesWithSubs = GetArticlesWithSubscriptions(articles, subscriptions);

                    storage.SerializeToStorage(SubscriptionsFileName, subscriptions);
                    storage.SerializeToStorage(CountsFileName, counts);

                    return new RefreshResult()
                    {
                        Sources = new ObservableCollection<CategoryData>(sources),
                        Articles = new ObservableCollection<ArticleData>(articlesWithSubs)
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
            });
        }

        public async Task<IEnumerable<CategoryData>> LoadCategories()
        {
            return await Task.Run(() =>
            {
                var subscriptions = storage.DeserializeFromStorage<Subscription[]>(SubscriptionsFileName);
                var counts = storage.DeserializeFromStorage<CountsResponse>(CountsFileName);

                if (counts == null || subscriptions == null)
                    return new List<CategoryData>();

                return GetSources(counts, subscriptions);
            });
        }

        public async Task<ObservableCollection<CategoryData>> LoadCategoriesWithSources()
        {
            return await Task.Run(() =>
            {
                var subscriptions = storage.DeserializeFromStorage<Subscription[]>(SubscriptionsFileName);
                var counts = storage.DeserializeFromStorage<CountsResponse>(CountsFileName);

                if (counts == null || subscriptions == null)
                    return new ObservableCollection<CategoryData>();

                return new ObservableCollection<CategoryData>(GetSources(counts, subscriptions));
            });
        }

        public async Task<SubscriptionData> GetSource(string id)
        {
            return await Task.Run(() =>
            {
                var subscriptions = storage.DeserializeFromStorage<Subscription[]>(SubscriptionsFileName);

                if (subscriptions == null) return null;
                
                var sub = subscriptions.FirstOrDefault(o => o.Id == id);

                if (sub == null) return null;

                return sub.ToSubscription(null, true);
            });
        }

        public async Task<IEnumerable<SearchResultData>> SearchSources(string query)
        {
            try
            {
                var client = new FeedlyWebClient(settings.OAuthTokenSetting);
                var results = await client.SearchFeeds(query);
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
                    var cli = new FeedlyWebClient(settings.OAuthTokenSetting);

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
                                .Replace(ApiConstants.FormatKey_UserId, settings.UserIdSetting)
                                .Replace(ApiConstants.FormatKey_Category, newCategory.OnlyLetterOrDigits()),
                            Label = string.Join("", newCategory.FormatCategoryLabel())
                        };
                        cats.Add(cat);
                    }

                    await cli.Subscribe(result.Id, result.Title, cats.ToArray());

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
                    var cli = new FeedlyWebClient(settings.OAuthTokenSetting);

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
                                .Replace(ApiConstants.FormatKey_UserId, settings.UserIdSetting)
                                .Replace(ApiConstants.FormatKey_Category, newCategory.OnlyLetterOrDigits()),
                            Label = string.Join("", newCategory.FormatCategoryLabel())
                        };
                        cats.Add(cat);
                    }

                    await cli.Subscribe(result.Id, result.Title, cats.ToArray());

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
                var cli = new FeedlyWebClient(settings.OAuthTokenSetting);
                if (articles.Any())
                {
                    await cli.MarkRead(new MarkerInputEntries(articles.Select(o => o.UniqueID).ToArray()));
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

        public async Task MarkAllArticlesRead(INewsSource source)
        {
            await Task.Run(async () =>
            {
                try
                {
                    var cli = new FeedlyWebClient(settings.OAuthTokenSetting);

                    var url = ApiConstants.GlobalCategory_All;

                    if (source != null)
                        url = source.UrlID;

                    var toMarkAsRead = new string[] { url.Replace(ApiConstants.FormatKey_UserId, settings.UserIdSetting) };
                    IMarkerInput input = null;

                    if (source is SubscriptionData)
                        input = new MarkerInputFeed(toMarkAsRead);
                    else
                        input = new MarkerInputCategories(toMarkAsRead);

                    await cli.MarkRead(input);

                    var counts = await cli.GetCounts();
                    storage.SerializeToStorage(CountsFileName, counts);
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
                var client = new FeedlyWebClient(settings.OAuthTokenSetting);
                var id = Uri.EscapeDataString(source.UrlID.Replace(ApiConstants.FormatKey_UserId, settings.UserIdSetting));
                await client.DeleteSubscription(id);
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
                var client = new FeedlyWebClient(settings.OAuthTokenSetting);
                var id = Uri.EscapeDataString(category.UrlID.Replace(ApiConstants.FormatKey_UserId, settings.UserIdSetting));
                await client.DeleteCategory(id);
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
                var client = new FeedlyWebClient(settings.OAuthTokenSetting);
                var content = await client.GetEntryContent(article.UniqueID);
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
                var client = new FeedlyWebClient(settings.OAuthTokenSetting);
                await client.SaveEntry(new TagEntryInput()
                {
                    EntryId = article.UniqueID,
                    UserId = settings.UserIdSetting
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
                var client = new FeedlyWebClient(settings.OAuthTokenSetting);
                await client.RemoveFromSaved(new TagEntryInput()
                {
                    EntryId = article.UniqueID,
                    UserId = settings.UserIdSetting
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
                var client = new FeedlyWebClient(settings.OAuthTokenSetting);
                await client.MarkRead(new MarkerInputEntries(new string[] { article.UniqueID }));
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
                var client = new FeedlyWebClient(settings.OAuthTokenSetting);
                await client.KeepUnread(new MarkerInputEntries(new string[] { article.UniqueID }));
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

        public void ClearProviderData()
        {
            storage.DeleteFile(SubscriptionsFileName);
            storage.DeleteFile(CountsFileName);
        }

        #region Private_Methods

        private async Task GetUserData(FeedlyWebClient cli)
        {
            var user = await cli.GetProfile();
            settings.ProfilePictureSetting = user.Picture;

            if (user.TwitterConnected)
            {
                settings.LoginEmailSetting = user.Twitter;
                settings.LoggedInServiceSetting = "Twitter";
            }
            else if (user.FacebookConnected)
            {
                settings.LoginEmailSetting = user.Email;
                settings.LoggedInServiceSetting = "Facebook";
            }
            else if (user.WindowsLiveConnected)
            {
                settings.LoginEmailSetting = user.Email;
                settings.LoggedInServiceSetting = "Microsoft";
            }
            else if (user.EvernoteConnected)
            {
                settings.LoginEmailSetting = user.Email;
                settings.LoggedInServiceSetting = "Evernote";
            }
            else
            {
                settings.LoginEmailSetting = user.Email;
                settings.LoggedInServiceSetting = "Google";
            }

            settings.UserNameSetting = user.FullName;
        }

        private string GetStreamID(INewsSource source)
        {
            var ident = ApiConstants.GlobalCategory_All;
            if (source != null)
                ident = source.UrlID;

            return ident;
        }

        private IEnumerable<ArticleData> GetArticlesWithSubscriptions(IEnumerable<Entry> items, Subscription[] subscriptions)
        {
            return items.ToArticleCollection((id) => 
            {
                if (subscriptions == null) return null;
                var sub = subscriptions.FirstOrDefault(o => o.Id == id);
                if (sub == null) return null;
                return sub.ToSubscription(null, false); 
            });
        }

        private async Task<IEnumerable<Entry>> DownloadArticles(INewsSource source, string continuation = null)
        {
            var ident = GetStreamID(source);

            var cli = new FeedlyWebClient(settings.OAuthTokenSetting);

            var ranked = settings.ArticlesFromOldestToNewestSetting ? Ranked.Oldest : Ranked.Newest;
            var unreadOnly = !settings.ShowReadSetting;

            var stream = await cli.GetStream(ident.Replace(ApiConstants.FormatKey_UserId, settings.UserIdSetting), continuation, ranked: ranked, unreadOnly: unreadOnly);
            if (!stream.Items.Any() && settings.ShowReadIfNoUnreadSetting)
                stream = await cli.GetStream(ident, continuation, unreadOnly: false, ranked: ranked);

            continuationString = stream.Continuation;

            return stream.Items;
        }

        private IEnumerable<CategoryData> GetSources(CountsResponse counts, Subscription[] subscriptions)
        {
            if (subscriptions == null || subscriptions.Length == 0)
                return new CategoryData[0];

            var uncategorized = new CategoryData()
            {
                UrlID = ApiConstants.GlobalCategory_Uncategorized,
                Name = "Uncategorized",
                Own = true
            };

            var categories = new List<CategoryData>();
            categories.Add(uncategorized);

            foreach (var sub in subscriptions)
            {
                UnreadCount count = null;
                if (counts != null && counts.UnreadCounts != null)
                    count = counts.UnreadCounts.FirstOrDefault(o => o.Id == sub.Id);
                var subscription = sub.ToSubscription(count, false);

                if (sub.Categories == null || !sub.Categories.Any())
                {
                    var cat = categories.FirstOrDefault(o => o.Own);

                    if (cat.Subscriptions == null)
                        cat.Subscriptions = new List<SubscriptionData>();

                    cat.Subscriptions.Add(subscription);
                    continue;
                }

                foreach (var cat in sub.Categories)
                {
                    var category = categories.FirstOrDefault(o => o.UrlID == cat.Id);

                    if (category == null)
                    {
                        category = cat.ToCategory();
                        categories.Add(category);
                    }

                    if (category.Subscriptions == null)
                        category.Subscriptions = new List<SubscriptionData>();

                    category.Subscriptions.Add(subscription);
                }
            }

            return categories.Where(o => o.Subscriptions != null && o.Subscriptions.Any()).OrderBy(o => o.Name).ToArray();
        }

        #endregion Private_Methods
        
        public Task<LoginStatus> Login(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
