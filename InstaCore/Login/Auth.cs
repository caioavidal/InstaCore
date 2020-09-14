using InstaCore.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InstaCore.Login
{
    public class Auth
    {
        private readonly RemoteWebDriver driver;

        public Auth(RemoteWebDriver driver)
        {
            this.driver = driver;
        }

        public async Task LogIn(string username, string password)
        {
            driver.OpenUrl("https://www.instagram.com", "instagram");

            driver.FindElement(By.Name("username"), 60)?.SendKeys(username);

            driver.FindElement(By.Name("password"), 60).SendKeys(password);

            driver.FindElement(By.CssSelector("button[type=\"submit\"]"), 60).Click();

            driver.FindElement(By.CssSelector("img[alt='Instagram']"), 60).Click();

            removePopup();
        }

        private void removePopup()
        {
            try
            {
                var notificationPopup = driver.FindElements(By.XPath("//*[contains(text(),'Not Now')]"), 60);

                if (notificationPopup.Count > 0)
                {
                    notificationPopup.FirstOrDefault().Click();
                }
            }
            catch
            {

            }
        }


    }
}
