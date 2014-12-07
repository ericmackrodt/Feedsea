using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace feedsea.Common.Api.Pocket
{
    [DataContract]
    public class RequestTokenData
    {
        [DataMember(Name = "consumer_key")]
        public string ConsumerKey { get; set; }

        [DataMember(Name = "redirect_uri")]
        public string RedirectUri { get; set; }
    }
}
