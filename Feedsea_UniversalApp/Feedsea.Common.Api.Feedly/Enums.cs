using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly
{
    public enum Interest
    {
        High,
        Medium,
        Low
    }

    public enum MarkerAction
    {
        MarkAsRead,
        KeepUnread
    }

    public enum MarkerType
    {
        Entries,
        Feeds,
        Categories
    }

    public enum Ranked
    {
        Oldest,
        Newest
    }
}
