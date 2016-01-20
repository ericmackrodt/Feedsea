using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly.APIs
{
    public class SubscriptionsAPI : ISubscriptionsAPI
    {
        private FeedlyWebClient client;

        internal SubscriptionsAPI(FeedlyWebClient client)
        {
            this.client = client;
        }
        
        public Task Delete(string id)
        {
            return client.Delete(string.Concat("subscriptions/", id));
        }

        public Task<Subscription[]> Get()
        {
            return client.Get<Subscription[]>("subscriptions");
        }

        public Task Subscribe(string url, string title, FeedCategory[] categories = null)
        {
            var content = new Subscription()
            {
                Id = url,
                Title = title,
                Categories = categories
            };
            return client.Post("subscriptions", content);
        }
    }
}
