using NUnit.Framework;
using GenpactTest.Reporter;

namespace GenpactTest.Tests
{
    [TestFixture]
    public abstract class BaseTest
    {
        [TearDown]
        public void AfterEachTest()
        {
            var ctx = TestContext.CurrentContext;
            HtmlReporter.LogTestResult(
                ctx.Test.Name,
                ctx.Result.Outcome.Status.ToString(),
                ctx.Result.Message ?? ""
            );
        }

        [OneTimeTearDown]
        public void AfterAllTestsInFixture()
        {
            HtmlReporter.GenerateReport();
        }
    }
}
