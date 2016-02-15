using Feedsea.Common.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feedsea.Common.Providers.Data;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using Windows.Storage;
using System.Diagnostics;
using System.ComponentModel;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensionsAsync.Extensions;
using SQLite.Net.Async;
using Feedsea.Common.Api.Feedly;

namespace Feedsea.Common.Components
{
    //internal class DbNewsSource
    //{
    //    [PrimaryKey]
    //    public string UrlID { get; set; }
    //    public string Name { get; set; }
    //    public string Link { get; set; }
    //    public int UnreadNumber { get; set; }
    //    public bool IsRoot { get; set; }
    //    public bool IsCategory { get; set; }

    //    [ManyToMany(typeof(DbNewsSourceLink), CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert)]
    //    public List<DbNewsSource> Children { get; set; }

    //    public DbNewsSource()
    //    {

    //    }

    //    public DbNewsSource(INewsSource source, bool isRoot = false)
    //    {
    //        if (source is CategoryData)
    //        {
    //            FromCategory(source);
    //            IsRoot = true;
    //            IsCategory = true;
    //        }
    //        else
    //        {
    //            FromSubscription(source);
    //            IsRoot = isRoot;
    //            IsCategory = false;
    //        }
    //    }

    //    public INewsSource ToNewsSource()
    //    {
    //        if (IsCategory)
    //            return ToCategory();
    //        else
    //            return ToSubscription();
    //    }

    //    private SubscriptionData ToSubscription()
    //    {
    //        var sub = new SubscriptionData()
    //        {
    //            UrlID = UrlID,
    //            Link = Link,
    //            Name = Name,
    //            UnreadNumber = UnreadNumber
    //        };

    //        if (Children != null)
    //            sub.Categories = Children.Select(o => o.ToNewsSource()).Cast<CategoryData>().ToArray();

    //        return sub;
    //    }

    //    private CategoryData ToCategory()
    //    {
    //        var cat = new CategoryData()
    //        {
    //            UrlID = UrlID,
    //            Name = Name,
    //            URL = Link,
    //            UnreadNumber = UnreadNumber
    //        };

    //        if (Children != null)
    //            cat.Subscriptions = Children.Select(o => o.ToNewsSource()).Cast<SubscriptionData>().ToList();

    //        return cat;
    //    }

    //    private void FromSubscription(INewsSource data)
    //    {
    //        var sub = data as SubscriptionData;
    //        UrlID = sub.UrlID;
    //        Name = sub.Name;
    //        Link = sub.Link;
    //        UnreadNumber = sub.UnreadNumber;

    //        if (sub.Categories == null) return;

    //        Children = sub.Categories.Select(o => new DbNewsSource(o)).ToList();
    //    }

    //    private void FromCategory(INewsSource data)
    //    {
    //        var cat = data as CategoryData;
    //        UrlID = cat.UrlID;
    //        Name = cat.Name;
    //        Link = cat.URL;
    //        UnreadNumber = cat.UnreadNumber;

    //        if (cat.Subscriptions != null)
    //            Children = cat.Subscriptions.Select(o => new DbNewsSource(o)).ToList();
    //    }
    //}

    //internal class DbNewsSourceLink
    //{
    //    [ForeignKey(typeof(DbNewsSource))]
    //    public string ParentID { get; set; }

    //    [ForeignKey(typeof(DbNewsSource))]
    //    public string ChildID { get; set; }
    //}

    internal class DbSubscription
    {
        [PrimaryKey]
        public string UrlID { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public int UnreadNumber { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]      // One to many relationship with Valuation
        public List<DbArticle> Articles { get; set; }

        [ManyToMany(typeof(DbCategorySubscription), CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert)]
        public List<DbCategory> Categories { get; set; }

        public DbSubscription() { }
        public DbSubscription(SubscriptionData sub)
        {
            UrlID = sub.UrlID;
            Name = sub.Name;
            Link = sub.Link;
            Image = sub.Image;
            UnreadNumber = sub.UnreadNumber;

            if (sub.Categories == null) return;

            Categories = sub.Categories.Select(o => new DbCategory(o)).ToList();
        }

        public SubscriptionData ToSubscriptionData()
        {
            var sub = new SubscriptionData()
            {
                UrlID = UrlID,
                Link = Link,
                Name = Name,
                Image = Image,
                UnreadNumber = UnreadNumber
            };

            if (Categories != null)
                sub.Categories = Categories.Select(o => o.ToCategoryData()).ToArray();

            return sub;
        }

        public INewsSource ToNewsSource()
        {
            return ToSubscriptionData();
        }
    }

    internal class DbCategory
    {
        [PrimaryKey]
        public string UrlID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public int UnreadNumber { get; set; }

