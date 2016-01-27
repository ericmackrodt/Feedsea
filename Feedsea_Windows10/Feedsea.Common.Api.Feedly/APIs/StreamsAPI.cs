using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpClientHelpers;

namespace Feedsea.Common.Api.Feedly.APIs
{
    public class StreamsAPI : IStreamsAPI
    {
        private FeedlyWebClient client;

        internal StreamsAPI(FeedlyWebClient client)
        {
            this.client = client;
        }

        public Task<FeedStream> GetContent(string id, string continuation = null, int? count = default(int?), long? newerThan = default(long?), Ranked? ranked = default(Ranked?), bool? unreadOnly = default(bool?))
        {
            return client.Get<FeedStream>(QueryHelper.BuildRequestUrl("streams/contents", new
            {
                streamId = id,
                continuation = continuation,
                count = count,
                newerThan = newerThan,
                ranked = ranked != null ? ranked.ToString().ToLower() : null,
                unreadOnly = unreadOnly
            }.BuildQueryString()));
        }

        public Task<FeedStreamIDs> GetIDs(string id, Ranked ranked, bool unreadOnly)
        {
            return GetIDs(id, null, null, null, ranked, unreadOnly);
        }

        public Task<FeedStreamIDs> GetIDs(string id, string continuation, Ranked ranked, bool unreadOnly)
        {
            return GetIDs(id, continuation, null, null, ranked, unreadOnly);
        }

        public Task<FeedStreamIDs> GetIDs(string id, string continuation = null, int? count = default(int?), long? newerThan = default(long?), Ranked? ranked = default(Ranked?), bool? unreadOnly = default(bool?))
        {
            return client.Get<FeedStreamIDs>(QueryHelper.BuildRequestUrl("streams/ids", new
            {
                streamId = id,
                count = count,
                continuation = continuation,
                ranked = ranked != null ? ranked.ToString().ToLower() : null,
                unreadOnly = unreadOnly,
                newerThan = newerThan
            }.BuildQueryString()));
        }
    }
}
