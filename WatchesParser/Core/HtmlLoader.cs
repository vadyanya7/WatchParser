using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace WatchesParser.Core
{
    class HtmlLoader
    {
        readonly HttpClient client;
        public readonly string url;

        public HtmlLoader(IParserSettings settings)
        {
            client = new HttpClient();
            url = $"{settings.BaseUrl}";
        }

        public async Task<string> GetSourceAsync(string watchUrl)
        {
            var response = await client.GetAsync(watchUrl);
            string source = null;
            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                source = await response.Content.ReadAsStringAsync();
            }   
            return source;
        }

    }
}
