using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaCore.Helpers
{
    public static class Executor
    {
        public static bool OpenUrl(this IWebDriver driver, string url, string urlToTest, int timeoutInSeconds = 60, bool checkPopup = true)
        {
            if (timeoutInSeconds > 0)
            {
                driver.Navigate().GoToUrl(url);

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));


                wait.Until(drv => drv.Url.Contains(urlToTest, StringComparison.OrdinalIgnoreCase));

                if (checkPopup)
                {
                    return removePopup(driver);
                }
                return true;

            }


            return true;
        }
        public static bool Exec(this IWebDriver driver, Action action, Func<bool> successWhen, int timeoutInSeconds = 60)
        {
            if (timeoutInSeconds > 0)
            {

                action?.Invoke();

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));


                return wait.Until(drv => successWhen?.Invoke() ?? false);
            }

            return true;
        }


        private static bool removePopup(IWebDriver driver)
        {
            try
            {
                var notificationPopup = driver.FindElements(By.XPath("//*[contains(text(),'Not Now')]"), 10);

                if (notificationPopup.Count > 0)
                {
                    notificationPopup.FirstOrDefault().Click();
                }
            }
            catch
            {

            }
            return true;
        }



        public static async Task Exec(Action action)
        {
            action?.Invoke();
            await Task.Delay(Constants.LowInterval);
        }

        public static IWebElement FindElement(this IWebDriver driver, By by, double timeoutInSeconds = 60)
        {

            if (timeoutInSeconds > 0)
            {

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

                return wait.Until(drv => drv.FindElement(by));

            }

            return driver.FindElement(by);

        }

        public static IList<IWebElement> FindElements(this IWebDriver driver, By by, double timeoutInSeconds = 30)
        {

            if (timeoutInSeconds > 0)
            {

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

                wait.Until(drv => drv.FindElements(by).Count > 0);
            }

            return driver.FindElements(by);

        }
    }
}
