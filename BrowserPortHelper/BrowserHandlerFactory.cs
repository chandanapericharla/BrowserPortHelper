using System;

namespace BrowserPortHelper
{
    public class BrowserHandlerFactory
    {
        public static IBrowserHandler GetBrowserHandler(BrowserType type)
        {
            switch(type)
            {
                case BrowserType.Chrome:
                    return new ChromeHandler();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
