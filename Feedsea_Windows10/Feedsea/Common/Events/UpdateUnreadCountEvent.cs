using Broadcaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Events
{
    public class UpdateUnreadCountEvent : BroadcasterEvent<KeyValuePair<string, bool>> { }
}
