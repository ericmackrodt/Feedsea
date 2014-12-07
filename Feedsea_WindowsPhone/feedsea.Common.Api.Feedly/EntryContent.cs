using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace feedsea.Common.Api.Feedly
{
    [DataContract]
    public class EntryContent
    {
        [DataMember(Name = "direction")]
        public string Direction { get; set; }
        [DataMember(Name = "content")]
        public string Content { get; set; }
    }
}
