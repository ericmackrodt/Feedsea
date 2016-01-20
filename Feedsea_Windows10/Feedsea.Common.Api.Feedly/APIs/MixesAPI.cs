using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpClientHelpers;

namespace Feedsea.Common.Api.Feedly.APIs
{
    public class MixesAPI : IMixesAPI
    {
        private FeedlyWebClient client;

        internal MixesAPI(FeedlyWebClient client)
        {
            this.client = client;
        }

        public Task<FeedStream> Get(string id, string continuation, int? count = default(int?), int? hours = default(int?), long? newerThan = default(long?), bool? backFill = default(bool?), bool? unreadOnly = default(bool?))
        {
            return client.Get<FeedStream>(QueryHelper.BuildRequestUrl("mixes/contents", new
            {
                streamId = id,
                continuation = continuation,
                count = count,
                newerThan = newerThan,
                hours = hours,
                backFill = backFill,
                unreadOnly = unreadOnly
            }.BuildQueryString()));
        }
    }
}
