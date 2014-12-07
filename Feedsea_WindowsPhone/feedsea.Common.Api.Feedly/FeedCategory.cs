using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Api.Feedly
{
    [DataContract]
    public class FeedCategory
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "label")]
        public string Label { get; set; }
    }
}
