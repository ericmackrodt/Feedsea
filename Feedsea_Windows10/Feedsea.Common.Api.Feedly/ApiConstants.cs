using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly
{
    public struct ApiConstants
    {
        public const string FormatKey_UserId = "{userId}";
        public const string FormatKey_Category = "{category}";
        public const string GlobalCategory_Uncategorized = "user/{userId}/category/global.uncategorized";
        public const string GlobalCategory_All = "user/{userId}/category/global.all";
        public const string GlobalCategory_Saved = "user/{userId}/category/global.saved";
        public const string GlobalCategory_Read = "user/{userId}/category/global.read";
        public const string GlobalTag_Saved = "user/{userId}/tag/global.saved";
        public const string GlobalTag_Regex = "user/.*/tag/global.saved";
        public const string CategoryFormat = "user/{userId}/category/{category}";
        public const string LoginDefaultRedirectUrl = "http://localhost";
        public const string BaseServiceUrl = "https://cloud.feedly.com/v3/";
        //public const string BaseServiceUrl = "https://sandbox.feedly.com/v3/";
        public const int NumberArticlesDownload = 20;
    }
}
