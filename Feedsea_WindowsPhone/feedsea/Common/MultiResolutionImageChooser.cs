using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace feedsea.Common
{
    public class MultiResolutionImageChooser
    {
        public Uri SplashScreenImage
        {
            get
            {
                switch (ResolutionHelper.CurrentResolution)
                {
                    case Resolutions.HD720p:
                        return new Uri("../SplashScreenImage.screen-720p.jpg", UriKind.Relative);
                    case Resolutions.WXGA:
                        return new Uri("../SplashScreenImage.screen-WXGA.jpg", UriKind.Relative);
                    case Resolutions.WVGA:
                        return new Uri("../SplashScreenImage.screen-WVGA.jpg", UriKind.Relative);
                    default:
                        throw new InvalidOperationException("Unknown resolution type");
                }
            }
        }
    }
}
