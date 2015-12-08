using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly
{
    [DataContract]
    public class SearchResult
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "website")]
        public string Website { get; set; }
        [DataMember(Name = "feedId")]
        public string FeedId { get; set; }
        [DataMember(Name = "velocity")]
        public double Velocity { get; set; }
        [DataMember(Name = "subscribers")]
        public int Subscribers { get; set; }
        [DataMember(Name = "curated")]
        public bool Curated { get; set; }
        [DataMember(Name = "deliciousTags")]
        public string[] Tags { get; set; }
        [DataMember(Name = "language")]
        public string Language { get; set; }
        [DataMember(Name = "lastUpdated")]
        public long LastUpdated { get; set; }
        [DataMember(Name = "score")]
        public string Score { get; set; }
        [DataMember(Name = "hint")]
        public string Hint { get; set; }
        [DataMember(Name = "partial")]
        public bool Partial { get; set; }
        [DataMember(Name = "estimatedEngagement")]
        public int EstimatedEngagement { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
    }
}
