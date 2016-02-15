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

        private Task<FeedStreamIDs> GetIds(INewsSource source)
        {
            //So, get the articles ids
            continuationString = null;

            var ident = GetStreamID(source);

            var ranked = settings.ArticlesFromOldestToNewest ? Ranked.Oldest : Ranked.Newest;
            var unreadOnly = !settings.ShowRead;

            return client.Streams.GetIDs(ident.Replace(ApiConstants.FormatKey_UserId, settings.UserID), ranked, unreadOnly);
        }

        private async Task<IEnumerable<ArticleData>> GetNonStoredArticles(string[] ids, FeedStreamIDs stream, IEnumerable<ArticleData> storedArticles)
        {
            var newIds = stream.Ids.Where(id => !storedArticles.Any(a => a.UniqueID == id));
            var contents = await client.Entries.GetMultipleContent(newIds.ToArray());

            var articles = contents.ToArticleCollection();

            await storage.SaveArticles(articles);

            return ids.Select(id =>
                storedArticles.FirstOrDefault(a => a.UniqueID == id) ??
                contents.FirstOrDefault(c => c.Id == id).ToArticle());
        }

        public Task<IEnumerable<ArticleData>> LoadArticles(INewsSource source)
        {
            return storage.LoadArticles(source);
        }

        public async Task<IEnumerable<ArticleData>> DownloadArticles(INewsSource source)
        {
            var stream = await GetIds(source);

            continuationString = stream.Continuation;

            var storedArticles = await storage.LoadArticles(stream.Ids);

            return await GetNonStoredArticles(stream.Ids, stream, storedArticles);
        }

        public async Task<IEnumerable<ArticleData>> DownloadArticles(IEnumerable<ArticleData> currentArticles, INewsSource source)
        {
            var stream = await GetIds(source);

            continuationString = stream.Continuation;

            return await GetNonStoredArticles(stream.Ids, stream, currentArticles);
        }
    }
}
