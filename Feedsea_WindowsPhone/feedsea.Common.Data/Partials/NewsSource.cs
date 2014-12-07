using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Data
{
    public partial class NewsSource
    {
        public void UpdateUnread()
        {
            UnreadNumber = Articles.Count(o => !o.IsRead);
        }
    }
}
