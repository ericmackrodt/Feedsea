using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly.APIs
{
    public interface ICategoriesAPI
    {
        Task<FeedCategory[]> Get();
        Task Delete(string id);
        Task ChangeLabel(string id, string newLabel);
    }
}
