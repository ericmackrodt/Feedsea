using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpClientHelpers;

namespace Feedsea.Common.Api.Feedly.APIs
{
    public class SearchAPI : ISearchAPI
    {
        private FeedlyWebClient client;

        internal SearchAPI(FeedlyWebClient client)
        {
            this.client = client;
        }

        public Task<SearchResponse> Feeds(string query)
        {
            return client.Get<SearchResponse>(QueryHelper.BuildRequestUrl("search/feeds", new { q = Uri.EscapeDataString(query) }.BuildQueryString()));
        }

        public Task<SearchResponse> Feeds(string query, int numberOfResults)
        {
            return client.Get<SearchResponse>(QueryHelper.BuildRequestUrl("search/feeds", new { q = Uri.EscapeDataString(query), n = numberOfResults }.BuildQueryString()));
        }
    }
}
