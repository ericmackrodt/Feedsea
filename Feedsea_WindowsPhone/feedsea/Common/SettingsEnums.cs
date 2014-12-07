using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common
{
    public enum ArticleTemplateType
    {
        NormalTemplate = 0,
        NormalTemplateNoImage = 1,
        SmallTemplate = 2,
        SmallTemplateNoImage = 3
    }

    public enum LinkNavigationBrowsers
    {
        InternetExplorer = 0,
        NokiaXpress = 1,
        UCBrowser = 2
    }

    public enum YouTubeClients
    {
        Default = 0,
        Metrotube = 1,
        MyTube = 2,
        Toib = 3,
        Other = 4
    }

    public enum AppTheme
    {
        Default = 0,
        Dark = 1
    }

    public enum VoiceCommandType
    {
        OpenNews = 0,
        SearchFeed = 1,
        None = 2
    }

    public enum Mobilizers
    {
        Page = 0,
        Instapaper = 1,
        Readability = 2,
        Google = 3,
        GoogleNoImg = 4
    }
}
