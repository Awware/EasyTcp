using EasyTcp.Common;
using EasyTcp.Common.Packets;
using EasyTcp.Server;
using System;
using System.Collections.Generic;
using System.IO;

namespace Server
{
    public class ServerTestPackets : IServerPacket
    {
        public static Random r = new Random();
        public string PacketType => "Some packet";

        public void Execute(Message msg, EasyTcpServer server)
        {
            Packet pack = msg.GetPacket;
            List<object> f = BytesTransformation.TransformToObject(pack.RawData, typeof(string), typeof(string), typeof(int), typeof(bool), typeof(double), typeof(uint));
            string readed = (string)f[0];
            string readed2 = (string)f[1];
            int readed3 = (int)f[2];
            bool readed4 = (bool)f[3];
            double readed5 = (double)f[4];
            Console.WriteLine($"{f[5].GetType()}");
            Int64 readed6 = (Int64)f[5];
            Console.WriteLine($"Some packet executed. | {readed} | {readed2} | {readed3} | {readed4} | {readed5} | {readed6}");
            msg.Reply(new Packet(BytesTransformation.TransformIt("AnyData.txt", BytesCompress.Compress(File.ReadAllBytes("AnyData.txt"))), "Data"));
            //msg.Reply(new Packet(BitConverter.GetBytes(r.Next(1, 9999)), "Some packet"));
        }
    }
}
