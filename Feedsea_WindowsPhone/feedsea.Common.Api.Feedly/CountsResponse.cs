using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Api.Feedly
{
    [DataContract]
    public class CountsResponse
    {
        [DataMember(Name = "unreadcounts")]
        public UnreadCount[] UnreadCounts { get; set; }
    }
}
