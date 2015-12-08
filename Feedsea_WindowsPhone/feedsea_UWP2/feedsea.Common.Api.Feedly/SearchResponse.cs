using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Api.Feedly
{
    [DataContract]
    public class SearchResponse
    {
        [DataMember(Name = "hint")]
        public string Hint { get; set; }
        [DataMember(Name = "results")]
        public SearchResult[] Results { get; set; }
        [DataMember(Name = "related")]
        public string[] Related { get; set; }
    }
}
