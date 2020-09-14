using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InstaCore.Helpers
{
    public class ScrollDown
    {
        public  static async Task Scroll(IWebElement element)
        {
            for (int i = 0; i < 50; i++)
            {
                element.SendKeys(Keys.ArrowDown);
                await Task.Delay(50);
            }
            
        }
        public static async Task ScrollInto(RemoteWebDriver driver, IWebElement element)
        {
            IJavaScriptExecutor je = (IJavaScriptExecutor)driver;
            je.ExecuteScript("arguments[0].scrollIntoView(true);", element);


            await Task.Delay(Constants.LowInterval);

        }
    }
}
