using System.Net;
using System.Net.NetworkInformation;

namespace Extenso.Net
{
    public static class IPAddressExtensions
    {
        public static IPStatus Ping(this IPAddress ipAddress, int timeout = 3000)
        {
            return new Ping().Send(ipAddress, timeout).Status;
        }
    }
}