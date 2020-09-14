using InstaCore.Helpers;
using InstaCore.Profiles;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InstaCore.Posts
{
    public class Post
    {
        private readonly RemoteWebDriver driver;

        public Post(RemoteWebDriver driver)
        {
            this.driver = driver;
        }

        public async Task<string> EnterPost(string profile, int position)
        {
            var user = new Profile(driver);
            user.Enter(profile);

            if (user.IsPrivate())
            {
                return null;
            }

            var post = await getPost(position);

            driver.Exec(action: post.Value.Click,
               successWhen: () => driver.Url.Contains(post.Key));

            return post.Key;
        }

        public bool IsPrivate()
        {
            try
            {
                if (driver.FindElements(By.XPath("//*[contains(text(),'this post have been limited')]"), 10).Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public async Task<bool> Comment(string profile, int post, string comment)
        {
            if(await EnterPost(profile, post) == null)
            {
                return false;
            }

            if (IsPrivate())
            {
                return false;
            }

            var textArea = driver.FindElements(By.CssSelector("form > textarea"), 60).FirstOrDefault();

            if (textArea == null)
            {
                return false;
            }

            textArea.Click();

            textArea = driver.FindElement(By.CssSelector("form > textarea"), 60);

            await Executor.Exec(() => textArea.SendKeys(comment));

            await Executor.Exec(textArea.Submit);

            return true;
        }
        private async Task<KeyValuePair<string, IWebElement>> getPost(int number)
        {
            var posts = new Dictionary<string, IWebElement>();
            foreach (var post in getPosts())
            {
                posts.TryAdd(post.Key, post.Value);
            }

            while (number > posts.Count())
            {
                await ScrollDown.Scroll(driver.FindElement(By.TagName("body"), 60));

                foreach (var post in getPosts())
                {
                    posts.TryAdd(post.Key, post.Value);
                }
            }
            var foundPost = posts.ElementAt(number - 1);

            return new KeyValuePair<string, IWebElement>(foundPost.Key, foundPost.Value);
        }
        private IDictionary<string, IWebElement> getPosts()
        {
            var dictionary = new Dictionary<string, IWebElement>();
            var posts = driver.FindElements(By.CssSelector("article > div:first-of-type > div:first-of-type > div > div"), 60);

            foreach (var post in posts)
            {
                var href = post.FindElement(By.TagName("a")).GetAttribute("href");
                dictionary.TryAdd(href, post);
            }
            return dictionary;
        }



    }

}
