using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Api
{
    public abstract class WebClientBase
    {
        protected string baseUrl;
        protected string authBasenUrl;
        protected AuthenticationHeaderValue authentication;

        public virtual string GetLoginUrl(string clientId, string redirectUri, string state)
        {
            return null;
        }

        public WebClientBase()
        {
            baseUrl = BaseUrlAttribute.GetAttributeValue(this.GetType());
            authBasenUrl = LoginUrlAttribute.GetAttributeValue(this.GetType());
        }

        public WebClientBase(string scheme, string parameter)
            : this()
        {
            this.authentication = new AuthenticationHeaderValue(scheme, parameter);
        }

        protected async Task PostRequest(string url, object content)
        {
            using (var client = new HttpClient())
            {
                await UploadRequest(client, HttpMethod.Post, url, content, null);
            }
        }

        protected async Task<TResponse> PostRequest<TResponse>(string url, string htmlContent, Encoding encoding, string mediaType)
        {
            using (var client = new HttpClient())
            {
                var result = await UploadRequest(client, HttpMethod.Post, url, htmlContent, encoding, mediaType);
                object data = await result.Content.ReadAsStringAsync();

                if (typeof(TResponse) == typeof(string))
                    return (TResponse)data;
                                
                try
                {
                    return JsonConvert.DeserializeObject<TResponse>(((string)data).EscapeJson());
                }
                catch (JsonReaderException ex)
                {
                    throw new JsonException(ex.Message, (string)data);
                }
            }
        }

        protected async Task<TResponse> PostRequest<TResponse>(string url, object content)
        {
            using (var client = new HttpClient())
            {
                var result = await UploadRequest(client, HttpMethod.Post, url, content, null);
                var data = await result.Content.ReadAsStringAsync();

                try
                {
                    return JsonConvert.DeserializeObject<TResponse>(data.EscapeJson());
                }
                catch (JsonReaderException ex)
                {
                    throw new JsonException(ex.Message, (string)data);
                }
            }
        }
        protected async Task<TResponse> PostRequest<TResponse>(string url, object content, Dictionary<string, string> customHeaders)
        {
            using (var client = new HttpClient())
            {
                var result = await UploadRequest(client, HttpMethod.Post, url, content, customHeaders);
                var data = await result.Content.ReadAsStringAsync();

                try
                {
                    return JsonConvert.DeserializeObject<TResponse>(data.EscapeJson());
                }
                catch (JsonReaderException ex)
                {
                    throw new JsonException(ex.Message, (string)data);
                }
            }
        }

        protected async Task PutRequest(string url, object content)
        {
            using (var client = new HttpClient())
            {
                await UploadRequest(client, HttpMethod.Put, url, content, null);
            }
        }

        protected async Task<TResponse> PutRequest<TResponse>(string url, object content)
        {
            using (var client = new HttpClient())
            {
                var result = await UploadRequest(client, HttpMethod.Put, url, content, null);
                var data = await result.Content.ReadAsStringAsync();

                try
                {
                    return JsonConvert.DeserializeObject<TResponse>(data.EscapeJson());
                }
                catch (JsonReaderException ex)
                {
                    throw new JsonException(ex.Message, (string)data);
                }
            }
        }

        private async Task<HttpResponseMessage> UploadRequest(HttpClient client, HttpMethod method, string url, string content, Encoding encoding, string mediaType)
        {
            var request = new HttpRequestMessage(method, url);
            if (authentication != null)
                request.Headers.Authorization = authentication;
            request.Content = new StringContent(content, encoding, mediaType);
            var result = await client.SendAsync(request);
            result.EnsureSuccessStatusCode();
            return result;
        }

        private async Task<HttpResponseMessage> UploadRequest(HttpClient client, HttpMethod method, string url, object content, Dictionary<string, string> customHeaders)
        {
            var request = new HttpRequestMessage(method, url);
            if (authentication != null)
                request.Headers.Authorization = authentication;
            request.Content = new StringContent(JsonConvert.SerializeObject(content), new UTF8Encoding(), "application/json");
            if (customHeaders != null && customHeaders.Any())
            {
                foreach (var h in customHeaders)
                {
                    request.Headers.Add(h.Key, h.Value);
                }
            }
            var result = await client.SendAsync(request);
            result.EnsureSuccessStatusCode();
            return result;
        }

        protected async Task<T> GetRequest<T>(string url)
        {
            var handler = new HttpClientHandler();
            
            if (handler.SupportsAutomaticDecompression)
                handler.AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate;

            using (var client = new HttpClient(handler))
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                if (authentication != null)
                    request.Headers.Authorization = authentication;
                var result = await client.SendAsync(request);
                result.EnsureSuccessStatusCode();
                var data = await result.Content.ReadAsStringAsync();
                try
                {
                    return JsonConvert.DeserializeObject<T>(data.EscapeJson());
                }
                catch (JsonReaderException ex)
                {
                    throw new JsonException(ex.Message, data);
                }
            }
        }

        protected async Task DeleteRequest(string url)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, url);
                if (authentication != null)
                    request.Headers.Authorization = authentication;
                var result = await client.SendAsync(request);
                result.EnsureSuccessStatusCode();
            }
        }

        protected string BuildMethodUrl(string path, params QueryStringParam[] queryStringData)
        {
            return baseUrl.BuildUrl(path, queryStringData);
        }

        protected string BuildAuthUrl(string path, params QueryStringParam[] queryStringData) 
        {
            return authBasenUrl.BuildUrl(path, queryStringData);
        }
    }
}
