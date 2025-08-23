using Microsoft.Playwright;
using GenpactTest.Pages;
using GenpactTest.Services;
using GenpactTest.Utils;

namespace GenpactTest.Tests
{
    [TestFixture]
    public class DebuggingFeaturesTests
    {
        private IPage page;
        private IBrowser browser;
        private IPlaywright playwright;

        [OneTimeSetUp]
        public async Task Setup()
        {
            TestContext.Progress.WriteLine("Setting up test...");
            playwright = await Playwright.CreateAsync();
            browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });

            page = await browser.NewPageAsync();
            await page.GotoAsync("https://en.wikipedia.org/wiki/Playwright_(software)");
            TestContext.Progress.WriteLine("SetUP completed\n");
        }

        [Test]
        public async Task Compare_UI_And_API_Debugging_Features_Word_Count()
        {
            TestContext.Progress.WriteLine("Starting test...");
            var wikiPage = new WikipediaPage(page);

            var uiText = await wikiPage.GetDebuggingFeaturesTextAsync();

            TestContext.Progress.WriteLine("\nStep 1: UI Text normalization");
            var normalizedUI = TextNormalizer.Normalize(uiText);

            foreach (var word in normalizedUI)
            {
                TestContext.Progress.WriteLine(word);
            }

            var apiClient = new WikipediaApiClient(new HttpClient());
            var apiText = await apiClient.GetDebuggingFeaturesTextAsync();

            TestContext.Progress.WriteLine("\nStep 2: API Text Normalization");
            var normalizedAPI = TextNormalizer.Normalize(apiText);

            foreach (var word in normalizedAPI)
            {
                TestContext.Progress.WriteLine(word);
            }

            TestContext.Progress.WriteLine("\nStep 3: Asserting UI unique words is equal to API unique words");            
            var uiCount = TextNormalizer.UniqueWordCount(normalizedUI);
            var apiCount = TextNormalizer.UniqueWordCount(normalizedAPI);

            TestContext.Progress.WriteLine("UI Unique Words: {0}", uiCount);
            TestContext.Progress.WriteLine("API Unique Words: {0}", apiCount);

            Assert.That(uiCount, Is.EqualTo(apiCount), "Assertion failure");

        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            if (browser != null)
                await browser.CloseAsync();
            playwright?.Dispose();
            TestContext.Progress.WriteLine("\nTest finished.");
        }
    }
}
