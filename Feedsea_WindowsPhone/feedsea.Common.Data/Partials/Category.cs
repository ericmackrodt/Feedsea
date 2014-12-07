using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Data
{
    public partial class Category
    {
        public override bool Equals(object obj)
        {
            return this.UrlID == (obj as Category).UrlID;
        }

        public IEnumerable<NewsSource> NewsSources
        {
            get { return NewsSourceCategories.Select(o => o.NewsSource).ToList(); }
        }
    }
}
