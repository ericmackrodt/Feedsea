using Feedsea.Common.Providers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Providers
{
    public interface INewsSourceProvider
    {
        Task<IEnumerable<INewsSource>> LoadNewsSources();
        Task<IEnumerable<INewsSource>> DownloadNewsSources();
        Task<KeyValuePair<bool, IEnumerable<INewsSource>>> DownloadNewsSources(IEnumerable<INewsSource> currentCollection);
    }
}
