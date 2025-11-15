using System.Net;
using System.Net.NetworkInformation;

namespace Extenso.Net;

/// <summary>
/// Provides a set of static methods for manipulating instances of System.Net.IPAddress.
/// </summary>
public static class IPAddressExtensions
{
    extension(IPAddress source)
    {
        /// <summary>
        ///  Attempts to send an Internet Control Message Protocol (ICMP) echo message to
        ///  the computer that has the specified System.Net.IPAddress, and receive a
        ///  corresponding ICMP echo reply message from that computer. This method allows
        ///  you to specify a time-out value for the operation.
        /// </summary>
        /// <param name="timeout">An System.Int32 value that specifies the maximum number of milliseconds (after sending the echo message) to wait for the ICMP echo reply message.</param>
        /// <returns>A System.Net.NetworkInformation.IPStatus enumeration that reports the status of the ICMP echo sent.</returns>
        public IPStatus Ping(int timeout = 3000)
        {
            using var ping = new Ping();
            return ping.Send(source, timeout).Status;
        }
    }
}