﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly
{
    [DataContract]
    public class UnreadCount
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "count")]
        public int Count { get; set; }
        [DataMember(Name = "updated")]
        public Int64 Updated { get; set; }
    }
}
