using feedsea.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace feedsea.Common.Converters
{
    public class IsoStorageImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter == null)
                throw new Exception("You have to set a default image as parameter!");

            BitmapImage bi = new BitmapImage();

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var fileName = string.Concat(InternalSettings.FolderFavicons, "/", string.Format(InternalSettings.SourceFaviconName, value));

                if (!myIsolatedStorage.FileExists(fileName))
                    bi.UriSource = new Uri((string)parameter, UriKind.Relative);
                else
                {
                    using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(fileName, FileMode.Open, FileAccess.Read))
                    {
                        bi.SetSource(fileStream);
                    }
                }
            }
            return bi;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
