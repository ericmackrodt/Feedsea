using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.OneNote
{
    [DataContract]
    public class PagesResponse
    {
        [DataMember(Name = "links")]
        public ResponseLinks Links { get; set; }
    }
}
