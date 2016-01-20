using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpClientHelpers;
using System.Net;

namespace Feedsea.Common.Api.Feedly.APIs
{
    public class MarkersAPI : IMarkersAPI
    {
        private FeedlyWebClient client;

        internal MarkersAPI(FeedlyWebClient client)
        {
            this.client = client;
        }

        public Task<CountsResponse> GetCounts(bool? autoRefresh = default(bool?), long? newerThan = default(long?))
        {
            return client.Get<CountsResponse>(QueryHelper.BuildRequestUrl("markers/counts", new
            {
                autoRefresh = autoRefresh,
                newerThan = newerThan
            }.BuildQueryString()));
        }

        public Task<ReadOperations> GetLastestReadOperations()
        {
            return client.Get<ReadOperations>("markers/reads");
        }

        public Task KeepUnread(MarkerInputEntries input)
        {
            input.Action = "keepUnread";
            return client.Post("markers", input);
        }

        public Task MarkRead(IMarkerInput input)
        {
            input.Action = "markAsRead";
            return client.Post("markers", input);
        }

        public Task SaveEntry(TagEntryInput entry)
        {
            return client.Put(string.Concat("tags/", WebUtility.UrlEncode(ApiConstants.GlobalTag_Saved.Replace(ApiConstants.FormatKey_UserId, entry.UserId))), entry);
        }

        public Task UnsaveEntry(TagEntryInput entry)
        {
            return client.Delete(string.Format("tags/{0}/{1}", WebUtility.UrlEncode(ApiConstants.GlobalTag_Saved.Replace(ApiConstants.FormatKey_UserId, entry.UserId)), WebUtility.UrlEncode(entry.EntryId)));
        }
    }
}
