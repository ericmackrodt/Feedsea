using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace feedsea.Common
{
    public class AssociationUriMapper : UriMapperBase
    {
        private string tempUri;

        public override Uri MapUri(Uri uri)
        {
            tempUri = HttpUtility.UrlDecode(uri.ToString());

            if (tempUri.Contains("feedsea:Pocket"))
            {
                return new Uri("/Views/MainPage.xaml?pocketCode=0", UriKind.Relative);
            }

            if (tempUri.Contains("feedsea"))
            {
                return new Uri("/Views/MainPage.xaml", UriKind.Relative);
            }

            return uri;
        }
    }
}
