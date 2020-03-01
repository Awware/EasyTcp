using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
<<<<<<< Updated upstream
using System.Runtime.Serialization.Formatters.Soap;
=======
using System.Runtime.Serialization.Formatters.Binary;
>>>>>>> Stashed changes
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
                SoapFormatter Soap = new SoapFormatter();
                return (T)Soap.Deserialize(mem);
            }
        }
        public static byte[] ToBytes(object obj)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                SoapFormatter Soap = new SoapFormatter();
                Soap.Serialize(mem, obj);
                return mem.ToArray();
            }
        }
    }
}