        [ManyToMany(typeof(DbCategorySubscription), CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert)]
        public List<DbSubscription> Subscriptions { get; set; }

        public DbCategory() { }
        public DbCategory(CategoryData cat)
        {
            UrlID = cat.UrlID;
            Name = cat.Name;
            URL = cat.URL;
            UnreadNumber = cat.UnreadNumber;

            if (cat.Subscriptions != null)
                Subscriptions = cat.Subscriptions.Select(o => new DbSubscription(o)).ToList();
        }

        public CategoryData ToCategoryData()
        {
            var cat = new CategoryData()
            {
                UrlID = UrlID,
                Name = Name,
                URL = URL,
                UnreadNumber = UnreadNumber
            };

            if (Subscriptions != null)
                cat.Subscriptions = Subscriptions.Select(o => o.ToSubscriptionData()).ToList();

            return cat;
        }

        public INewsSource ToNewsSource()
        {
            return ToCategoryData();
        }
    }

    public class DbCategorySubscription
    {
        [ForeignKey(typeof(DbCategory))]
        public string CategoryID { get; set; }

        [ForeignKey(typeof(DbSubscription))]
        public string SubscriptionID { get; set; }
    }

    internal class DbArticle
    {
        [PrimaryKey]
        public string UniqueID { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public bool IsRead { get; set; }
        public bool IsFavorite { get; set; }
        public string MainImageUrl { get; set; }
        public string URL { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey(typeof(DbSubscription))]     // Specify the foreign key
        public string SubscriptionID { get; set; }

        [ManyToOne]      // Many to one relationship with Stock
        public DbSubscription Subscription { get; set; }

        //internal EnclosureData[] Enclosure { get; set; }
        public string MobilizedUrl { get; set; }

        public DbArticle() { }

        public DbArticle(ArticleData data)
        {
            UniqueID = data.UniqueID;
            Title = data.Title;
            Summary = data.Summary;
            Content = data.Content;
            Author = data.Author;
            IsRead = data.IsRead;
            IsFavorite = data.IsFavorite;
            MainImageUrl = data.MainImageUrl;
            URL = data.URL;
            Date = data.Date;
            MobilizedUrl = data.MobilizedUrl;

            if (data.Source != null)
                Subscription = new DbSubscription(data.Source);
        }

        public ArticleData ToArticleData()
        {
            var data = new ArticleData()
            {
                UniqueID = UniqueID,
                Title = Title,
                Summary = Summary,
                Content = Content,
                Author = Author,
                IsRead = IsRead,
                IsFavorite = IsFavorite,
                MainImageUrl = MainImageUrl,
                URL = URL,
                Date = Date,
                MobilizedUrl = MobilizedUrl
            };

            if (Subscription != null)
                data.Source = Subscription.ToSubscriptionData();

            return data;
        }
    }



    public class ProviderStorage : IProviderStorage
    {
        private static SQLiteAsyncConnection DbConnection
        {
            get
            {
                var connection = new SQLiteConnectionWithLock(
                    new SQLitePlatformWinRT(), 
                    new SQLiteConnectionString(Path.Combine(ApplicationData.Current.LocalFolder.Path, "Storage.sqlite"), true));
                connection.TraceListener = new DebugTraceListener();
                return new SQLiteAsyncConnection(() => connection);
            }
        }

        public async Task Initialize()
        {
            var con = DbConnection;
            await con.CreateTableAsync<DbSubscription>();
            await con.CreateTableAsync<DbCategory>();
            await con.CreateTableAsync<DbCategorySubscription>();
            await con.CreateTableAsync<DbArticle>();
        }

        public async Task SaveSubscriptions(IEnumerable<SubscriptionData> subscriptions)
        {
            //var db = DbConnection;
            //var subs = subscriptions.Select(o => new DbSubscriptionData(o));

            //await db.InsertOrReplaceAllWithChildrenAsync(subs);
        }

        public async Task SaveCategories(IEnumerable<CategoryData> categories)
        {
            //var db = DbConnection;
            //var cats = categories.Select(o => new DbCategoryData(o));

            //await db.InsertOrReplaceAllWithChildrenAsync(cats);
        }

        public async Task<IEnumerable<INewsSource>> LoadNewsSources()
        {
            var db = DbConnection;
            var sources = await db.GetAllWithChildrenAsync<DbCategory>();
            var query = "SELECT * FROM {0} WHERE NOT EXISTS (SELECT * FROM {1} WHERE {0}.UrlID = {1}.SubscriptionID);";
            var subscriptions = await db.QueryAsync<DbSubscription>(string.Format(query, typeof(DbSubscription).Name, typeof(DbCategorySubscription).Name));
            var result = sources.Select(o => o.ToNewsSource()).Union(subscriptions.Select(o => o.ToNewsSource()));
            //var result = await db.GetAllWithChildrenAsync<DbNewsSource>(o => o.IsRoot == true);
            return result.OrderBy(o => o.GetType().Name).ThenBy(o => o.Name);
        }

