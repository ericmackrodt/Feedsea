using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Controls
{

   public interface IPaidFeatures
   {

      bool IsAdsDisabled { get; }

      string DisableAdsProduct { get; }

      void Update();

      Task<LicenseStatus> BuyFeature(string featureId);

      void FulfillPurchase(string productId);

      bool CheckProductPurchase(string productId);

   }


   public enum LicenseStatus
   {NotPurchased = 0,
   Purchased = 1,
   AlreadyActivated = 2}
}