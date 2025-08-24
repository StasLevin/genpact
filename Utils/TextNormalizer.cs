

namespace GenpactTest.Utils
{
    public static partial class TextNormalizer
    {
        [GeneratedRegex(@"<.*?>")]
        private static partial Regex HtmlTagRegex();

        [GeneratedRegex(@"[^\w\s]")]
        private static partial Regex NonWordRegex();

        [GeneratedRegex(@"\s+")]
        private static partial Regex WhitespaceRegex();
        public static IList<string> Normalize(IList<string> text)
        {
            var list = new List<string>();
            foreach (var t in text)
            {
                var txt = HtmlTagRegex().Replace(t, "");
                txt = NonWordRegex().Replace(txt, "");
                txt = WhitespaceRegex().Replace(txt, " ");
                if (txt.Trim().Length > 0)
                    list.Add(txt.Trim());
            }
            return list;
        }

        public static string Normalize(string text)
        {
            return Normalize([text]).First();
        }

        public static int UniqueWordCount(IList<string> text)
        {
            return string.Join(Environment.NewLine, text).Split(' ').Distinct().Count();
        }
    }
}
