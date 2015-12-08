using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Providers.Data
{
    public interface ISearchResult
    {
        string Id { get; set; }
        string Url { get; set; }
        string Title { get; set; }
        string Subscribers { get; set; }
        string Description { get; set; }
        string Tags { get; set; }
    }
}
