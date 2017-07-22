using System.Collections.Generic;

namespace BrowserPortHelper
{
    public interface IBrowserHandler
    {
        List<BrowserWindow> GetBrowserWindows();
        void OpenBrowserWindow(BrowserWindow window);
        void OpenBrowserWindows(List<BrowserWindow> window);
    }
}
