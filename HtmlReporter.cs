using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GenpactTest.Reporter
{
    public static class HtmlReporter
    {
        private static readonly List<string> _testEntries = new List<string>();
        private static readonly string _reportDirName = "TestReports";
        private static readonly string _reportFile = "TestReport.html";
        private static readonly string reportDir;

        static HtmlReporter()
        {
            reportDir = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "..", "..", _reportDirName)).ToString();
            TestContext.Progress.WriteLine("ReportDir: {0}", Path.GetFullPath(reportDir));
        }

        public static void LogTestResult(string stepName, string status = "", string message = "")
        {
            string color = status switch
            {
                "Passed" => "green",
                "Failed" => "red",
                "Skipped" => "orange",
                _ => "black"
            };

            string entry = $@"
<tr>
    <td>{DateTime.Now:HH:mm:ss}</td>
    <td>{stepName}</td>
    <td style='color:{color};'>{status}</td>
    <td>{System.Net.WebUtility.HtmlEncode(message)}</td>
</tr>";

            _testEntries.Add(entry);
        }

        public static void GenerateReport()
        {
            var html = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Test Report</title>
    <style>
        body {{ font-family: Arial; }}
        table {{ border-collapse: collapse; width: 100%; }}
        th, td {{ border: 1px solid #ccc; padding: 5px; }}
        th {{ background-color: #f0f0f0; }}
    </style>
</head>
<body>
    <h2>Test Report - {DateTime.Now:yyyy-MM-dd HH:mm}</h2>
    <table>
        <tr>
            <th>Time</th>
            <th>Step</th>
            <th>Status</th>
            <th>Message</th>
        </tr>
        {string.Join("\n", _testEntries)}
    </table>
</body>
</html>";

            File.WriteAllText(Path.Combine(reportDir, _reportFile), html);
        }
    }
}
