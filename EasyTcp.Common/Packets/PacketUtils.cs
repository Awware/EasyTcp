using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace EasyTcp.Common.Packets
{
    public static class PacketUtils
    {
        public static byte[] ToBytes(Packet packet)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream memory = new MemoryStream();
                bf.Serialize(memory, packet);
                byte[] bytes = memory.ToArray();
                memory.Close();
                return bytes;
            }
            catch { return new byte[0]; }
        }
        public static Packet FromBytes(byte[] raw)
        {
            try
            {
                return (Packet)new BinaryFormatter().Deserialize(new MemoryStream(raw));
            }
            catch { return new Packet(null, "ERROR"); }
        }
    }
}
