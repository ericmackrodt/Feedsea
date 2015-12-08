using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Feedsea.Common.Api.Feedly
{
    [DataContract]
    public class Tag
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "label")]
        public string Label { get; set; }
    }
}
