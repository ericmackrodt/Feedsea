using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Providers.Data
{
    public class EnclosureData : IEnclosure
    {
        public string Href { get; set; }
        public string Type { get; set; }
    }
}
