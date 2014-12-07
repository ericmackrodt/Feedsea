using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace feedsea.Common.ApiClient
{
    [ProtoContract]
    public class LiveTileSourceData
    {
        [ProtoMember(1)]
        public string URL { get; set; }
        [ProtoMember(2)]
        public int SourceID { get; set; }
        [ProtoMember(3)]
        public string SourceName { get; set; }
    }

    [ProtoContract]
    public class LiveTileData
    {
        [ProtoMember(1)]
        public string Title { get; set; }
        [ProtoMember(2)]
        public byte[] Image { get; set; }
        [ProtoMember(3)]
        public DateTime PubDate { get; set; }
        [ProtoMember(4)]
        public int SourceID { get; set; }
        [ProtoMember(5)]
        public string SourceName { get; set; }
    }
}
