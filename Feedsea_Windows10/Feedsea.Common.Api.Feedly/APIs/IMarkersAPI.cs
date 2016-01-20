using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly.APIs
{
    public interface IMarkersAPI
    {
        Task<CountsResponse> GetCounts(bool? autoRefresh = null, long? newerThan = null);
        Task MarkRead(IMarkerInput input);
        Task KeepUnread(MarkerInputEntries input);
        Task SaveEntry(TagEntryInput entry);
        Task UnsaveEntry(TagEntryInput entry);
        Task<ReadOperations> GetLastestReadOperations();
    }
}
