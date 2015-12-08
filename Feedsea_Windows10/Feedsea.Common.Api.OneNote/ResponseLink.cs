using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Feedsea.Common.Api.OneNote
{
    [DataContract]
    public class ResponseLink
    {
        [DataMember(Name = "href")]
        public string Href { get; set; }
    }
}
