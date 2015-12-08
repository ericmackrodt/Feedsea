using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Providers.Helpers
{
    public static class UrlHelper
    {
        public static string GetUrlParameter(this string uri, string param)
        {
            Dictionary<String, String> result = new Dictionary<string, string>();
            var query = from match in uri.Split('?').Where(m => m.Contains("="))
                            .SelectMany(pr => pr.Split('&'))
                        where match.Contains("=")
                        select new KeyValuePair<string, String>(
                            match.Split('=')[0],
                            match.Split('=')[1]);

            foreach (var kvp in query.ToList())
            {
                result.Add(kvp.Key, kvp.Value);
            }

            return result[param];
        }

        public static string OnlyLetterOrDigits(this string url)
        {
            var s = "";

            foreach (var c in url)
            {
                if (char.IsLetterOrDigit(c))
                    s += c;
            }

            return s;
        }
    }
}
