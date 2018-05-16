using System;
using System.Net.NetworkInformation;

namespace Extenso
{
    public static class UriExtensions
    {
        public static IPStatus Ping(this Uri uri, int timeout = 3000)
        {
            return new Ping().Send(uri.Host, timeout).Status;
        }
    }
}