using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly.APIs
{
    public class CategoriesAPI : ICategoriesAPI
    {
        private FeedlyWebClient client;

        internal CategoriesAPI(FeedlyWebClient client)
        {
            this.client = client;
        }

        public async Task<FeedCategory[]> Get()
        {
            return await client.Get<FeedCategory[]>("categories");
        }

        public async Task Delete(string id)
        {
            await client.Delete(string.Concat("categories/", id));
        }

        public async Task ChangeLabel(string id, string newLabel)
        {
            throw new NotImplementedException();
        }
    }
}
