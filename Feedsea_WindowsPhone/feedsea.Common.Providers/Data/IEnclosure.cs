using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace feedsea.Common.Providers.Data
{
    public interface IEnclosure
    {
        string Href { get; set; }
        string Type { get; set; }
    }
}
