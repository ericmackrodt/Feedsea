using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Data
{
    public partial class RssReaderDataContext
    {
        public void UpdateSourcesUnread()
        {
            foreach (var src in NewsSources)
            {
                src.UpdateUnread();
            }
        }
    }
}
