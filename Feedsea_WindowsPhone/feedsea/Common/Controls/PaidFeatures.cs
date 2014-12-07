using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;

namespace feedsea.Common.Controls
{
    public class PaidFeatures : IPaidFeatures
    {
        public string DisableAdsProduct { get { return "FeedseaDisableAds"; } }
        
        private bool isAdsDisabled;
        public bool IsAdsDisabled
        {
            get { return isAdsDisabled; }
            protected set { isAdsDisabled = value; }
        }

        public PaidFeatures()
        {
            IsAdsDisabled = false;
        }

        public void Update()
        {
            var check = CheckProductPurchase(DisableAdsProduct);
            IsAdsDisabled = check;
        }

        public async Task<LicenseStatus> BuyFeature(string featureId)
        {
           // var listing = await CurrentApp.LoadListingInformationByProductIdsAsync(new string[] { DisableAdsProduct });
            ProductLicense productLicense = null;
            var licenses = CurrentApp.LicenseInformation.ProductLicenses;
            if (CurrentApp.LicenseInformation.ProductLicenses.TryGetValue(DisableAdsProduct, out productLicense))
            {
                if (productLicense.IsActive)
                    return LicenseStatus.AlreadyActivated;
            }
                
            try
            {
                return await ExecutePurchase(featureId);
            }
            catch (SystemException)
            {
                return LicenseStatus.NotPurchased;
            }
        }

        private async Task<LicenseStatus> ExecutePurchase(string productId)
        {
            ProductLicense productLicense = null;

            var result = await CurrentApp.RequestProductPurchaseAsync(productId, false);

            if (CurrentApp.LicenseInformation.ProductLicenses.TryGetValue(DisableAdsProduct, out productLicense))
            {
                if (productLicense.IsActive)
                    return LicenseStatus.Purchased;
            }
            
            return LicenseStatus.NotPurchased;
        }

        public void FulfillPurchase(string productId)
        {
            CurrentApp.ReportProductFulfillment(productId);
        }

        public bool CheckProductPurchase(string productId)
        {
            ProductLicense productLicense = null;

            var result = CurrentApp.LicenseInformation.ProductLicenses.TryGetValue(DisableAdsProduct, out productLicense);
            
            if (result)
                result = productLicense.IsActive;

            return result;
        }
    }
}
