using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Data
{
    [global::System.Data.Linq.Mapping.DatabaseAttribute(Name = "rssreaderdb")]
    public partial class RssReaderDataContext : System.Data.Linq.DataContext
    {
        #region Members

        public static string ConnectionString = "Data Source=isostore:/Database/RssAppDatabase.sdf";

        private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();

        #endregion Members

        #region Extensibility Method Definitions
        partial void OnCreated();
        partial void InsertArticle(Article instance);
        partial void UpdateArticle(Article instance);
        partial void DeleteArticle(Article instance);
        partial void InsertArticleContent(ArticleContent instance);
        partial void UpdateArticleContent(ArticleContent instance);
        partial void DeleteArticleContent(ArticleContent instance);
        partial void InsertArticleImage(ArticleImage instance);
        partial void UpdateArticleImage(ArticleImage instance);
        partial void DeleteArticleImage(ArticleImage instance);
        partial void InsertReadLater(ReadLater instance);
        partial void UpdateReadLater(ReadLater instance);
        partial void DeleteReadLater(ReadLater instance);
        partial void InsertReadQueue(ReadQueue instance);
        partial void UpdateReadQueue(ReadQueue instance);
        partial void DeleteReadQueue(ReadQueue instance);
        partial void InsertSource(NewsSource instance);
        partial void UpdateSource(NewsSource instance);
        partial void DeleteSource(NewsSource instance);
        partial void InsertCategory(Category instance);
        partial void UpdateCategory(Category instance);
        partial void DeleteCategory(Category instance);
        partial void InsertNewsSourceCategory(NewsSourceCategory instance);
        partial void UpdateNewsSourceCategory(NewsSourceCategory instance);
        partial void DeleteNewsSourceCategory(NewsSourceCategory instance);
        #endregion

        #region Constructors

        public RssReaderDataContext() :
            base(ConnectionString)
        {
            OnCreated();
        }

        public RssReaderDataContext(string connection) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public RssReaderDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        #endregion Constructors

        public System.Data.Linq.Table<Article> Articles
        {
            get
            {
                return this.GetTable<Article>();
            }
        }

        public System.Data.Linq.Table<ArticleContent> ArticleContents
        {
            get
            {
                return this.GetTable<ArticleContent>();
            }
        }

        public System.Data.Linq.Table<ArticleImage> ArticleImages
        {
            get
            {
                return this.GetTable<ArticleImage>();
            }
        }

        public System.Data.Linq.Table<ReadLater> ReadLaters
        {
            get
            {
                return this.GetTable<ReadLater>();
            }
        }

        public System.Data.Linq.Table<ReadQueue> ReadQueues
        {
            get
            {
                return this.GetTable<ReadQueue>();
            }
        }

        public System.Data.Linq.Table<NewsSource> NewsSources
        {
            get
            {
                return this.GetTable<NewsSource>();
            }
        }

        public System.Data.Linq.Table<Category> Categories
        {
            get
            {
                return this.GetTable<Category>();
            }
        }

        public System.Data.Linq.Table<NewsSourceCategory> NewsSourceCategories
        {
            get
            {
                return this.GetTable<NewsSourceCategory>();
            }
        }
    }
}
