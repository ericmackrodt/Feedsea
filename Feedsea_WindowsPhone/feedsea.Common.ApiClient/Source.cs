using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace feedsea.Common.ApiClient
{
    [ProtoContract]
    public class Source
    {
        [ProtoMember(1)]
        public string Title { get; set; }
        [ProtoMember(2)]
        public string Description { get; set; }
        [ProtoMember(3)]
        public string Link { get; set; }
        [ProtoMember(4)]
        public string Id { get; set; }
        [ProtoMember(5)]
        public DateTime PubDate { get; set; }
        [ProtoMember(6)]
        public string ImageLogo { get; set; }
        [ProtoMember(7)]
        public string Culture { get; set; }
        [ProtoMember(8)]
        public string URL { get; set; }
        [ProtoMember(9)]
        public Item[] Items { get; set; }
    }
}
