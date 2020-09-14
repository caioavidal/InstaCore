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
    public static class Auth
    {
        /// <summary>
        /// Log in user
        /// </summary>
        /// <param name="insta"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static Insta LogIn(this Insta insta, string username, string password)
        {
            var driver = insta.Driver;

            driver.OpenUrl("https://www.instagram.com", "instagram", checkPopup: false);

            driver.FindElement(By.Name("username"), 60)?.SendKeys(username);

            driver.FindElement(By.Name("password"), 60).SendKeys(password);

            driver.FindElement(By.CssSelector("button[type=\"submit\"]"), 60).Click();

            driver.FindElement(By.CssSelector("img[alt='Instagram']"), 60);

            driver.OpenUrl("https://www.instagram.com", "instagram");

            return insta;
        }


    }
}
