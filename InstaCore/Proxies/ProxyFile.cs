using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace InstaCore.Proxies
{
    public class ProxyFile
    {
        public static void Create(InstaProxy proxy) 
        {
            var script = File.ReadAllText("data/background.js");

            script = script.Replace("{host}", proxy.Host);
            script = script.Replace("{port}", proxy.Port.ToString());
            script = script.Replace("{username}", proxy.Username);
            script = script.Replace("{password}", proxy.Password);

            using (FileStream zipToOpen = new FileStream(@"data/proxy.zip", FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry entry = archive.GetEntry("background.js");
                    entry?.Delete();

                    ZipArchiveEntry newEntry = archive.CreateEntry("background.js");

                    using (StreamWriter writer = new StreamWriter(newEntry.Open()))
                    {
                        writer.Write(script);
                    }
                }
            }
        }
    }
}
