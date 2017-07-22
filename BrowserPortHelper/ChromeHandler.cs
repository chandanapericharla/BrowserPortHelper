using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Automation;
using System.Windows.Forms;

namespace BrowserPortHelper
{
    public class ChromeHandler: IBrowserHandler
    {
        public List<BrowserWindow> GetBrowserWindows()
        {
            List<BrowserWindow> browserWindows = new List<BrowserWindow>();
            foreach (AutomationElement chromeWindow in GetAllChromeWindows())
            {
                List<BrowserTab> tabs = GetAllOpenTabs(chromeWindow);
                if (tabs != null && tabs.Count > 0)
                {
                    BrowserWindow window = new BrowserWindow(BrowserType.Chrome);
                    window.Tabs.AddRange(tabs);

                    browserWindows.Add(window);
                }
            }

            return browserWindows;
        }

        public void OpenBrowserWindows(List<BrowserWindow> windows)
        {
            if (windows == null || windows.Count == 0)
            {
                return;
            }

            foreach(BrowserWindow window in windows)
            {
                OpenBrowserWindow(window);
            }
        }

        public void OpenBrowserWindow(BrowserWindow window)
        {
            if (window == null || window.Tabs.Count == 0)
            {
                return;
            }

            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "chrome.exe";
                process.StartInfo.Arguments = string.Join(", ", window.Tabs.Select(t => t.Url)) + " --new-window";
                process.Start();
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private static List<AutomationElement> GetAllChromeWindows()
        {
            List<AutomationElement> chromeWindows = new List<AutomationElement>();

            AutomationElementCollection windows = GetAllWindows();
            if (windows != null)
            {
                foreach (AutomationElement window in windows)
                {
                    if (IsChromeWindow(window))
                    {
                        chromeWindows.Add(window);
                    }
                }
            }

            return chromeWindows;
        }

        private static AutomationElementCollection GetAllWindows()
        {
            // RootElement compraises of all the windows currently open.
            return AutomationElement.RootElement.FindAll(TreeScope.Children, Condition.TrueCondition);
        }

        private static bool IsChromeWindow(AutomationElement element)
        {
            AutomationElement chromeWindow = null;
            if (element != null)
            {
                chromeWindow = element.FindFirst(
                    TreeScope.Children,
                    new PropertyCondition(AutomationElement.NameProperty, windowName));
            }

            return chromeWindow != null;
        }

        private static List<BrowserTab> GetAllOpenTabs(AutomationElement window)
        {
            List<BrowserTab> tabs = new List<BrowserTab>();
            int noOfOpenTabs = GetOpenTabsCount(window);

            for (int i = 0; i < noOfOpenTabs; i++)
            {
                window.SetFocus();
                SendKeys.SendWait(KeyCodes.CtrlPlusTab);

                BrowserTab tab = new BrowserTab();
                tab.Url = GetCurrentBrowserTabUrl(window);
                tab.Title = window.Current.Name;

                tabs.Add(tab);
            }

            return tabs;
        }

        private static string GetCurrentBrowserTabUrl(AutomationElement window)
        {
            AutomationElement SearchBar = window.FindFirst(
                TreeScope.Descendants,
                new PropertyCondition(AutomationElement.NameProperty, searchBarPropertyName));

            if (SearchBar != null)
            {
                return (string)SearchBar.GetCurrentPropertyValue(ValuePatternIdentifiers.ValueProperty);
            }

            return string.Empty;
        }

        private static int GetOpenTabsCount(AutomationElement window)
        {
            int openTabsCount = 0;

            if (window != null)
            {                
                AutomationElement tabControl = window.FindFirst(
                    TreeScope.Descendants,
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Tab));

                if(tabControl != null)
                {
                    AutomationElementCollection tabsList = tabControl.FindAll(TreeScope.Children, Condition.TrueCondition);

                    // In addition to the user opened tabs, there will always be an addition "New tab" in the tabsList.
                    // Hence we need to subtract it for the actual tabs count
                    openTabsCount = tabsList == null ? 0 : tabsList.Count - 1;
                }
            }

            return openTabsCount;
        }

        private const string windowName = "Google Chrome";
        private const string searchBarPropertyName = "Address and search bar";
    }
}
