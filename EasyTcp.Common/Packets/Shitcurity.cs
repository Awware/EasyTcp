using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EasyTcp.Common.Packets
{
    public static class Shitcurity
    {
        public static string Password = "";
        public static byte[] Salt = new byte[0];
        public static Packet EncryptPacket(string name, byte[] data)
        {
            return new Packet(Encrypt(data), Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(name))));
        }
        public static Packet EncryptPacket(Packet packet) => EncryptPacket(packet.PacketType, packet.RawData);
        public static Packet DecryptPacket(Packet packet)
        {
            return new Packet(Decrypt(packet.RawData), Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(packet.PacketType))));
        }
        public static Packet EncryptAndCompress(string name, byte[] data)
        {
            Packet pack = EncryptPacket(name, BytesCompress.Compress(data));
            return pack;
        }
        public static Packet DecryptAndDecompress(Packet packet)
        {
            Packet pack = DecryptPacket(packet);
            pack.RawData = BytesCompress.Decompress(pack.RawData);
            return pack;
        }
        #region AES
        /// <summary>
        /// AES
        /// </summary>
        /// <param name="data">Data for encryption</param>
        /// <returns></returns>
        private static byte[] Encrypt(byte[] data)
        {
            try
            {
                byte[] encryped = null;
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes bdp = new Rfc2898DeriveBytes(Password, Salt);
                    encryptor.Key = bdp.GetBytes(32);
                    encryptor.IV = bdp.GetBytes(16);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(data, 0, data.Length);
                            cs.Close();
                        }
                        encryped = ms.ToArray();
                    }
                }
                return encryped;
            }
            catch { return null; }
        }
        private static byte[] Decrypt(byte[] data)
        {
            try
            {
                byte[] Decrypted = null;
                using (Aes decryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes bdp = new Rfc2898DeriveBytes(Password, Salt);
                    decryptor.Key = bdp.GetBytes(32);
                    decryptor.IV = bdp.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(data, 0, data.Length);
                            cs.Close();
                        }
                        Decrypted = ms.ToArray();
                    }
                }
                return Decrypted;
            }
            catch { return null; }
        }
        #endregion
    }
}
