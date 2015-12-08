using Feedsea.Common.Providers.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Providers
{
    public class RefreshResult
    {
        public IEnumerable<ArticleData> Articles { get; set; }
        public IEnumerable<INewsSource> Sources { get; set; }
    }
}
