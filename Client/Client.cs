using EasyTcp.Client;
using EasyTcp.Common;
using EasyTcp.Common.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Client
    {
        static void Main(string[] args)
        {
            EasyTcpClient client = new EasyTcpClient(System.Reflection.Assembly.GetExecutingAssembly());
            client.OnConnected += (sender, tcp) =>
            {
                Console.WriteLine("Client successfuly connected!");
                client.Send(new Packet(BytesTransformation.TransformIt("Test String", "Test String2", 1337), "Some packet"));
            };
            client.OnDisconnect += (sender, tcp) =>
            {
                Console.WriteLine("Client disconnected!");
            };
            client.OnError += (sender, error) =>
            {
                Console.WriteLine($"{error.Message}\n{error.StackTrace}");
            };
            client.DataReceived += (sender, msg) =>
            {
                Console.WriteLine($"PacketType: {msg.GetPacket.PacketType}");
                client.PacketHandler(msg);
            };
            if(!client.Connect("127.0.0.1", 6124, TimeSpan.FromSeconds(15)))
                Console.WriteLine("Connection aborted. Timeout!");
            Console.ReadLine();
            Task.Delay(-1).Wait();
        }
    }
}
