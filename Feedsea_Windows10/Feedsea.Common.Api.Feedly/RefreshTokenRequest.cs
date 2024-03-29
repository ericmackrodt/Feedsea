﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly
{
    [DataContract]
    public class RefreshTokenRequest
    {
        [DataMember(Name = "refresh_token")]
        public string RefreshToken { get; set; }
        [DataMember(Name = "client_id")]
        public string ClientId { get; set; }
        [DataMember(Name = "client_secret")]
        public string ClientSecret { get; set; }
        [DataMember(Name = "grant_type")]
        public string GrantType { get; set; }
    }
}
