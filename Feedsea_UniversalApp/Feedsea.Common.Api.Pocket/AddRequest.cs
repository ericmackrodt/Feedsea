using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Feedsea.Common.Api.Pocket
{
    [DataContract]
    public class AddRequest
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        //[DataMember(Name = "time")]
        //public long Time { get; set; }
        [DataMember(Name = "consumer_key")]
        public string ConsumerKey { get; set; }
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }
    }
}