        public async Task<IEnumerable<SubscriptionData>> LoadSubscriptions()
        {
            var db = DbConnection;
            var subs = await db.GetAllWithChildrenAsync<DbSubscription>();
            return null; //subs.Cast<Subscription>();
        }

        public async Task<IEnumerable<CategoryData>> LoadCategories()
        {
            var db = DbConnection;
            var cats = await db.GetAllWithChildrenAsync<DbCategory>();
            return cats.Select(o => o.ToCategoryData());
        }

        public async Task<IEnumerable<ArticleData>> LoadArticles()
        {
            return null;
        }

        public async Task<SubscriptionData> GetSubscription(string id)
        {
            var db = DbConnection;
            var subscription = await db.GetWithChildrenAsync<DbSubscription>(id);
            return null;// subscription.ToSubscriptionData();
        }

        public async Task UpdateSubscription(SubscriptionData subscription)
        {
            var db = DbConnection;
            //await db.UpdateAsync(new DbSubscriptionData(subscription));
        }

        public async Task SaveSources(IEnumerable<INewsSource> sources)
        {
            var db = DbConnection;
            var categories = sources.Where(o => o is CategoryData);
            var subscriptions = sources.Where(o => o is SubscriptionData);
            await db.InsertOrReplaceAllWithChildrenAsync(categories.Select(o => new DbCategory(o as CategoryData)));
            await db.InsertOrReplaceAllWithChildrenAsync(subscriptions.Select(o => new DbSubscription(o as SubscriptionData)));
        }

        public async Task UpdateSources(IEnumerable<INewsSource> sources)
        {
            var db = DbConnection;
            var roots = sources.Select(o => o is CategoryData ? new DbCategory(o as CategoryData) : new DbSubscription(o as SubscriptionData) as object);
            await db.UpdateAllAsync(roots);
            var subscriptions = sources.Where(o => o is CategoryData).SelectMany(o => (o as CategoryData).Subscriptions.Select(x => new DbSubscription(x)));
            await db.UpdateAllAsync(subscriptions);
        }

        public async Task ClearNewsSources()
        {
            var db = DbConnection;
            await db.DeleteAllAsync<DbCategorySubscription>();
            await db.DeleteAllAsync<DbCategory>();
            await db.DeleteAllAsync<DbSubscription>();
        }

        public async Task<IEnumerable<ArticleData>> LoadArticles(INewsSource source)
        {
            var db = DbConnection;
            IEnumerable<DbArticle> articles = null;
            //NOT HAPPY WITH THIS IMPLEMENTATION...
            if (source is CategoryData)
            {
                var cat = new DbCategory(source as CategoryData);
                var query =
                    "SELECT * FROM DbArticle WHERE EXISTS " +
                    "(SELECT * FROM DbSubscription WHERE DbArticle.SubscriptionID = DbSubscription.UrlID AND EXISTS " +
                    "(SELECT * FROM DbCategorySubscription WHERE DbSubscription.UrlID = DbCategorySubscription.SubscriptionID AND DbCategorySubscription.CategoryID = '{0}'))";

                articles = await db.QueryAsync<DbArticle>(string.Format(query, source.UrlID));

                if (articles.Any())
                {
                    var subIds = articles.Select(o => o.SubscriptionID).Distinct();
                    
                    var subs = await db.GetAllWithChildrenAsync<DbSubscription>(o => subIds.Contains(o.UrlID));
                    foreach(var article in articles)
                    {
                        article.Subscription = subs.FirstOrDefault(o => o.UrlID == article.SubscriptionID);
                    }
                }
            }
            else
                articles = await db.GetAllWithChildrenAsync<DbArticle>(o => o.SubscriptionID == source.UrlID);

            return articles.Select(o => o.ToArticleData()).OrderByDescending(o => o.Date);
        }

        public async Task<IEnumerable<ArticleData>> LoadArticles(string[] articleIds)
        {
            var db = DbConnection;
            var articles = await db.GetAllWithChildrenAsync<DbArticle>(o => articleIds.Contains(o.UniqueID));

            return articles.Select(o => o.ToArticleData()).OrderByDescending(o => o.Date);
        }

        public async Task SaveArticles(IEnumerable<ArticleData> articles)
        {
            var db = DbConnection;
            var dbArticles = articles.Select(o => new DbArticle(o));
            await db.InsertOrReplaceAllWithChildrenAsync(dbArticles);
        }
    }

    public class DebugTraceListener : ITraceListener
    {
        public void Receive(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
