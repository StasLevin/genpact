using Microsoft.Playwright;

namespace GenpactTest.Utils
{
    public static class PlaywrightHelpers
    {
        public static async Task<IList<string>> GetTextsBetweenAsync(
            IPage page,
            string startSelector,
            string endSelector,
            bool onlyVisible = true)
        {
            var startElement = page.Locator(startSelector).First;
            var endElement = page.Locator(endSelector).First;

            if (await startElement.CountAsync() == 0 || await endElement.CountAsync() == 0)
                return [];

            var startHandle = await startElement.ElementHandleAsync();
            var endHandle = await endElement.ElementHandleAsync();

            if (startHandle == null || endHandle == null)
                return [];

            var txts = await page.EvaluateAsync<string[]>(
                @"([start, end, onlyVisible]) => {
                const texts = [];
                const isVisible = el => !!(el.offsetWidth || el.offsetHeight || el.getClientRects().length);

                const toSkip = node => {
                    let el = node.nodeType === Node.ELEMENT_NODE ? node : node.parentElement;
                    while (el) {
                        if (el.tagName === 'A') return true;
                        el = el.parentElement;
                    }
                    return false;
                };

                const collect = node => {
                    if (toSkip(node)) return;

                    if (node.nodeType === Node.TEXT_NODE) {
                        const t = node.textContent?.trim();
                        if (t) texts.push(t);
                    } else if (node.nodeType === Node.ELEMENT_NODE) {
                        if (!onlyVisible || isVisible(node)) {
                            const t = node.childNodes.length === 0 ? node.textContent?.trim() : '';
                            if (t) texts.push(t);
                        }
                    }
                };

                let node = start;
                while (node) {
                    collect(node);

                    if (node === end) break;

                    if (node.firstChild) {
                        node = node.firstChild;
                    } else {
                        while (node && !node.nextSibling) {
                            node = node.parentNode;
                            if (node === end) break;
                        }
                        if (node) node = node.nextSibling;
                    }
                }
                return texts;  
            }",
                new object[] { startHandle, endHandle, onlyVisible }
            );
            return txts
            .Where(t => !string.IsNullOrWhiteSpace(t) && t.Length > 1)
            .ToList();
        }
    }
}