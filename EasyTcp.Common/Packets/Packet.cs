using System;

namespace EasyTcp.Common.Packets
{
    [Serializable]
    public class Packet
    {
        public Packet(byte[] Data, string PacketType)
        {
            RawData = Data;
            this.PacketType = PacketType;
        }
        public byte[] RawData { get; }
        public string PacketType { get; }
    }
}
