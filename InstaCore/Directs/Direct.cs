using InstaCore.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System.Linq;
using System.Threading.Tasks;

namespace InstaCore.Profiles
{
    public static class Direct
    {
        /// <summary>
        /// Send message to a profile. If message contains any breaklines, bot will use SHIFT+ENTER in order to
        /// send only one message
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task<Insta> SendMessage(this Insta insta, string profile, string message)
        {
            var driver = insta.Driver;

            driver.OpenUrl("https://www.instagram.com/direct/new/", "direct");

            driver.Exec(() => driver.FindElement(By.Name("queryBox"), 60).SendKeys(profile), () => driver.FindElements(By.CssSelector("div[role='dialog'] div[role='button']"), 60).ElementAtOrDefault(0) != null);

            driver.FindElements(By.CssSelector("div[role='dialog'] div[role='button']"), 60).ElementAtOrDefault(0).Click();

            driver.FindElement(By.XPath("//div[contains(text(),'Next')]"), 60).Click();

            foreach (var msg in message.Split("\r\n").Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                driver.FindElement(By.TagName("textarea"), 60).SendKeys(msg);

                await Executor.Exec(() =>
                {
                    Actions a = new Actions(driver);

                    // Press SHIFT-CTRL
                    a.KeyDown(Keys.Shift)
                     .SendKeys(Keys.Enter)
                     .KeyUp(Keys.Shift)
                     .Build()
                     .Perform();
                });
            }

            driver.FindElement(By.TagName("textarea")).SendKeys(Keys.Enter);

            return insta;
        }
        public static Insta OpenDirect(this Insta insta)
        {
            var driver = insta.Driver;

            driver.OpenUrl("https://www.instagram.com/direct/inbox/", "direct/inbox");

            return insta;
        }
    }
}
