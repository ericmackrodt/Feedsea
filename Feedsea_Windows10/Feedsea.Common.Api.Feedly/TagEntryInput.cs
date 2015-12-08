using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly
{
    [DataContract]
    public class TagEntryInput
    {
        [DataMember(Name = "entryId")]
        public string EntryId { get; set; }

        [IgnoreDataMember]
        public string UserId { get; set; }
    }
}
