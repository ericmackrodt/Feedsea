using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Providers.OneNote
{
    public interface IOneNoteProvider : IProvider
    {
        Task AddPage(string content);
    }
}
