using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace feedsea.Common.Data
{
    public interface ISource
    {
        string UrlID { get; set; }
        string Name { get; set; }
    }
}
