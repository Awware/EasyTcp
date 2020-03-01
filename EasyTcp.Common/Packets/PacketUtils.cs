using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace EasyTcp.Common.Packets
{
    public static class PacketUtils
    {
        public static byte[] ToBytes(Packet packet)
        {
            return SerializationUtils.ToBytes(packet);
        }
        public static Packet FromBytes(byte[] raw)
        {
            return SerializationUtils.FromBytes<Packet>(raw);
        }
    }
}
