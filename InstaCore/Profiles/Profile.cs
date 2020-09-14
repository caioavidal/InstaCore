using InstaCore.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaCore.Profiles
{
    public class Profile
    {
        private readonly RemoteWebDriver driver;

        public Profile(RemoteWebDriver driver)
        {
            this.driver = driver;
        }
        public bool Enter(string profile) => driver.OpenUrl($"https://www.instagram.com/{profile}", profile);

        public bool IsPrivate(bool enterProfile = false)
        {
            try
            {
                if (driver.FindElements(By.XPath("//*[contains(text(),'Account is Private')]"), 10).Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public int GetFollowingsCount(string profile, bool enterProfile = false)
        {
            if (enterProfile)
            {
                Enter(profile);
            }

            var followingElement = driver.FindElement(By.CssSelector("main header section > ul:first-of-type > li:nth-of-type(3) span"), 60).Text.Replace(",", "");

            return Convert.ToInt32(followingElement);
        }

        public void OpenFollowingDialog()
        {
            driver.Exec(driver.FindElement(By.XPath("//a[contains(@href,'/following/')]"), 60).Click,
                successWhen: () => driver.Url.Contains("/following"));
        }

        public async IAsyncEnumerable<string> GetAllFollowings(string profile, bool enterProfile = true, bool openDialog = true)
        {
            if (enterProfile)
            {
                Enter(profile);
            }

            var followingsCount = GetFollowingsCount(profile);

            if (openDialog)
            {

                driver.Exec(driver.FindElement(By.XPath("//a[contains(@href,'/following/')]"), 60).Click,
                    successWhen: () => driver.Url.Contains("/following"));
            }

            var users = new HashSet<string>();

            var lastTimeFoundNewUser = DateTime.Now;
                
            while (users.Count < followingsCount)
            {

                if((DateTime.Now - lastTimeFoundNewUser).Seconds >= 60)
                {
                    yield break;
                }

                foreach (var user in getUsers())
                {
                    if (users.Add(user))
                    {
                        lastTimeFoundNewUser = DateTime.Now;
                        yield return user;
                    }
                }

                var liElements = driver.FindElements(By.CssSelector("ul > div:first-of-type > li span > a.notranslate"), 60);

                await ScrollDown.ScrollInto(driver, liElements.Last());   
            }
        }

        public async Task<string> GetFollowing(string profile, int position, bool enterProfile = true, bool openDialog = true)
        {
            if (enterProfile)
            {
                Enter(profile);
            }

            var followingsCount = GetFollowingsCount(profile);

            if (position > followingsCount)
            {
                return null;
            }

            if (openDialog)
            {

                driver.Exec(driver.FindElement(By.XPath("//a[contains(@href,'/following/')]"), 60).Click,
                    successWhen: () => driver.Url.Contains("/following"));
            }


            var liElements = driver.FindElements(By.CssSelector("ul > div:first-of-type > li span > a.notranslate"), 30);

            var users = liElements.Select(x => x.Text).ToHashSet();
            Actions actions = new Actions(driver);


            while (position > users.Count())
            {
                liElements = driver.FindElements(By.CssSelector("ul > div:first-of-type > li span > a.notranslate"), 60);

                await ScrollDown.ScrollInto(driver, liElements.Last());

                foreach (var user in getUsers())
                {
                    users.Add(user);
                }
            }

            return users.ElementAt(position - 1);
        }

        private HashSet<string> getUsers() => driver.FindElements(By.CssSelector("li span > a.notranslate"), 60).Select(x => x.Text).ToHashSet();



    }
}
