using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly
{
    [DataContract]
    public class Entry
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "unread")]
        public bool Unread { get; set; }
        [DataMember(Name = "categories")]
        public FeedCategory[] Categories { get; set; }
        [DataMember(Name = "tags")]
        public Tag[] Tags { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "published")]
        public long Published { get; set; }
        [DataMember(Name = "updated")]
        public long Updated { get; set; }
        [DataMember(Name = "crawled")]
        public long Crawled { get; set; }
        [DataMember(Name = "originId")]
        public string OriginId { get; set; }
        [DataMember(Name = "fingerprint")]
        public string Fingerprint { get; set; }
        [DataMember(Name = "alternate")]
        public Alternate[] Alternate { get; set; }
        [DataMember(Name = "canonical")]
        public Alternate[] Canonical { get; set; }
        [DataMember(Name = "author")]
        public string Author { get; set; }
        [DataMember(Name = "engagement")]
        public int Engagement { get; set; }
        [DataMember(Name = "origin")]
        public EntryOrigin Origin { get; set; }
        [DataMember(Name = "summary")]
        public EntryContent Summary { get; set; }
        [DataMember(Name = "content")]
        public EntryContent Content { get; set; }
        [DataMember(Name = "visual")]
        public EntryVisual Visual { get; set; }
        [DataMember(Name = "enclosure")]
        public EntryEnclosure[] Enclosure { get; set; }
    }
}
