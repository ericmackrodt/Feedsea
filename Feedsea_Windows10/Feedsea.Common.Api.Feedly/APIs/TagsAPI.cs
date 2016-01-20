using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly.APIs
{
    public class TagsAPI : ITagsAPI
    {
        private FeedlyWebClient client;

        internal TagsAPI(FeedlyWebClient client)
        {
            this.client = client;
        }

        public Task<Topic[]> GetTopics()
        {
            return client.Get<Topic[]>("topics");
        }

        public Task AddTopic(Topic topic)
        {
            return client.Post("topics", topic);
        }
    }
}
