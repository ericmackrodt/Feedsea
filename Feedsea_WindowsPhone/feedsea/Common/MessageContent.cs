using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common
{
    public enum MessageContentType
    {
        UpdateSources,
        ToggleRead,
        SettingsUpdated,
        UpdateFavorites,
        Logoff
    }

    public class MessageContent
    {
        public MessageContent(MessageContentType type, object content)
        {
            Type = type;
            Content = content;
        }

        public MessageContent(MessageContentType type, object content, object content2)
            : this(type, content)
        {
            Content2 = content2;
        }

        public MessageContentType Type { get; set; }
        public object Content { get; set; }
        public object Content2 { get; set; }
    }
}
