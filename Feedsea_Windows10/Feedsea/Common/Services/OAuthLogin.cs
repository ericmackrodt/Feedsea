using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;

namespace Feedsea.Common.Services
{
    public class OAuthLogin : IOAuthLogin
    {
        public async Task<BrokerResult> Login(string loginUrl, string redirectUrl)
        {
            var result = await WebAuthenticationBroker.AuthenticateAsync(Windows.Security.Authentication.Web.WebAuthenticationOptions.None, new Uri(loginUrl), new Uri(redirectUrl));

            string code = result.ResponseData;
            if (result.ResponseStatus == WebAuthenticationStatus.Success && result.ResponseData.Contains("code="))
                code = ParseQueryString(result.ResponseData)["code"];

            return new BrokerResult()
            {
                Status = (BrokerStatus)result.ResponseStatus,
                Code = code
            };
        }

        public static Dictionary<string, string> ParseQueryString(string uri)
        {
            string substring = uri.Substring(((uri.LastIndexOf('?') == -1) ? 0 : uri.LastIndexOf('?') + 1));

            string[] pairs = substring.Split('&');

            Dictionary<string, string> output = new Dictionary<string, string>();

            foreach (string piece in pairs)
            {
                string[] pair = piece.Split('=');
                output.Add(pair[0], pair[1]);
            }

            return output;
        }
    }
}
