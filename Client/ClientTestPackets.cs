using EasyTcp.Common;
using EasyTcp.Common.Packets;
using System;
using System.Collections.Generic;

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
            msg.Reply(new Packet(BytesTransformation.TransformIt(r.Next(1, 9999)), "Some packet"));
        }
    }
    public class DataPacket : IClientPacket
    {
        public string PacketType => "Data";

        public void Execute(Message msg, EasyTcpClient client)
        {
            Packet pack = msg.GetPacket;
            List<object> ObjectList = BytesTransformation.TransformToObject(pack.RawData, typeof(string), typeof(byte[]));
            string name = (string)ObjectList[0];
            byte[] file = BytesCompress.Decompress((byte[])ObjectList[1]);
            System.IO.File.WriteAllBytes($"Test_{name}.txt", file);
        }
    }
}
