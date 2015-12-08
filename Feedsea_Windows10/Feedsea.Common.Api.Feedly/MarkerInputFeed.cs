using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly
{
    [DataContract]
    public class MarkerInputFeed : IMarkerInput
    {
        public MarkerInputFeed(string[] ids)
        {
            Type = MarkerType.Feeds.ToString().ToLower();
            Ids = ids;
        }

        public MarkerInputFeed(string[] ids, string lastReadEntryId)
            : this(ids)
        {
            LastReadEntryId = lastReadEntryId;
        }

        [DataMember(Name = "action")]
        public string Action { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; private set; }
        [DataMember(Name = "feedIds")]
        public string[] Ids { get; private set; }
        [DataMember(Name = "lastReadEntryId")]
        public string LastReadEntryId { get; set; }
    }
}
