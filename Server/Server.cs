using EasyTcp.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server starting...");
            EasyTcpServer server = new EasyTcpServer(System.Reflection.Assembly.GetExecutingAssembly());
            server.OnServerStarted += (sender, s) =>
            {
                Console.WriteLine("Server started!");
            };
            server.ClientConnected += (sender, client) =>
            {
                Console.WriteLine($"Client [{client.RemoteEndPoint.ToString()}] connected!");
            };
            server.ClientDisconnected += (sender, client) =>
            {
                Console.WriteLine($"Client [{client.RemoteEndPoint.ToString()}] disconnected!");
            };
            server.DataReceived += (sender, msg) =>
            {
                Console.WriteLine($"PacketType: {msg.GetPacket.PacketType}");
                server.PacketHandler(msg);
            };
            server.OnError += (sender, ex) =>
            {
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
            };
            server.Start("127.0.0.1", 6124, 10);
            Task.Delay(-1).Wait();
        }
    }
}
