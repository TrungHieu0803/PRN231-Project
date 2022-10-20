using System.Text.Json;

namespace Client.Helper
{
    public class CallAPI<T>
    {
        static HttpClient client = new HttpClient();
        public async Task<T> Get(string url)
        {
            var response = await client.GetAsync(url);
            return await JsonSerializer.DeserializeAsync<T>(response.Content.ReadAsStream());

        }

    }
}
