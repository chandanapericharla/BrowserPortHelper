using System;
using System.Collections.Generic;

namespace BrowserPortHelper
{
    public class BrowserWindow
    {
        public BrowserType Type;

        public List<BrowserTab> Tabs
        {
            get
            {
                if (tabs == null)
                {
                    tabs = new List<BrowserTab>();
                }

                return tabs;
            }
        }

        public BrowserWindow(BrowserType type)
        {
            this.Type = type;
        }
        
        private List<BrowserTab> tabs;
    }
}