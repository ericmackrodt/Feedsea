using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace feedsea.Common.Api.Feedly
{
    [DataContract]
    public class EntryOrigin
    {
        [DataMember(Name = "streamId")]
        public string StreamId { get; set; }
        [DataMember(Name = "htmlUrl")]
        public string HtmlUrl { get; set; }
        [DataMember(Name = "sid")]
        public string Sid { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
    }
}
