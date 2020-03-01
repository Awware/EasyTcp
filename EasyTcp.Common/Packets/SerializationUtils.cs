using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace EasyTcp.Common.Packets
{
    public static class SerializationUtils
    {
        public static T FromBytes<T>(byte[] bytes)
        {
            using (MemoryStream mem = new MemoryStream(bytes))
            {
                BinaryFormatter bin = new BinaryFormatter();
                return (T)bin.Deserialize(mem);
            }
        }
        public static byte[] ToBytes(object obj)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(mem, obj);
                return mem.ToArray();
            }
        }
    }
}
