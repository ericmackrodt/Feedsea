using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading;
using feedsea.Common.Data.Helpers;
using System.Threading.Tasks;
using feedsea.Common.Data.Helpers;

namespace feedsea.Common.Data
{
    public partial class Database : IDisposable
    {
        public static AutoResetEvent OperationOnDatabase
            = new AutoResetEvent(true); 

        private RssReaderDataContext context;

        public RssReaderDataContext Context { get { return context; } }

        public Database()
        {
            context = new RssReaderDataContext();
        }

        public Func<RssReaderDataContext, IOrderedQueryable<NewsSource>> Query_GetSource =
            CompiledQuery.Compile((RssReaderDataContext db) => db.NewsSources.OrderBy(o => o.Name));

        public Func<RssReaderDataContext, string, IOrderedQueryable<NewsSource>> Query_GetSourceFromCategory =
            CompiledQuery.Compile((RssReaderDataContext db, string categoryID) => db.NewsSources.Where(o => o.NewsSourceCategories.Any(x => x.CategoryID == categoryID)).OrderBy(o => o.Name));

        public Func<RssReaderDataContext, IOrderedQueryable<NewsSource>> Query_GetUncategorizedSources =
            CompiledQuery.Compile((RssReaderDataContext db) => db.NewsSources.Where(o => o.NewsSourceCategories.All(x => x.Category.Own)).OrderBy(o => o.Name));

        public Func<RssReaderDataContext, string, NewsSource> Query_GetSingleSource =
            CompiledQuery.Compile((RssReaderDataContext db, string sourceID) => db.NewsSources.FirstOrDefault(o => o.UrlID == sourceID));

        public Func<RssReaderDataContext, IOrderedQueryable<Category>> Query_GetCategories =
            CompiledQuery.Compile((RssReaderDataContext db) => db.Categories.OrderByDescending(o => o.Own).ThenBy(o => o.Name));

        public Func<RssReaderDataContext, IOrderedQueryable<Category>> Query_GetCategoriesWithSourcesOnly =
            CompiledQuery.Compile((RssReaderDataContext db) => db.Categories.Where(o => o.NewsSourceCategories.Any()).OrderByDescending(o => o.Own).ThenBy(o => o.Name));

        public Func<RssReaderDataContext, string, Category> Query_GetSingleCategory =
            CompiledQuery.Compile((RssReaderDataContext db, string categoryID) => db.Categories.FirstOrDefault(o => o.UrlID == categoryID));

        public IQueryable<NewsSource> GetSources()
        {
            return Query_GetSource(context);
        }

        public List<NewsSource> GetSourceList()
        {
            return Query_GetSource(context).ToList();
        }

        public IQueryable<NewsSource> GetSourcesFromCategory(string categoryID)
        {
            return Query_GetSourceFromCategory(context, categoryID);
        }

        public IQueryable<NewsSource> GetUncategorizedSources()
        {
            return Query_GetUncategorizedSources(context);
        }

        public NewsSource GetSingleSource(string sourceID)
        {
            return Query_GetSingleSource(context, sourceID);
        }

        public IQueryable<Category> GetCategories()
        {
            return Query_GetCategories(context);
        }

        public IQueryable<Category> GetCategoriesWithSourcesOnly()
        {
            return Query_GetCategoriesWithSourcesOnly(context);
        }

        public List<Category> GetCategoryList()
        {
            return Query_GetCategories(context).ToList();
        }

        public Category GetSingleCategory(string sourceID)
        {
            return Query_GetSingleCategory(context, sourceID);
        }

        public void RemoveSource(NewsSource src, bool submit = false)
        {
            var source = context.NewsSources.FirstOrDefault(o => o.UrlID == src.UrlID);

            context.ArticleContents.DeleteAllOnSubmit(source.Articles.SelectMany(o => o.ArticleContents));
            context.ArticleImages.DeleteAllOnSubmit(source.Articles.SelectMany(o => o.ArticleImages));
            context.Articles.DeleteAllOnSubmit(source.Articles);
            context.NewsSources.DeleteOnSubmit(source);

            if (submit)
                context.SubmitChanges();
        }

        public bool RemoveCategory(Category cat, bool submit = false)
        {
            var categ = context.Categories.FirstOrDefault(o => o.UrlID == cat.UrlID);
            var defaultCat = context.Categories.First(o => o.Own);
            if (categ == null || categ.Own) return false;

            var srcs = categ.NewsSourceCategories.Select(o => o.NewsSource).ToList();

            foreach (var src in srcs)
            {
                if (src.NewsSourceCategories.Count == 1)
                    src.NewsSourceCategories.Add(new NewsSourceCategory() { Category = defaultCat });
                
                src.NewsSourceCategories.Remove(categ.NewsSourceCategories.First(o => o.NewsSourceID == src.UrlID && o.CategoryID == categ.UrlID));    
            }

            context.Categories.DeleteOnSubmit(categ);

            if (submit)
                context.SubmitChanges();
            return true;
        }

