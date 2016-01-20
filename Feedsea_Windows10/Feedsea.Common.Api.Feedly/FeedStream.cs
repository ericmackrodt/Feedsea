using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly
{
    [DataContract]
    public class FeedStreamIDs
    {
        [DataMember(Name = "continuation")]
        public string Continuation { get; set; }
        [DataMember(Name = "ids")]
        public string[] Ids { get; set; }
    }

    [DataContract]
    public class FeedStream
    {
        [DataMember(Name = "direction")]
        public string Direction { get; set; }
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "continuation")]
        public string Continuation { get; set; }
        [DataMember(Name = "alternate")]
        public Alternate[] Alternate { get; set; }
        [DataMember(Name = "updated")]
        public long Updated { get; set; }
        [DataMember(Name = "items")]
        public Entry[] Items { get; set; }
    }

    [DataContract]
    public class Alternate 
    {
        [DataMember(Name = "href")]
        public string Href { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}
