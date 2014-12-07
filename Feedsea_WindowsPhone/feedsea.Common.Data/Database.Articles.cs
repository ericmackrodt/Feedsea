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
        #region Listing_Queries

        public Func<RssReaderDataContext, IOrderedQueryable<Article>> Query_GetAllArticles_Ordered =
            CompiledQuery.Compile((RssReaderDataContext db) => db.Articles.OrderByDescending(o => o.Date).ThenByDescending(o => o.ArticleID));

        public Func<RssReaderDataContext, string, IOrderedQueryable<Article>> Query_GetAllArticles_Ordered_FromSource =
            CompiledQuery.Compile((RssReaderDataContext db, string sourceID) => db.Articles.Where(o => o.NewsSourceID == sourceID)
               .OrderByDescending(o => o.Date).ThenByDescending(o => o.ArticleID));

        public Func<RssReaderDataContext, string, IOrderedQueryable<Article>> Query_GetAllArticles_Ordered_FromCategory =
            CompiledQuery.Compile((RssReaderDataContext db, string categoryID) => db.Articles.Where(o => o.NewsSource.NewsSourceCategories.Any(x => x.CategoryID == categoryID))
                .OrderByDescending(o => o.Date).ThenByDescending(o => o.ArticleID));

        public Func<RssReaderDataContext, int, int, IQueryable<Article>> Query_GetAllArticles_Ordered_Interval =
            CompiledQuery.Compile((RssReaderDataContext db, int skip, int amount) => db.Articles.OrderByDescending(o => o.Date).ThenByDescending(o => o.ArticleID).Skip(skip).Take(amount));

        public Func<RssReaderDataContext, int, int, IQueryable<Article>> Query_GetAllArticles_Ordered_Interval_UnreadFirst =
            CompiledQuery.Compile((RssReaderDataContext db, int skip, int amount) => db.Articles.OrderBy(o => o.IsRead).ThenByDescending(o => o.Date).ThenByDescending(o => o.ArticleID).Skip(skip).Take(amount));

        public Func<RssReaderDataContext, IOrderedQueryable<Article>> Query_GetUnreadArticles_Ordered =
            CompiledQuery.Compile((RssReaderDataContext db) => db.Articles.Where(o => !o.IsRead).OrderByDescending(o => o.Date).ThenByDescending(o => o.ArticleID));

        public Func<RssReaderDataContext, int, int, IQueryable<Article>> Query_GetUnreadArticles_Ordered_Interval =
            CompiledQuery.Compile((RssReaderDataContext db, int skip, int amount) => db.Articles.Where(o => !o.IsRead).OrderByDescending(o => o.Date).ThenByDescending(o => o.ArticleID).Skip(skip).Take(amount));

        public Func<RssReaderDataContext, string, int, int, IQueryable<Article>> Query_GetAllArticles_Ordered_Interval_FromSource =
            CompiledQuery.Compile((RssReaderDataContext db, string sourceID, int skip, int amount) => db.Articles.Where(o => o.NewsSourceID == sourceID)
                .OrderByDescending(o => o.Date).ThenByDescending(o => o.ArticleID).Skip(skip).Take(amount));

        public Func<RssReaderDataContext, string, int, int, IQueryable<Article>> Query_GetAllArticles_Ordered_Interval_FromCategory =
            CompiledQuery.Compile((RssReaderDataContext db, string categoryID, int skip, int amount) => db.Articles.Where(o => o.NewsSource.NewsSourceCategories.Any(x => x.CategoryID == categoryID))
                .OrderByDescending(o => o.Date).ThenByDescending(o => o.ArticleID).Skip(skip).Take(amount));

        public Func<RssReaderDataContext, string, int, int, IQueryable<Article>> Query_GetAllArticles_Ordered_Interval_UnreadFirst_FromSource =
            CompiledQuery.Compile((RssReaderDataContext db, string sourceID, int skip, int amount) => db.Articles.Where(o => o.NewsSourceID == sourceID)
                .OrderBy(o => o.IsRead).ThenByDescending(o => o.Date).ThenByDescending(o => o.ArticleID).Skip(skip).Take(amount));

        public Func<RssReaderDataContext, string, int, int, IQueryable<Article>> Query_GetAllArticles_Ordered_Interval_UnreadFirst_FromCategory =
            CompiledQuery.Compile((RssReaderDataContext db, string categoryID, int skip, int amount) => db.Articles.Where(o => o.NewsSource.NewsSourceCategories.Any(x => x.CategoryID == categoryID))
                .OrderBy(o => o.IsRead).ThenByDescending(o => o.Date).ThenByDescending(o => o.ArticleID).Skip(skip).Take(amount));

        public Func<RssReaderDataContext, string, IOrderedQueryable<Article>> Query_GetUnreadArticles_Ordered_FromSource =
            CompiledQuery.Compile((RssReaderDataContext db, string sourceID) => db.Articles.Where(o => o.NewsSourceID == sourceID && !o.IsRead)
                .OrderByDescending(o => o.Date).ThenByDescending(o => o.ArticleID));

        public Func<RssReaderDataContext, string, IOrderedQueryable<Article>> Query_GetUnreadArticles_Ordered_FromCategory =
            CompiledQuery.Compile((RssReaderDataContext db, string categoryID) => db.Articles.Where(o => o.NewsSource.NewsSourceCategories.Any(x => x.CategoryID == categoryID) && !o.IsRead)
                .OrderByDescending(o => o.Date).ThenByDescending(o => o.ArticleID));

        public Func<RssReaderDataContext, string, int, int, IQueryable<Article>> Query_GetUnreadArticles_Ordered_Interval_FromSource =
            CompiledQuery.Compile((RssReaderDataContext db, string sourceID, int skip, int amount) => db.Articles.Where(o => o.NewsSourceID == sourceID && !o.IsRead)
                .OrderByDescending(o => o.Date).ThenByDescending(o => o.ArticleID).Skip(skip).Take(amount));

        public Func<RssReaderDataContext, string, int, int, IQueryable<Article>> Query_GetUnreadArticles_Ordered_Interval_FromCategory =
            CompiledQuery.Compile((RssReaderDataContext db, string categoryID, int skip, int amount) => db.Articles.Where(o => o.NewsSource.NewsSourceCategories.Any(x => x.CategoryID == categoryID) && !o.IsRead)
                .OrderByDescending(o => o.Date).ThenByDescending(o => o.ArticleID).Skip(skip).Take(amount));

        public Func<RssReaderDataContext, IOrderedQueryable<Article>> Query_GetFavorites =
            CompiledQuery.Compile((RssReaderDataContext db) => db.Articles.Where(o => o.IsFavorite).OrderByDescending(o => o.Date).ThenByDescending(o => o.ArticleID));

        public Func<RssReaderDataContext, int, int, IQueryable<Article>> Query_GetFavorites_Interval =
            CompiledQuery.Compile((RssReaderDataContext db, int skip, int amount) => db.Articles.Where(o => o.IsFavorite).OrderByDescending(o => o.Date).ThenByDescending(o => o.ArticleID).Skip(skip).Take(amount));

        #endregion Listing_Queries

        #region Single_Article_Queries

        public Func<RssReaderDataContext, int, Article> Query_GetSingleArticle =
            CompiledQuery.Compile((RssReaderDataContext db, int articleID) => db.Articles.FirstOrDefault(o => o.ArticleID == articleID));

        public Func<RssReaderDataContext, DateTime?, int, Article> Query_GetNextArticle =
            CompiledQuery.Compile((RssReaderDataContext db, DateTime? date, int articleID) => db.Articles.OrderByDescending(o => o.Date)
                .ThenByDescending(o => o.ArticleID).FirstOrDefault(a => a.Date < date || (a.Date == date && a.ArticleID < articleID)));

        public Func<RssReaderDataContext, DateTime?, int, Article> Query_GetPreviousArticle =
            CompiledQuery.Compile((RssReaderDataContext db, DateTime? date, int articleID) => db.Articles.OrderBy(o => o.Date)
                .ThenBy(o => o.ArticleID).FirstOrDefault(a => a.Date > date || (a.Date == date && a.ArticleID > articleID)));

        public Func<RssReaderDataContext, string, DateTime?, int, Article> Query_GetNextArticleFromSource =
            CompiledQuery.Compile((RssReaderDataContext db, string sourceID, DateTime? date, int articleID) => db.Articles.Where(o => o.NewsSourceID == sourceID)
                .OrderByDescending(o => o.Date).ThenByDescending(o => o.ArticleID)
                .FirstOrDefault(a => a.Date < date || (a.Date == date && a.ArticleID < articleID)));

        public Func<RssReaderDataContext, string, DateTime?, int, Article> Query_GetPreviousArticleFromSource =
            CompiledQuery.Compile((RssReaderDataContext db, string sourceID, DateTime? date, int articleID) => db.Articles.Where(o => o.NewsSourceID == sourceID)
                .OrderBy(o => o.Date).ThenBy(o => o.ArticleID).FirstOrDefault(a => a.Date > date || (a.Date == date && a.ArticleID > articleID)));

        public Func<RssReaderDataContext, string, DateTime?, int, Article> Query_GetNextArticleFromCategory =
            CompiledQuery.Compile((RssReaderDataContext db, string categoryID, DateTime? date, int articleID) => db.Categories.FirstOrDefault(o => o.UrlID == categoryID).NewsSourceCategories.SelectMany(o => o.NewsSource.Articles)
                .OrderByDescending(o => o.Date).ThenByDescending(o => o.ArticleID)
                .FirstOrDefault(a => a.Date < date || (a.Date == date && a.ArticleID < articleID)));

        public Func<RssReaderDataContext, string, DateTime?, int, Article> Query_GetPreviousArticleFromCategory =
            CompiledQuery.Compile((RssReaderDataContext db, string categoryID, DateTime? date, int articleID) => db.Categories.FirstOrDefault(o => o.UrlID == categoryID).NewsSourceCategories.SelectMany(o => o.NewsSource.Articles)
                .OrderBy(o => o.Date).ThenBy(o => o.ArticleID).FirstOrDefault(a => a.Date > date || (a.Date == date && a.ArticleID > articleID)));

        #endregion Single_Article_Queries

        #region Verification_Queries

        public Func<RssReaderDataContext, Article, bool> Query_HasNextArticle =
            CompiledQuery.Compile((RssReaderDataContext db, Article article) => db.Articles.Any(a => a.Date < article.Date || (a.Date == article.Date && a.ArticleID < article.ArticleID)));

        public Func<RssReaderDataContext, Article, bool> Query_HasPreviousArticle =
            CompiledQuery.Compile((RssReaderDataContext db, Article article) => db.Articles.Any(a => a.Date > article.Date || (a.Date == article.Date && a.ArticleID > article.ArticleID)));

        public Func<RssReaderDataContext, string, Article, bool> Query_HasNextArticleFromSource =
           CompiledQuery.Compile((RssReaderDataContext db, string sourceID, Article article) => db.NewsSources.FirstOrDefault(o => o.UrlID == sourceID).Articles.Any(a => a.Date < article.Date || (a.Date == article.Date && a.ArticleID < article.ArticleID)));

        public Func<RssReaderDataContext, string, Article, bool> Query_HasPreviousArticleFromSource =
            CompiledQuery.Compile((RssReaderDataContext db, string sourceID, Article article) => db.NewsSources.FirstOrDefault(o => o.UrlID == sourceID).Articles.Any(a => a.Date > article.Date || (a.Date == article.Date && a.ArticleID > article.ArticleID)));

        public Func<RssReaderDataContext, string, Article, bool> Query_HasNextArticleFromCategory =
            CompiledQuery.Compile((RssReaderDataContext db, string categoryID, Article article) => db.Categories.FirstOrDefault(o => o.UrlID == categoryID).NewsSourceCategories.SelectMany(o => o.NewsSource.Articles).Any(a => a.Date < article.Date || (a.Date == article.Date && a.ArticleID < article.ArticleID)));

        public Func<RssReaderDataContext, string, Article, bool> Query_HasPreviousArticleFromCategory =
            CompiledQuery.Compile((RssReaderDataContext db, string categoryID, Article article) => db.Categories.FirstOrDefault(o => o.UrlID == categoryID).NewsSourceCategories.SelectMany(o => o.NewsSource.Articles).Any(a => a.Date > article.Date || (a.Date == article.Date && a.ArticleID > article.ArticleID)));

        #endregion Verification_Queries

        #region Listing_Methods

        public IQueryable<Article> GetAllArticles_Ordered()
        {
            return Query_GetAllArticles_Ordered(context);
        }

        public IQueryable<Article> GetAllArticles_Ordered_FromSource(string sourceID)
        {
            return Query_GetAllArticles_Ordered_FromSource(context, sourceID);
        }

        public IQueryable<Article> GetAllArticles_Ordered_FromCategory(string categoryID)
        {
            return Query_GetAllArticles_Ordered_FromCategory(context, categoryID);
        }

        public IQueryable<Article> GetAllArticles_Ordered_Interval(int skip, int amount)
        {
            return Query_GetAllArticles_Ordered_Interval(context, skip, amount);
        }

        public IQueryable<Article> GetAllArticles_Ordered_Interval_FromSource(string sourceID, int skip, int amount)
        {
            return Query_GetAllArticles_Ordered_Interval_FromSource(context, sourceID, skip, amount);
        }

        public IQueryable<Article> GetAllArticles_Ordered_Interval_FromCategory(string categoryID, int skip, int amount)
        {
            return Query_GetAllArticles_Ordered_Interval_FromCategory(context, categoryID, skip, amount);
        }

        public IQueryable<Article> GetAllArticles_Ordered_Interval_UnreadFirst(int skip, int amount)
        {
            return Query_GetAllArticles_Ordered_Interval_UnreadFirst(context, skip, amount);
        }

        public IQueryable<Article> GetAllArticles_Ordered_Interval_UnreadFirst_FromSource(string sourceID, int skip, int amount)
        {
            return Query_GetAllArticles_Ordered_Interval_UnreadFirst_FromSource(context, sourceID, skip, amount);
        }

        public IQueryable<Article> GetAllArticles_Ordered_Interval_UnreadFirst_FromCategory(string categoryID, int skip, int amount)
        {
            return Query_GetAllArticles_Ordered_Interval_UnreadFirst_FromCategory(context, categoryID, skip, amount);
        }

        public IQueryable<Article> GetUnreadArticles_Ordered()
        {
            return Query_GetUnreadArticles_Ordered(context);
        }

        public IQueryable<Article> GetUnreadArticles_Ordered_FromSource(string sourceID)
        {
            return Query_GetUnreadArticles_Ordered_FromSource(context, sourceID);
        }

        public IQueryable<Article> GetUnreadArticles_Ordered_FromCategory(string categoryID)
        {
            return Query_GetUnreadArticles_Ordered_FromCategory(context, categoryID);
        }

        public IQueryable<Article> GetUnreadArticles_Ordered_Interval(int skip, int amount)
        {
            return Query_GetUnreadArticles_Ordered_Interval(context, skip, amount);
        }

        public IQueryable<Article> GetUnreadArticles_Ordered_Interval_FromSource(string sourceID, int skip, int amount)
        {
            return Query_GetUnreadArticles_Ordered_Interval_FromSource(context, sourceID, skip, amount);
        }

        public IQueryable<Article> GetUnreadArticles_Ordered_Interval_FromCategory(string categoryID, int skip, int amount)
        {
            return Query_GetUnreadArticles_Ordered_Interval_FromCategory(context, categoryID, skip, amount);
        }

        public IQueryable<Article> GetFavorites()
        {
            return Query_GetFavorites(context);
        }

        public IQueryable<Article> GetFavorites_Interval(int skip, int amount)
        {
            return Query_GetFavorites_Interval(context, skip, amount);
        }

        #endregion Listing_Methods

        #region Single_Article_Methods

        public Article GetSingleArticle(int articleID)
        {
            return Query_GetSingleArticle(context, articleID);
        }

        public Article GetNextArticle(Article article, ISource source) 
        {
            if (source == null)
                return Query_GetNextArticle(context, article.Date, article.ArticleID);

            if (source is Category)
                return Query_GetNextArticleFromCategory(context, source.UrlID, article.Date, article.ArticleID);
            else
                return Query_GetNextArticleFromSource(context, source.UrlID, article.Date, article.ArticleID);
        }

        public Article GetPreviousArticle(Article article, ISource source) 
        {
            if (source == null)
                return Query_GetPreviousArticle(context, article.Date, article.ArticleID);

            if (source is Category)
                return Query_GetPreviousArticleFromCategory(context, source.UrlID, article.Date, article.ArticleID);
            else
                return Query_GetPreviousArticleFromSource(context, source.UrlID, article.Date, article.ArticleID);
        }

        //public Article GetNextArticle(DateTime? currentDate, int articleID)
        //{
        //    return Query_GetNextArticle(context, currentDate, articleID);
        //}

        //public Article GetNextArticleFromSource(int sourceID, DateTime? currentDate, int articleID)
        //{
        //    return Query_GetNextArticleFromSource(context, sourceID, currentDate, articleID);
        //}

        //public Article GetPreviousArticle(DateTime? currentDate, int articleID)
        //{
        //    return Query_GetPreviousArticle(context, currentDate, articleID);
        //}

        //public Article GetPreviousArticleFromSource(int sourceID, DateTime? currentDate, int articleID)
        //{
        //    return Query_GetPreviousArticleFromSource(context, sourceID, currentDate, articleID);
        //}

        #endregion Single_Article_Methods

        #region Verification_Methods

        public bool HasNextArticle(Article article)
        {
            return HasNextArticle(article, null);
        }

        public bool HasNextArticle(Article article, ISource source)
        {
            if (source == null)
                return Query_HasNextArticle(context, article);

            if (source is Category)
                return Query_HasNextArticleFromCategory(context, source.UrlID, article);
            else
                return Query_HasNextArticleFromSource(context, source.UrlID, article);
        }

        //public bool HasNextArticleFromSource(int sourceID, Article article)
        //{
        //    return Query_HasNextArticleFromSource(context, sourceID, article);
        //}

        public bool HasPreviousArticle(Article article)
        {
            return Query_HasPreviousArticle(context, article);
        }

        public bool HasPreviousArticle(Article article, ISource source)
        {
            if (source == null)
                return Query_HasPreviousArticle(context, article);

            if (source is Category)
                return Query_HasPreviousArticleFromCategory(context, source.UrlID, article);
            else
                return Query_HasPreviousArticleFromSource(context, source.UrlID, article);
        }

        //public bool HasPreviousArticleFromSource(int sourceID, Article article)
        //{
        //    return Query_HasPreviousArticleFromSource(context, sourceID, article);
        //}

        #endregion Verification_Methods
    }
}
