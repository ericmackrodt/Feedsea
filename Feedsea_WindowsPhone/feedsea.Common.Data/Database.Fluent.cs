using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace feedsea.Common.Data
{
    public partial class Database
    {
        IQueryable<Article> articles;

        public Database GetArticles()
        {
            articles = context.Articles;
            return this;
        }

        public Database Ordered()
        {
            articles = articles.OrderByDescending(o => o.Date).ThenByDescending(o => o.ArticleID);
            return this;
        }

        public Database Interval(int skip, int amount)
        {
            articles = articles.Skip(skip).Take(amount);
            return this;
        }

        public Database FromSource(string sourceID)
        {
            articles = articles.Where(o => o.NewsSourceID == sourceID);
            return this;
        }

        public Database FromCategory(string categoryID)
        {
            articles = articles.Where(o => o.NewsSource.NewsSourceCategories.Any(x => x.CategoryID == categoryID));
            return this;
        }

        public Database UnreadFirst()
        {
            articles = articles.OrderBy(o => o.IsRead);
            return this;
        }

        public IEnumerable<Article> Result() 
        {
            return articles.ToList();
        }
    }
}
