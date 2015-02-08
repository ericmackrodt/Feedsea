using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Feedsea.Common.Api.Pocket
{
    [DataContract]
    public class RequestTokenResponse
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }
    }
}