        public void MarkAllRead(NewsSource src)
        {            
            IQueryable<Article> articlesToToggle = context.Articles;

            if (src != null)
                articlesToToggle = GetAllArticles_Ordered_FromSource(src.UrlID);

            foreach (var a in articlesToToggle)
                a.IsRead = true;

            if (src != null)
            {
                var source = GetSingleSource(src.UrlID);
                source.UpdateUnread();
            }
            else
                context.UpdateSourcesUnread();

            context.SubmitChanges();
        }

        public bool HasUnread(NewsSource src)
        {
            if (src != null)
                return GetSingleSource(src.UrlID).UnreadNumber > 0;
            else
                return context.NewsSources.Any(o => o.UnreadNumber > 0);
        }

        public Article ToggleArticleRead(int artId)
        {
            return ToggleArticleRead(artId, false);
        }

        public Article ToggleArticleRead(int artId, bool justSetRead)
        {
            var article = GetSingleArticle(artId);
            article.IsRead = justSetRead ? true : !article.IsRead;
            article.NewsSource.UpdateUnread();

            context.SubmitChanges();
            return article;
        }

        public bool ToggleArticleFavorite(Article obj)
        {
            var art = GetSingleArticle(obj.ArticleID);
            art.IsFavorite = !obj.IsFavorite;
            context.SubmitChanges();
            return art.IsFavorite;
        }

        public bool CreateCategory(string url, string categoryName, bool isOwn, bool submit = false)
        {
            if (context.Categories.Any(o => o.UrlID == url)) return false;

            var cat = new Category()
            {
                Name = categoryName,
                Own = isOwn,
                UrlID = url
            };

            context.Categories.InsertOnSubmit(cat);

            if (submit)
                context.SubmitChanges();

            return true;
        }

        public void AddContentToArticle(Article art, string[] contents)
        {
            var article = context.Articles.FirstOrDefault(o => o.ArticleID == art.ArticleID);
            article.ArticleContents.Clear();
            var count = 0;
            foreach (var c in contents)
            {
                article.ArticleContents.Add(new ArticleContent()
                {
                    Content = c,
                    ContentOrder = 0
                });
                count++;
            }
            context.SubmitChanges();
        }

        public void SubmitChanges()
        {
            context.SubmitChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        #region Static_Methods

        public static T Query<T>(Func<Database, T> function)
        {
            using (var db = new Database())
            {
                return (T)function(db);
            }
        }

        public static List<T> QueryAsList<T>(Func<Database, IQueryable<T>> function)
        {
            using (var db = new Database())
            {
                return function(db).ToList();
            }
        }

        public async static Task<T> QueryAsync<T>(Func<Database, T> function)
        {
            return await TaskEx.Run(() => Query(function));
        }

        public async static Task<List<T>> QueryAsListAsync<T>(Func<Database, IQueryable<T>> function)
        {
            return await TaskEx.Run(() => QueryAsList(function));
        }

        public static T QueryIncluded<T>(Func<Database, T> function, params System.Linq.Expressions.Expression<Func<T, object>>[] expressions)
        {
            using (var db = new Database())
            {
                db.Context.LoadIncluded<T>(expressions);
                return (T)function(db);
            }
        }

        public static T QueryIncluded<T>(Func<Database, T> function, params IIncluder[] includers)
        {
            using (var db = new Database())
            {
                db.Context.LoadIncluded(includers);
                return (T)function(db);
            }
        }

        public static List<T> QueryIncludedAsList<T>(Func<Database, IQueryable<T>> function, params System.Linq.Expressions.Expression<Func<T, object>>[] expressions)
        {
            using (var db = new Database())
            {
                db.Context.LoadIncluded<T>(expressions);
                return function(db).ToList();
            }
        }

        public static List<T> QueryIncludedAsList<T>(Func<Database, IQueryable<T>> function, params IIncluder[] includers)
        {
            using (var db = new Database())
            {
                db.Context.LoadIncluded(includers);
                return function(db).ToList();
            }
        }

        public async static Task<T> QueryIncludedAsync<T>(Func<Database, T> function, params IIncluder[] includers)
        {
            return await TaskEx.Run(() => QueryIncluded(function, includers));
        }

        public async static Task<T> QueryIncludedAsync<T>(Func<Database, T> function, params System.Linq.Expressions.Expression<Func<T, object>>[] expressions)
        {
            return await TaskEx.Run(() => QueryIncluded(function, expressions));
        }

        public async static Task<List<T>> QueryIncludedAsListAsync<T>(Func<Database, IQueryable<T>> function, params System.Linq.Expressions.Expression<Func<T, object>>[] expressions)
        {
            return await TaskEx.Run(() => QueryIncludedAsList(function, expressions));
        }

        public async static Task<List<T>> QueryIncludedAsListAsync<T>(Func<Database, IQueryable<T>> function, params IIncluder[] includers)
        {
            return await TaskEx.Run(() => QueryIncludedAsList(function, includers));
        }

        public static void Execute(Action<Database> function)
        {
            using (var db = new Database())
            {
                function(db);
            }
        }

        public async static Task ExecuteAsync(Action<Database> function)
        {
            await TaskEx.Run(() => Execute(function));
        }

        #endregion Static_Methods
    }
}
