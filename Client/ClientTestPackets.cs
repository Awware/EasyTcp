using EasyTcp.Common;
using EasyTcp.Common.Packets;
using System;

namespace EasyTcp.Client
{
    public class ClientTestPackets : IClientPacket
    {
        public static Random r = new Random();
        public string PacketType => "Some packet";

        public void Execute(Message msg, EasyTcpClient client)
        {
            Packet pack = msg.GetPacket;
            Console.WriteLine($"Some packet executed. | {BitConverter.ToInt32(pack.RawData, 0)}");
            msg.Reply(new Packet(BitConverter.GetBytes(r.Next(1, 9999)), "Some packet"));
        }
    }
    public class DataPacket : IClientPacket
    {
        public string PacketType => "Data";

        public void Execute(Message msg, EasyTcpClient client)
        {
            Packet pack = msg.GetPacket;
            BinaryBuffer buff = new BinaryBuffer(pack.RawData);
            buff.BeginRead();
            string name = buff.ReadStringField();
            byte[] file = buff.ReadFileBytes();
            System.IO.File.WriteAllBytes($"Test_{name}.txt", file);
        }
    }
}
