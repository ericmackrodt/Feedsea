using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Providers.Data
{
    public class ContinuedArticles
    {
        public ContinuedArticles(IEnumerable<ArticleData> articles, string continuation)
        {
            Articles = articles;
            Continuation = continuation;
        }

        public IEnumerable<ArticleData> Articles { get; set; }
        public string Continuation { get; set; }
    }
}
