using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.BackgroundAgent.Common
{
    public enum TileType
    {
        Main,
        Category,
        Subscription
    }

    public class LiveTileData
    {
        public string Title { get; set; }
        public TileFace Front { get; set; }
        public TileFace Back { get; set; }
        public Uri NavigationUri { get; set; }
        public TileType Type { get; set; }
        public int? Count { get; set; }
    }

    public class TileFace
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string Favicon { get; set; }
        public string SourceID { get; set; }
        public string SourceName { get; set; }
        public byte[] FaviconBytes { get; set; }
        public byte[] ImageBytes { get; set; }
    }
}
