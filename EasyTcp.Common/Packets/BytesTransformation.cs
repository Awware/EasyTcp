using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTcp.Common.Packets
{
    public static class BytesTransformation
    {
        public static byte[] TransformIt(params object[] objects)
        {
            using (BinaryBuffer buffer = new BinaryBuffer())
            {
                buffer.BeginWrite();
                foreach (var obj in objects)
                {
                    switch (obj)
                    {
                        case string @string:
                            buffer.WriteField(@string);
                            break;
                        case int @int:
                            buffer.Write(@int);
                            break;
                        case double @double:
                            buffer.Write(@double);
                            break;
                        case float @float:
                            buffer.Write(@float);
                            break;
                        case uint @uint:
                            buffer.Write(@uint);
                            break;
                        case long @long:
                            buffer.Write(@long);
                            break;
                        case char @char:
                            buffer.Write(@char);
                            break;
                        case bool @bool:
                            buffer.Write(@bool);
                            break;
                        case byte[] bytes:
                            buffer.WriteBytes(bytes);
                            break;
                        case byte @byte:
                            buffer.Write(@byte);
                            break;
                        default:
                            throw new Exception($"Unknown type [{obj.ToString()}]");
                    }
                }
                buffer.EndWrite();
                return buffer.ByteBuffer;
            }
        }
        public static List<object> TransformToObject(byte[] RawBytes, params object[] types)
        {
            List<object> Objects = new List<object>();
            using (BinaryBuffer buffer = new BinaryBuffer(RawBytes))
            {
                buffer.BeginRead();
                foreach (var type in types)
                {
                    if (type.Equals(typeof(string)))
                        Objects.Add(buffer.ReadStringField());
                    else if (type.Equals(typeof(int)))
                        Objects.Add(buffer.ReadInt());
                    else if (type.Equals(typeof(double)))
                        Objects.Add(buffer.ReadDouble());
                    else if (type.Equals(typeof(float)))
                        Objects.Add(buffer.ReadFloat());
                    else if (type.Equals(typeof(uint)) || type.Equals(typeof(long)))
                        Objects.Add(buffer.ReadLong());
                    else if (type.Equals(typeof(char)))
                        Objects.Add(buffer.ReadChar());
                    else if (type.Equals(typeof(bool)))
                        Objects.Add(buffer.ReadBool());
                    else if (type.Equals(typeof(byte[])))
                        Objects.Add(buffer.ReadByteArray());
                    else if (type.Equals(typeof(byte)))
                        Objects.Add(buffer.ReadByte());
                    else
                        throw new Exception($"Unknown type [{type.ToString()}]");
                }
                buffer.EndRead();
            }
            return Objects;
        }
      
    }
}
