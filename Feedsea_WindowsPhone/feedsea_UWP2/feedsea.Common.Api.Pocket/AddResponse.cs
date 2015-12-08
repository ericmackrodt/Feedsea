using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace feedsea.Common.Api.Pocket
{
    [DataContract]
    public class AddResponse
    {
        [DataMember(Name = "item")]
        public object Item { get; set; }
        [DataMember(Name = "status")]
        public int Status { get; set; }
    }
}
