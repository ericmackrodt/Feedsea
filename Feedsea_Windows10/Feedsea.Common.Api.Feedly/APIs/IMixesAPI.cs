using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly.APIs
{
    public interface IMixesAPI
    {
        Task<FeedStream> Get(string id, string continuation, int? count = null, int? hours = null, long? newerThan = null, bool? backFill = null, bool? unreadOnly = null);
    }
}
