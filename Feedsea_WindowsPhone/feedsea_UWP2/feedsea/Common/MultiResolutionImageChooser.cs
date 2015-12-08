using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace feedsea.Common
{

   public class MultiResolutionImageChooser
   {

      public Uri SplashScreenImage
      {
         get
         {
            switch ( ResolutionHelper.CurrentResolution )
            {
               case Resolutions.HD720p:
                  return new Uri(new Uri("ms-appx://"), "../SplashScreenImage.screen-720p.jpg");
               case Resolutions.WXGA:
                  return new Uri(new Uri("ms-appx://"), "../SplashScreenImage.screen-WXGA.jpg");
               case Resolutions.WVGA:
                  return new Uri(new Uri("ms-appx://"), "../SplashScreenImage.screen-WVGA.jpg");
               default:
                  throw new InvalidOperationException("Unknown resolution type");
            }
         }
      }

   }

}