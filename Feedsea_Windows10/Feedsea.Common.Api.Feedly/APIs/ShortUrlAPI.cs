using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly.APIs
{
    public class ShortUrlAPI : IShortUrlAPI
    {
        private FeedlyWebClient client;

        internal ShortUrlAPI(FeedlyWebClient client)
        {
            this.client = client;
        }
    }
}
