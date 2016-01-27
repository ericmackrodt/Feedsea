using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly.APIs
{
    public interface IStreamsAPI
    {
        Task<FeedStream> GetContent(string id, string continuation = null, int? count = null, long? newerThan = null, Ranked? ranked = null, bool? unreadOnly = null);
        Task<FeedStreamIDs> GetIDs(string id, Ranked ranked, bool unreadOnly);
        Task<FeedStreamIDs> GetIDs(string id, string continuation, Ranked ranked, bool unreadOnly);
        Task<FeedStreamIDs> GetIDs(string id, string continuation = null, int? count = null, long? newerThan = null, Ranked? ranked = null, bool? unreadOnly = null);
    }
}
