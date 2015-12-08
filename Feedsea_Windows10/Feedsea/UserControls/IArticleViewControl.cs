using Feedsea.Common.Providers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.UserControls
{
    public interface IArticleViewControl
    {
        void ScrollToTop(ArticleData article);
    }
}
