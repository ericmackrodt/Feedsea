using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Providers.Data
{
    public class SearchResultData
    {
        public SearchResultData()
        {

        }

        public string Url { get; set; }
        public string Title { get; set; }
        public string Id { get; set; }
        public string Subscribers { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
    }
}
