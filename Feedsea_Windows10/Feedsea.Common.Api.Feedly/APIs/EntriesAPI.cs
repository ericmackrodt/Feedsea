using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly.APIs
{
    public class EntriesAPI : IEntriesAPI
    {
        private FeedlyWebClient client;

        internal EntriesAPI(FeedlyWebClient client)
        {
            this.client = client;
        }

        public Task<Entry> GetContent(string id)
        {
            return client.Get<Entry>(string.Concat("entries/", id));
        }

        public Task<Entry[]> GetMultipleContent(string[] ids)
        {
            return client.Post<Entry[]>("entries/.mget", ids);
        }
    }
}
