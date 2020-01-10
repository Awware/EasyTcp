using System.Net.Sockets;

namespace EasyTcp.Server
{
    internal class ClientObject
    {
        public Socket Socket;

        /// <summary>
        /// Data buffer for incoming data.
        /// </summary>
        public byte[] Buffer;
    }
}
