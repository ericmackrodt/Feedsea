using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Data
{
    public partial class Database
    {
        public Func<RssReaderDataContext, string, int> Query_GetSourceUnreadCount =
            CompiledQuery.Compile((RssReaderDataContext db, string sourceID) => db.NewsSources.First(o => o.UrlID == sourceID).Articles.Count(o => !o.IsRead));

        public Func<RssReaderDataContext, int> Query_GetAllUnreadCount =
            CompiledQuery.Compile((RssReaderDataContext db) => db.Articles.Count(o => !o.IsRead));

        public Func<RssReaderDataContext, string, int> Query_GetSourceTotalCount =
            CompiledQuery.Compile((RssReaderDataContext db, string sourceID) => db.NewsSources.First(o => o.UrlID == sourceID).Articles.Count());

        public Func<RssReaderDataContext, int> Query_GetAllTotalCount =
            CompiledQuery.Compile((RssReaderDataContext db) => db.Articles.Count());

        public Func<RssReaderDataContext, int> Query_GetFavoriteCount =
            CompiledQuery.Compile((RssReaderDataContext db) => db.Articles.Count(o => o.IsFavorite));
        
        public int GetSourceUnreadCount(string sourceID)
        {
            return Query_GetSourceUnreadCount(context, sourceID);
        }

        public int GetAllUnreadCount()
        {
            return Query_GetAllUnreadCount(context);
        }

        public int GetSourceTotalCount(string sourceID)
        {
            return Query_GetSourceTotalCount(context, sourceID);
        }

        public int GetAllTotalCount()
        {
            return Query_GetAllTotalCount(context);
        }

        public int GetFavoriteCount()
        {
            return Query_GetFavoriteCount(context);
        }
    }
}
