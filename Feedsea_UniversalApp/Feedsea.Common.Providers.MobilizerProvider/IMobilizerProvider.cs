using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Providers.MobilizerProvider
{
    public interface IMobilizerProvider
    {
        Task<string> GetMobilized(string articleUrl);
    }
}
