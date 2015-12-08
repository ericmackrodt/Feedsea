using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Api.Feedly
{
    public interface IMarkerInput
    {
        string Action { get; set; }
        string Type { get; }
        string[] Ids { get; }
    }
}
