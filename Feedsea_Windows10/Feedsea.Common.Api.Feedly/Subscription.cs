using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly
{
    [DataContract]
    public class Subscription
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "categories")]
        public FeedCategory[] Categories { get; set; }
        [DataMember(Name = "velocity")]
        public double Velocity { get; set; }
        [DataMember(Name = "updated")]
        public long Updated { get; set; }
        [DataMember(Name = "website")]
        public string Website { get; set; }
        [DataMember(Name = "topics")]
        public string[] Topics { get; set; }
    }
}
