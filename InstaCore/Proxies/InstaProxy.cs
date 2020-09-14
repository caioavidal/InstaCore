using System;
using System.Collections.Generic;
using System.Text;

namespace InstaCore.Proxies
{
    public class InstaProxy
    {
        /// <summary>
        /// Proxy to access Instagram
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public InstaProxy(string host, int port, string username, string password)
        {
            Host = host;
            Port = port;
            Username = username;
            Password = password;
        }

        public string Host { get;  }
        public int Port { get;  }
        public string Username { get; }
        public string Password { get; }
    }
}
