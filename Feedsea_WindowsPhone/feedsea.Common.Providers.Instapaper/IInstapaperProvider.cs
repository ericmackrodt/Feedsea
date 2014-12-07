using feedsea.Common.Providers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Providers.Instapaper
{
    public interface IInstapaperProvider : IProvider
    {
        Task Add(ArticleData article);
    }
}
