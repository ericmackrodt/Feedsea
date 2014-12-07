using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace feedsea.Common.ApiClient
{
    [ProtoContract]
    public class Image
    {
        [ProtoMember(1)]
        public string URL { get; set; }
        //public ImageScope Scope { get; set; }
        [ProtoMember(2)]
        public string ContentType { get; set; }
    }

    [ProtoContract]
    public class Item
    {
        [ProtoMember(1)]
        public string Link { get; set; }
        [ProtoMember(2)]
        public string Title { get; set; }
        [ProtoMember(3)]
        public string Author { get; set; }
        [ProtoMember(4)]
        public string Summary { get; set; }
        [ProtoMember(5)]
        public string PubDate { get; set; }
        [ProtoMember(6)]
        public string UniqueID { get; set; }
        [ProtoMember(7)]
        public string Source { get; set; }
        [ProtoMember(8)]
        public string Image { get; set; }
    }
}
