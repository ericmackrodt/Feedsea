using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly.APIs
{
    public class ProfileAPI : IProfileAPI
    {
        private FeedlyWebClient client;

        internal ProfileAPI(FeedlyWebClient client)
        {
            this.client = client;
        }

        public Task<Profile> Get()
        {
            return client.Get<Profile>("profile");
        }
    }
}
