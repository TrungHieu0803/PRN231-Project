using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Client.Helper
{
    public class CallAPI
    {
        static HttpClient client = new HttpClient();
        
        public async Task<Stream> Get(string url, string? token)
        {

           if(token != null)
            {
                client.DefaultRequestHeaders.Authorization =
   new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.GetAsync(url);
            return response.Content.ReadAsStream();

        }

        public async Task<HttpResponseMessage> Post<T>(string url, T requestBody)
        {
            var json = JsonConvert.SerializeObject(requestBody, Formatting.Indented);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, data);
            return response;
        }

    }
}
