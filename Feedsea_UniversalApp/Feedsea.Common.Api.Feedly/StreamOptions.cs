using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feedsea.Common.Api.Feedly
{
    public class StreamOptions
    {
        public int Count { get; set; }
        public bool Ranked { get; set; }
        public bool UnreadOnly { get; set; }
        public long NewerThan { get; set; }
    }
}
