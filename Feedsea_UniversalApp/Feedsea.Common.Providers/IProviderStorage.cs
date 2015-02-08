using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Providers
{
    public interface IProviderStorage
    {
        void SerializeToStorage(string fileName, object data);
        T DeserializeFromStorage<T>(string fileName);
        void DeleteFile(string fileName);
    }
}
