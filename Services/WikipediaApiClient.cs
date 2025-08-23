using Newtonsoft.Json.Linq;

namespace GenpactTest.Services
{
    public partial class WikipediaApiClient
    {
        private readonly HttpClient _client;

        [GeneratedRegex("<.*?>")]
        private static partial Regex HtmlTagRegex();

        public WikipediaApiClient(HttpClient client)
        {
            _client = client;
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (compatible; MyApp/1.0)");
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<IList<string>> GetDebuggingFeaturesTextAsync()
        {
            var url = "https://en.wikipedia.org/w/api.php?action=parse&page=Playwright_(software)&prop=text&format=json";
            var response = await _client.GetStringAsync(url);

            var json = JObject.Parse(response);
            var html = json["parse"]?["text"]?["*"]?.ToString() ?? string.Empty;

            var start = html.IndexOf("Debugging features");
            var end = html.IndexOf("Usage Trends");
            if (start == -1 || end == -1)
                return [];

            var snippet = html[start..end].Trim();
            var txt = HtmlTagRegex().Replace(snippet, string.Empty);
            txt = txt.Replace("[edit]", string.Empty);
            
            return txt.Split("\n")
            .Where(t => !string.IsNullOrWhiteSpace(t) && t.Length > 1)
            .ToList();
        }
    }
}
