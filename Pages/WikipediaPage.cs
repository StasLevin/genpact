using Microsoft.Playwright;
using GenpactTest.Utils;

namespace GenpactTest.Pages
{
    public class WikipediaPage(IPage page)
    {
        private readonly IPage _page = page;
        private const string Url = "https://en.wikipedia.org/wiki/Playwright_(software)";

        public async Task NavigateAsync()
        {
            await _page.GotoAsync(Url);
        }

        public async Task<IList<string>> GetDebuggingFeaturesTextAsync()
        {
            return await PlaywrightHelpers.GetTextsBetweenAsync(_page,
                "role=heading[name=\"Debugging features\"]",
                "role=heading[name=\"Usage Trends\"]"
                );
        }
    }
}
