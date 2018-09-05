using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Immedia.Core.Helpers
{
    public static class WebRequestHelper
    {
        public static string BaseUrl => "https://api.flickr.com/services/rest/";

        public static async Task<HttpResponseMessage> MakeAsyncRequest(string url, Dictionary<string, string> content)
        {
            var httpClient = new HttpClient
            {
                Timeout = new TimeSpan(0, 5, 0),
                BaseAddress = new Uri(url)
            };

            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type: application/x-www-form-urlencoded", "application/json");

            if (content == null)
            {
                content = new Dictionary<string, string>();
            }

            var encodedContent = new FormUrlEncodedContent(content);

            return await httpClient.PostAsync(httpClient.BaseAddress, encodedContent); ;
        }
    }
}
