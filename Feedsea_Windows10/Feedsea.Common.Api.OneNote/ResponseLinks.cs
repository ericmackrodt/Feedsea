using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Feedsea.Common.Api.OneNote
{
    [DataContract]
    public class ResponseLinks
    {
        [DataMember(Name = "oneNoteClientURL")]
        public ResponseLink OneNoteClientURL { get; set; }
        [DataMember(Name = "oneNoteWebURL")]
        public ResponseLink OneNoteWebURL { get; set; }
    }
}
