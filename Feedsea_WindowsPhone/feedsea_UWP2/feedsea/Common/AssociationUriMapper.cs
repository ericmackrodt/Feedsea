using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace feedsea.Common
{

   public class AssociationUriMapper
      : WindowsPhoneUWP.UpgradeHelpers.UriMapperBaseHelper
   {
      private string tempUri;

      public override Uri MapUri(Uri uri)
      {
         tempUri = System.Net.WebUtility.UrlDecode(uri.ToString());
         if ( tempUri.Contains("feedsea:Pocket") )
         {
            return new Uri(new Uri("ms-appx://"), "/Views/MainPage.xaml?pocketCode=0");
         }
         if ( tempUri.Contains("feedsea") )
         {
            return new Uri(new Uri("ms-appx://"), "/Views/MainPage.xaml");
         }
         return uri;
      }

   }

}