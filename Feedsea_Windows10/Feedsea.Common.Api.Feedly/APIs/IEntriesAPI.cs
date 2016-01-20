using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly.APIs
{
    public interface IEntriesAPI
    {
        Task<Entry> GetContent(string id);
        Task<Entry[]> GetMultipleContent(string[] ids);
    }
}
