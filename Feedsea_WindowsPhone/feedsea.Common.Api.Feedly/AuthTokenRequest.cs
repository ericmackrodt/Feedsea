using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Api.Feedly
{
    [DataContract]
    public class AuthTokenRequest
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }
        [DataMember(Name = "client_id")]
        public string ClientId { get; set; }
        [DataMember(Name = "client_secret")]
        public string ClientSecret { get; set; }
        [DataMember(Name = "redirect_uri")]
        public string RedirectUri { get; set; }
        [DataMember(Name = "state")]
        public string State { get; set; }
        [DataMember(Name = "grant_type")]
        public string GrantType { get; set; }
    }
}
