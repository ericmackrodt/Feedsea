using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Providers.Data
{
    public interface ICategory : INewsSource
    {
        string URL { get; set; }
        bool IsSelected { get; set; }
        bool Own { get; set; }
        List<SubscriptionData> Subscriptions { get; set; }
    }
}
