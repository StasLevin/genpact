using Microsoft.Playwright;
using GenpactTest.Pages;
using GenpactTest.Reporter;
using GenpactTest.Services;
using GenpactTest.Utils;

namespace GenpactTest.Tests
{
    [TestFixture]
    public class DebuggingFeaturesTests : BaseTest
    {
        private IPage page;
        private IBrowser browser;
        private IPlaywright playwright;

        [OneTimeSetUp]
        public async Task Setup()
        {
            TestContext.Progress.WriteLine("Setting up test...");
            HtmlReporter.LogTestResult("Setting up test...");
            playwright = await Playwright.CreateAsync();
            browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });

            page = await browser.NewPageAsync();
            await page.GotoAsync("https://en.wikipedia.org/wiki/Playwright_(software)");
            TestContext.Progress.WriteLine("\nSetup completed");
            HtmlReporter.LogTestResult("Setup completed");
        }

        [Test]
        public async Task Compare_UI_And_API_Debugging_Features_Word_Count()
        {
            TestContext.Progress.WriteLine("\nStarting test...");
            HtmlReporter.LogTestResult("Starting test...");
            var wikiPage = new WikipediaPage(page);

            var uiText = await wikiPage.GetDebuggingFeaturesTextAsync();

            TestContext.Progress.WriteLine("\nStep 1: UI Text normalization");
            HtmlReporter.LogTestResult("Step 1: UI Text normalization");
            var normalizedUI = TextNormalizer.Normalize(uiText);

            foreach (var word in normalizedUI)
            {
                TestContext.Progress.WriteLine(word);
                HtmlReporter.LogTestResult("", "", word);
            }

            var apiClient = new WikipediaApiClient(new HttpClient());
            var apiText = await apiClient.GetDebuggingFeaturesTextAsync();

            TestContext.Progress.WriteLine("\nStep 2: API Text Normalization");
            HtmlReporter.LogTestResult("Step 2: API Text Normalization");
            var normalizedAPI = TextNormalizer.Normalize(apiText);

            foreach (var word in normalizedAPI)
            {
                TestContext.Progress.WriteLine(word);
                HtmlReporter.LogTestResult("", "", word);
            }

            TestContext.Progress.WriteLine("\nStep 3: Asserting UI unique words is equal to API unique words");            
            HtmlReporter.LogTestResult("Step 3: Asserting UI unique words is equal to API unique words");            
            var uiCount = TextNormalizer.UniqueWordCount(normalizedUI);
            var apiCount = TextNormalizer.UniqueWordCount(normalizedAPI);

            TestContext.Progress.WriteLine("UI Unique Words: {0}", uiCount);
            HtmlReporter.LogTestResult("UI Unique Words", "", uiCount.ToString());
            TestContext.Progress.WriteLine("API Unique Words: {0}", apiCount);
            HtmlReporter.LogTestResult("API Unique Words", "", apiCount.ToString());

            Assert.That(uiCount, Is.EqualTo(apiCount), "Assertion failure");
            
        }

        [TearDown]
        public async Task TearDownBrowser()
        {
            if (browser != null)
                await browser.CloseAsync();
            playwright?.Dispose();
            TestContext.Progress.WriteLine("\nTest finished.");
            HtmlReporter.LogTestResult("Test finished.");
        }
    }
}
