using EasyTcp.Common;
using EasyTcp.Common.Packets;
using EasyTcp.Server;
using System;
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
            BinaryBuffer readBuffer = new BinaryBuffer(pack.RawData);
            readBuffer.BeginRead();
            string readed = readBuffer.ReadStringField();
            readBuffer.EndRead();
            Console.WriteLine($"Some packet executed. | {readed}");
            BinaryBuffer bin = new BinaryBuffer();
            bin.BeginWrite();
            bin.WriteField("AnyData.txt");
            bin.WriteFileBytes(File.ReadAllBytes("AnyData.txt"));
            bin.EndWrite();
            msg.Reply(new Packet(bin.ByteBuffer, "Data"));
            //msg.Reply(new Packet(BitConverter.GetBytes(r.Next(1, 9999)), "Some packet"));
        }
    }
}
