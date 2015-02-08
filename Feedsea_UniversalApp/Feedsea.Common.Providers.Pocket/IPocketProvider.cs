using Feedsea.Common.Providers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Providers.Pocket
{
    public interface IPocketProvider : IProvider
    {
        Task PreLogin();

        Task<bool> Add(ArticleData article);
    }
}
