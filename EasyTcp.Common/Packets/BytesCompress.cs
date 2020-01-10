using System.IO;
using System.IO.Compression;

namespace EasyTcp.Common.Packets
{
    public static class BytesCompress
    {
        /// <summary>
        /// Compress data, make sure you check the size. the stream might be compressed already.
        /// </summary>
        /// <param name="raw"></param>
        /// <param name="data"></param>
        public static void Compress(ref byte[] raw, out byte[] data)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory,
                CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                data = memory.ToArray();
            }
        }

        public static void Decompress(ref byte[] gzip, out byte[] data)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    data = memory.ToArray();
                }
            }
        }
    }
}
