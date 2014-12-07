using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace feedsea.Common.ApiClient
{
    [ProtoContract]
    public class Response<TData>
    {
        [ProtoMember(1)]
        public TData Data { get; set; }
        [ProtoMember(2)]
        public bool Successful { get; set; }
    }
}
