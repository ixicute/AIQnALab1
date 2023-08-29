using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AIQnALab1
{
    internal class TranslationService
    {
        public async Task<string> DetectLanguage(string text, string translatorEndpoint, string cogSrvKey, string cogSrvRegion)
        {
            object body = new object[] { new { Text = text } };
            var req = Newtonsoft.Json.JsonConvert.SerializeObject(body);
            using (var c = new HttpClient())
            {
                using (var request = new HttpRequestMessage())
                {
                    string path = "/detect?api-version=3.0";
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(translatorEndpoint + path);
                    request.Content = new StringContent(req, Encoding.UTF8, "application/json");
                    request.Headers.Add("Ocp-Apim-Subscription-Key", cogSrvKey);
                    request.Headers.Add("Ocp-Apim-Subscription-Region", cogSrvRegion);

                    HttpResponseMessage res = await c.SendAsync(request).ConfigureAwait(false);
                    string responseContent = await res.Content.ReadAsStringAsync();

                    JArray jsonResponse = JArray.Parse(responseContent);
                    return (string)jsonResponse[0]["language"];
                }
            }
        }

        public async Task<string> TranslateText(string text, string from, string to, string translatorEndpoint, string cogSrvKey, string cogSrvRegion)
        {
            object body = new object[] { new { Text = text } };
            var req = Newtonsoft.Json.JsonConvert.SerializeObject(body);
            using (var c = new HttpClient())
            {
                using (var request = new HttpRequestMessage())
                {
                    string path = $"/translate?api-version=3.0&from={from}&to={to}";
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(translatorEndpoint + path);
                    request.Content = new StringContent(req, Encoding.UTF8, "application/json");
                    request.Headers.Add("Ocp-Apim-Subscription-Key", cogSrvKey);
                    request.Headers.Add("Ocp-Apim-Subscription-Region", cogSrvRegion);

                    HttpResponseMessage res = await c.SendAsync(request).ConfigureAwait(false);
                    string responseContent = await res.Content.ReadAsStringAsync();

                    JArray jsonResponse = JArray.Parse(responseContent);
                    return (string)jsonResponse[0]["translations"][0]["text"];
                }
            }
        }
    }
}
