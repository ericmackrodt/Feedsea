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
    internal class DbSubscriptionData
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Title { get; set; }
        public long Updated { get; set; }
        public string Website { get; set; }
        public string Image { get; set; }

        [ManyToMany(typeof(DbCategorySubscription), CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert)]
        public List<DbCategoryData> Categories { get; set; }

        public static implicit operator Subscription(DbSubscriptionData subscription)
        {
            if (subscription == null) throw new ArgumentNullException();
            return subscription.ToSubscriptionData();
        }

        public static implicit operator DbSubscriptionData(Subscription subscription)
        {
            if (subscription == null) throw new ArgumentNullException();
            return new DbSubscriptionData(subscription);
        }

        public DbSubscriptionData() { }
        public DbSubscriptionData(Subscription sub)
        {
            Id = sub.Id;
            Title = sub.Title;
            Website = sub.Website;
            Updated = sub.Updated;

            if (sub.Categories == null) return;

            Categories = sub.Categories.Select(o => new DbCategoryData(o)).ToList();
        }

        public Subscription ToSubscriptionData()
        {
            var sub = new Subscription()
            {
                Id = Id,
                Website = Website,
                Title = Title,
                Updated = Updated
            };

            if (Categories != null)
                sub.Categories = Categories.Select(o => o.ToCategoryData()).ToArray();

            return sub;
        }
    }

    internal class DbCategoryData
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Label { get; set; }
        public string URL { get; set; }

        [ManyToMany(typeof(DbCategorySubscription), CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert)]
        public List<DbSubscriptionData> Subscriptions { get; set; }

        public static implicit operator FeedCategory(DbCategoryData category)
        {
            if (category == null) throw new ArgumentNullException();
            return category.ToCategoryData();
        }

        public static implicit operator DbCategoryData(FeedCategory category)
        {
            if (category == null) throw new ArgumentNullException();
            return new DbCategoryData(category);
        }

        public DbCategoryData() { }
        public DbCategoryData(FeedCategory cat)
        {
            Id = cat.Id;
            Label = cat.Label;
        }

        public FeedCategory ToCategoryData()
        {
            var cat = new FeedCategory()
            {
                Id = Id,
                Label = Label
            };
            return cat;
        }
    }

    public class DbCategorySubscription
    {
        [ForeignKey(typeof(DbCategoryData))]
        public string CategoryID { get; set; }

        [ForeignKey(typeof(DbSubscriptionData))]
        public string SubscriptionID { get; set; }
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
            //await con.CreateTableAsync<DbSubscriptionData>();
            //await con.CreateTableAsync<DbCategoryData>();
            //await con.CreateTableAsync<DbCategorySubscription>();
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

        public async Task SaveArticles(IEnumerable<ArticleData> articles)
        {
            
        }

        public async Task<IEnumerable<SubscriptionData>> LoadSubscriptions()
        {
            var db = DbConnection;
            var subs = await db.GetAllWithChildrenAsync<DbSubscriptionData>();
            return null; //subs.Cast<Subscription>();
        }

        public async Task<IEnumerable<CategoryData>> LoadCategories()
        {
            var db = DbConnection;
            var cats = await db.GetAllWithChildrenAsync<DbCategoryData>();
            return null; // cats.Select(o => o.ToCategoryData());
        }

        public async Task<IEnumerable<ArticleData>> LoadArticles()
        {
            return null;
        }

        public async Task<SubscriptionData> GetSubscription(string id)
        {
            var db = DbConnection;
            var subscription = await db.GetWithChildrenAsync<DbSubscriptionData>(id);
            return null;// subscription.ToSubscriptionData();
        }

        public async Task UpdateSubscription(SubscriptionData subscription)
        {
            var db = DbConnection;
            //await db.UpdateAsync(new DbSubscriptionData(subscription));
        }

        public async Task SaveSources(IEnumerable<INewsSource> sources)
        {
            var subs = sources.Where(o => o is SubscriptionData).Cast<SubscriptionData>();
            var cats = sources.Where(o => o is CategoryData).Cast<CategoryData>();
            await SaveCategories(cats);
            await SaveSubscriptions(subs);
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
