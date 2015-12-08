using HttpClientHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Api.Instapaper
{
    public class InstapaperApiClient
    {
        private const string BaseUrl = "https://www.instapaper.com/api/";

        public async Task<string> Authenticate(string username, string password)
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    var byteArray = Encoding.UTF8.GetBytes(string.Format("{0}:{1}", username, password));
                    var authData = Convert.ToBase64String(byteArray);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authData);
                    var result = await client.GetAsync("authenticate");
                    statusCode = result.StatusCode;
                    result.EnsureSuccessStatusCode();
                    return authData;
                }
            }
            catch (HttpRequestException ex)
            {
                if (statusCode == HttpStatusCode.Forbidden)
                    return null;
                else
                    throw ex;
            }
        }

        public async Task Add(string authString, AddRequestData addData)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authString);
                var result = await client.GetAsync(QueryHelper.BuildRequestUrl("add", new
                {
                    url = addData.Url,
                    title = addData.Title
                }.BuildQueryString()));
                result.EnsureSuccessStatusCode();
            }
        }
    }
}
