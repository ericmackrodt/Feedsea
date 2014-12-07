using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace feedsea.Common.Api.Feedly
{
    [DataContract]
    public class EntryEnclosure
    {
        [DataMember(Name = "href")]
        public string Href { get; set; }
        [DataMember(Name = "length")]
        public int Length { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}
