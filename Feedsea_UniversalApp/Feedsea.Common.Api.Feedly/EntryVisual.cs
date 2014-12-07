using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Feedsea.Common.Api.Feedly
{
    [DataContract]
    public class EntryVisual
    {
        [DataMember(Name = "width")]
        public int Width { get; set; }
        [DataMember(Name = "height")]
        public int Height { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "contentType")]
        public string ContentType { get; set; }
    }
}
