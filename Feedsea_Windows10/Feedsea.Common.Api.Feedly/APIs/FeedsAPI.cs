using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly.APIs
{
    public class FeedsAPI : IFeedsAPI
    {
        private FeedlyWebClient client;

        internal FeedsAPI(FeedlyWebClient client)
        {
            this.client = client;
        }

        public Task<object> GetMetadata()
        {
            throw new NotImplementedException();
        }

        public Task<object> GetMultipleMetadata()
        {
            throw new NotImplementedException();
        }
    }
}
