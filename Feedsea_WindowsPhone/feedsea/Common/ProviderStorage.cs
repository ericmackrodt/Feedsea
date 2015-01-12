using feedsea.Common.Providers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common
{
    public class ProviderStorage : IProviderStorage
    {
        public T DeserializeFromStorage<T>(string fileName)
        {
            try
            {
                using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!storage.FileExists(fileName))
                        return default(T);

                    using (var file = storage.OpenFile(fileName, FileMode.Open, FileAccess.Read))
                    {
                        using (TextReader xml = new StreamReader(file))
                        {
                            return JsonConvert.DeserializeObject<T>(xml.ReadToEnd());
                        }
                    }
                }
            }
            catch (JsonReaderException)
            {
                DeleteFile(fileName);
                return default(T);
            }
            catch (IsolatedStorageException)
            {
                DeleteFile(fileName);
                return default(T);
            }
        }

        public void SerializeToStorage(string fileName, object data)
        {
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storage.FileExists(fileName))
                    storage.DeleteFile(fileName);

                using (var file = storage.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        var str = JsonConvert.SerializeObject(data);
                        writer.Write(str);
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
        }


        public void DeleteFile(string fileName)
        {
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storage.FileExists(fileName))
                    storage.DeleteFile(fileName);
            }
        }
    }
}
