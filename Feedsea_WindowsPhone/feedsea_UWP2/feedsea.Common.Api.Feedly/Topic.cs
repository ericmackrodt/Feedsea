using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Api.Feedly
{
    [DataContract]
    public class Topic
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "interest")]
        public Interest Interest { get; set; }
        [DataMember(Name = "updated")]
        public Int64 Updated { get; set; }
        [DataMember(Name = "created")]
        public Int64 Created { get; set; }
    }
}
