using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly
{
    [DataContract]
    public class MarkerInputEntries : IMarkerInput
    {
        public MarkerInputEntries(string[] ids)
        {
            Type = MarkerType.Entries.ToString().ToLower();
            Ids = ids;
        }

        [DataMember(Name = "action")]
        public string Action { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; private set; }
        [DataMember(Name = "entryIds")]
        public string[] Ids { get; private set; }
    }
}
