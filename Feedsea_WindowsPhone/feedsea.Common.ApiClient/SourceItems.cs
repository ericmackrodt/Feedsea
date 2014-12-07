using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace feedsea.Common.ApiClient
{
    [ProtoContract]
    public class SourceItems
    {
        [ProtoMember(1)]
        public string Link { get; set; }
        [ProtoMember(2)]
        public Item[] Items { get; set; }
    }
}
