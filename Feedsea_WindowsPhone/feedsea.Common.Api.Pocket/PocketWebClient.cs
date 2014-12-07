using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using HttpClientHelpers;
using Newtonsoft.Json;

namespace feedsea.Common.Api.Pocket
{
    public class PocketWebClient
    {
        private const string baseUrl = "https://getpocket.com/v3/";
        private const string loginUrl = "https://getpocket.com/auth/";

        public PocketWebClient()
        {

        }

        public string GetLoginUrl(string requestToken, string redirectUri)
        {
            return QueryHelper.BuildRequestUrl(loginUrl, "authorize", new
            {
                request_token =  requestToken,
                redirect_uri = redirectUri,
                mobile = 1
            }.BuildQueryString());
        }

        public async Task<RequestTokenResponse> GetRequestToken(RequestTokenData data)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Add("X-Accept", "application/x-www-form-urlencoded");
                var response = await client.PostAsync("oauth/request", new StringContent(new { consumer_key = data.ConsumerKey, redirect_uri = data.RedirectUri }.BuildQueryString(), new UTF8Encoding(), "application/x-www-form-urlencoded"));
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                var dict = result.ParseQueryString();
                return new RequestTokenResponse()
                {
                    Code = (string)dict["code"]
                };
            }
        }

        public async Task<AccessTokenResponse> GetAccessToken(AccessTokenRequest data)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Add("X-Accept", "application/x-www-form-urlencoded");
                var response = await client.PostAsync("oauth/authorize", new StringContent(new { consumer_key = data.ConsumerKey, code = data.Code }.BuildQueryString(), new UTF8Encoding(), "application/x-www-form-urlencoded"));
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = content.ParseQueryString();
                return new AccessTokenResponse()
                {
                    AccessToken = (string)result["access_token"],
                    Username = (string)result["username"]
                };
            }
        }

        public async Task<AddResponse> Add(AddRequest data)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Add("X-Accept", "application/json");
                var response = await client.PostAsync("add", new StringContent(JsonConvert.SerializeObject(data), new UTF8Encoding(), "application/json"));
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AddResponse>(content);
            }
        }
    }
}
