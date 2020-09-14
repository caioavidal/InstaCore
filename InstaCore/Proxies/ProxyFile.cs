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
        public static void Create(string host, int port,string username, string password) 
        {
            var script = File.ReadAllText("data/background.js");

            script = script.Replace("{host}", host);
            script = script.Replace("{port}", port.ToString());
            script = script.Replace("{username}", username);
            script = script.Replace("{password}", password);

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
