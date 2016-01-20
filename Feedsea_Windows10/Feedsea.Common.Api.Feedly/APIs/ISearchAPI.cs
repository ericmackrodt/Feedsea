using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly.APIs
{
    public interface ISearchAPI
    {
        Task<SearchResponse> Feeds(string query);
        Task<SearchResponse> Feeds(string query, int numberOfResults);
    }
}
