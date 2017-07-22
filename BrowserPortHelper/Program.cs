using System;
using System.Collections.Generic;
using System.Windows.Automation;
using System.Windows.Forms;

namespace BrowserPortHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            ActivateAllWindows();
            List<BrowserWindow> windows = BrowserHandlerFactory.GetBrowserHandler(BrowserType.Chrome).GetBrowserWindows();
            Testing.PrettyPrintTabs(windows);

            BrowserHandlerFactory.GetBrowserHandler(BrowserType.Chrome).OpenBrowserWindows(windows);
        }

        // Incase the browser window is minimized, the list of open tabs will be empty as nothing is actually rendered.
        // In order to ensure that we do not miss out on minimized windows 
        // we explicitly invoke ATL+TAB key inorder to ensure all tabs are active/maximized
        static void ActivateAllWindows()
        {
            AutomationElement root = AutomationElement.RootElement;
            AutomationElementCollection children = root.FindAll(TreeScope.Children, Condition.TrueCondition);
            for (int i = 1; i <= children.Count; i++)
            {
               for (int j = 0; j <= i; j++)
                {
                    SendKeys.SendWait(KeyCodes.AltPlusTab);
                }
            }
        }
    }

    // Temporary code. 
    class Testing
    {
        public static void PrettyPrintTabs(List<BrowserWindow> windows)
        {
            if (windows == null || windows.Count == 0)
            {
                Console.WriteLine("No windows are currently open");
                return;
            }

            foreach(BrowserWindow window in windows)
            {
                Console.WriteLine("BrowserType: {0}", window.Type.ToString());
                int index = 1;
                foreach(BrowserTab tab in window.Tabs)
                {
                    Console.WriteLine("    Tab-{0}", index);
                    Console.WriteLine("        Title: {0}", tab.Title);
                    Console.WriteLine("        Url: {0}", tab.Url);

                    index++;
                }
            }
        }
    }
}
