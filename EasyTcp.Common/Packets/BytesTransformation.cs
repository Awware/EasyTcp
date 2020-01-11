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
                    string objType = obj.GetType().Name;
                    switch (objType)
                    {
                        case "String":
                            buffer.WriteField((string)obj);
                            break;
                        case "Int32":
                            buffer.Write((int)obj);
                            break;
                        case "Double":
                            buffer.Write((double)obj);
                            break;
                        case "Single":
                            buffer.Write((float)obj);
                            break;
                        case "UInt32":
                            buffer.Write((uint)obj);
                            break;
                        case "Char":
                            buffer.Write((char)obj);
                            break;
                        case "Boolean":
                            buffer.Write((bool)obj);
                            break;
                        case "Byte[]":
                            buffer.WriteBytes((byte[])obj);
                            break;
                        case "Byte":
                            buffer.Write((byte)obj);
                            break;
                        default:
                            throw new Exception($"Unknown type [{objType}]");
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
                    switch (type.ToString())
                    {
                        case "System.String":
                            Objects.Add(buffer.ReadStringField());
                            break;
                        case "System.Int32":
                            Objects.Add(buffer.ReadInt());
                            break;
                        case "System.Double":
                            Objects.Add(buffer.ReadDouble());
                            break;
                        case "System.Single":
                            Objects.Add(buffer.ReadFloat());
                            break;
                        case "System.UInt32":
                            Objects.Add(buffer.ReadLong());
                            break;
                        case "System.Char":
                            Objects.Add(buffer.ReadChar());
                            break;
                        case "System.Boolean":
                            Objects.Add(buffer.ReadBool());
                            break;
                        case "System.Byte[]":
                            Objects.Add(buffer.ReadByteArray());
                            break;
                        case "System.Byte":
                            Objects.Add(buffer.ReadByte());
                            break;
                        default:
                            throw new Exception($"Unknown type [{type.ToString()}]");
                    }
                }
                buffer.EndRead();
            }
            return Objects;
        }
    }
}
