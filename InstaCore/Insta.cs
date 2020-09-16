using InstaCore.Proxies;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstaCore
{
    public class Insta
    {
        public RemoteWebDriver Driver { get; private set; }

        public Insta()
        {
            var options = new ChromeOptions();

            CodePagesEncodingProvider.Instance.GetEncoding(437);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Driver = new ChromeDriver(options);
        }
        public Insta(InstaProxy proxy)
        {
            ProxyFile.Create(proxy);

            var options = new ChromeOptions();

            options.AddExtension(@"data/proxy.zip");

            CodePagesEncodingProvider.Instance.GetEncoding(437);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Driver = new ChromeDriver(Environment.CurrentDirectory + @"\data", options);

        }

    }
}
