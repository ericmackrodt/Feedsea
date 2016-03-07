using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feedsea.Common.Providers.Data;
using Feedsea.Common.Api.Feedly;

namespace Feedsea.Common.Providers.Feedly
{
    public class FeedlyArticleProvider : IArticleProvider
    {
        private string continuationString;

        private readonly IFeedlySettings settings;
        private readonly IProviderStorage storage;
        private readonly IFeedlyClient client;

        public FeedlyArticleProvider(IFeedlyClient feedlyClient, IFeedlySettings providerSettings, IProviderStorage providerStorage)
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

        private Task<FeedStreamIDs> GetIds(INewsSource source, string continuation = null)
        {
            //So, get the articles ids
            continuationString = null;

            var ident = GetStreamID(source);

            var ranked = settings.ArticlesFromOldestToNewest ? Ranked.Oldest : Ranked.Newest;
            var unreadOnly = !settings.ShowRead;

            return client.Streams.GetIDs(ident.Replace(ApiConstants.FormatKey_UserId, settings.UserID), continuation, ranked, unreadOnly);
        }

        private async Task<IEnumerable<ArticleData>> GetNonStoredArticles(FeedStreamIDs stream, IEnumerable<ArticleData> storedArticles)
        {
            var newIds = stream.Ids.Where(id => !storedArticles.Any(a => a.UniqueID == id));
            var contents = await client.Entries.GetMultipleContent(newIds.ToArray());

            var articles = contents.ToArticleCollection();

            await storage.SaveArticles(articles);

            return stream.Ids.Select(id =>
                storedArticles.FirstOrDefault(a => a.UniqueID == id) ??
                contents.FirstOrDefault(c => c.Id == id).ToArticle()).OrderByDescending(o => o.Date);
        }

        public Task<IEnumerable<ArticleData>> LoadArticles(INewsSource source)
        {
            return storage.LoadArticles(source);
        }

        public async Task<ContinuedArticles> DownloadArticles(INewsSource source)
        {
            var stream = await GetIds(source);

            var storedArticles = await storage.LoadArticles(stream.Ids);

            var nonStored = await GetNonStoredArticles(stream, storedArticles);
            return new ContinuedArticles(nonStored, stream.Continuation);
        }

        public async Task<ContinuedArticles> DownloadArticles(IEnumerable<ArticleData> currentArticles, INewsSource source)
        {
            var stream = await GetIds(source);
            var nonStored = await GetNonStoredArticles(stream, currentArticles);
            return new ContinuedArticles(nonStored, stream.Continuation);
        }

        public Task<IEnumerable<ArticleData>> LoadMoreArticles(ArticleData lastArticle, INewsSource source = null)
        {
            throw new NotImplementedException();
        }

        public Task<ContinuedArticles> DownloadMoreArticles(string continuation)
        {
            throw new NotImplementedException();
        }

        public Task<ContinuedArticles> DownloadMoreArticles(string continuation, ArticleData lastArticle)
        {
            throw new NotImplementedException();
        }

        public Task<ContinuedArticles> DownloadMoreArticles(string continuation, IEnumerable<ArticleData> currentArticles)
        {
            throw new NotImplementedException();
        }

        public Task<ContinuedArticles> DownloadMoreArticles(string continuation, INewsSource source)
        {
            throw new NotImplementedException();
        }

        public Task<ContinuedArticles> DownloadMoreArticles(string continuation, IEnumerable<ArticleData> currentArticles, INewsSource source)
        {
            var lastArticle = currentArticles.LastOrDefault();
            return DownloadMoreArticles(continuation, lastArticle, source);
        }

        public async Task<ContinuedArticles> DownloadMoreArticles(string continuation, ArticleData lastArticle, INewsSource source)
        {
            var savedArticles = await storage.LoadMoreArticles(source, lastArticle);

            var stream = await GetIds(source, continuation);

            continuationString = stream.Continuation;

            var nonStored = await GetNonStoredArticles(stream, savedArticles);
            return new ContinuedArticles(nonStored, stream.Continuation);
        }

        public async Task MarkAllArticlesRead(INewsSource source = null)
        {
            var i = await storage.MarkAllRead(source);

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

        public async Task MarkArticleRead(ArticleData article)
        {
            await client.Markers.MarkRead(new MarkerInputEntries(new string[] { article.UniqueID }));
            article.IsRead = true;
            await storage.UpdateArticle(article);
        }

        public async Task UnmarkArticleRead(ArticleData article)
        {
            await client.Markers.KeepUnread(new MarkerInputEntries(new string[] { article.UniqueID }));
            article.IsRead = false;
            await storage.UpdateArticle(article);
        }
    }
}
