using Feedsea.Common.Providers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Services
{
    public interface IShareService
    {
        void Share(ArticleData article);
    }
}
