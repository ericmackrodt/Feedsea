using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly.APIs
{
    public interface ISubscriptionsAPI
    {
        Task<Subscription[]> Get();
        Task Subscribe(string url, string title, FeedCategory[] categories = null);
        Task Delete(string id);
    }
}
