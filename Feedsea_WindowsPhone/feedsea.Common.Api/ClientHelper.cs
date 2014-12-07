using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace feedsea.Common.Api
{
    public static class ClientHelper
    {
        public static string BuildUrl(this string baseUrl, string path, params QueryStringParam[] queryStringData)
        {
            var queryString = "";

            if (queryStringData != null && queryStringData.Count() > 0)
            {
                queryString = string.Concat("?", BuildQueryString(queryStringData));
            }

            return string.Concat(baseUrl, path, queryString);
        }

        public static string BuildQueryString(params QueryStringParam[] queryStringData)
        {
            var queryString = "";

            if (queryStringData != null && queryStringData.Count() > 0)
            {
                var q = queryStringData.Select(o => string.Concat(o.Key, "=", o.Value));
                queryString = string.Join("&", q);
            }

            return queryString;
        }

        public static string EscapeJson(this string stringToEscape)
        {
            return Regex.Replace(stringToEscape, @"(?<!\\)\\(?!"")(?!n)(?!\\)", @"\\", RegexOptions.IgnorePatternWhitespace);
        }

        public static string EscapeQuery(this string q)
        {
            return string.Join("", q.ToCharArray().Where(o => char.IsLetterOrDigit(o) || char.IsWhiteSpace(o)).Select(o => o.ToString()));
        }

        public static Dictionary<string, string> ParseQueryString(this string uri)
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
