using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace feedsea.Common.Api.Pocket
{
    [DataContract]
    public class AccessTokenRequest
    {
        [DataMember(Name = "consumer_key")]
        public string ConsumerKey { get; set; }
        [DataMember(Name = "code")]
        public string Code { get; set; }
    }
}
