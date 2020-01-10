using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text;

namespace EasyTcp.Common.Packets
{
    public class BinaryBuffer
    {
        private const string STR_EOF = "You are at the End of File!";
        private const string NOT_READ = "You are Not Reading from the Buffer!";
        private const string CURR_WRITE = "You are Currenlty Writing to the Buffer!";
        private const string CURR_READ = "You are Currenlty Reading from the Buffer!";
        private const string NOT_WRITE = "You are Not Writing to the Buffer!";
        private const string REVERSE_SEEK = "You are trying to Reverse Seek, Unable to add a Negative value!";
        private bool _inRead;
        private bool _inWrite;
        private List<byte> _newBytes;
        private int _pointer;
        public byte[] ByteBuffer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return Encoding.Default.GetString(ByteBuffer, 0, ByteBuffer.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryBuffer(string data)
            : this(Encoding.Default.GetBytes(data))
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryBuffer()
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryBuffer(byte[] data, bool openRead = false)
            : this(ref data)
        {
            if (openRead)
            {
                this.BeginRead();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryBuffer(ref byte[] data)
        {
            ByteBuffer = data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void IncrementPointer(int add)
        {
            if (add < 0)
            {
                throw new Exception(REVERSE_SEEK);
            }
            _pointer += add;
            if (EofBuffer())
            {
                throw new Exception(STR_EOF);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetPointer()
        {
            return _pointer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetString(ref byte[] buffer)
        {
            return Encoding.Default.GetString(buffer, 0, buffer.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetString(byte[] buffer)
        {
            return GetString(ref buffer);
        }
        #region Writing methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BeginWrite()
        {
            if (_inRead)
            {
                throw new Exception(CURR_READ);
            }
            _inWrite = true;

            _newBytes = new List<byte>();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(float value)
        {
            if (!_inWrite)
            {
                throw new Exception(NOT_WRITE);
            }
            _newBytes.AddRange(BitConverter.GetBytes(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(byte value)
        {
            if (!_inWrite)
            {
                throw new Exception(NOT_WRITE);
            }
            _newBytes.Add(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(int value)
        {
            if (!_inWrite)
            {
                throw new Exception(NOT_WRITE);
            }

            _newBytes.AddRange(BitConverter.GetBytes(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(long value)
        {
            if (!_inWrite)
            {
                throw new Exception(NOT_WRITE);
            }
            byte[] byteArray = new byte[8];

            unsafe
            {
                fixed (byte* bytePointer = byteArray)
                {
                    *((long*)bytePointer) = value;
                }
            }

            _newBytes.AddRange(byteArray);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteField(string value)
        {
            Write(value.Length);
            Write(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Obsolete("Use WriteField if you wanna write string")]
        public void Write(string value)
        {
            if (!_inWrite)
            {
                throw new Exception(NOT_WRITE);
            }
            byte[] byteArray = Encoding.Default.GetBytes(value);
            _newBytes.AddRange(byteArray);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(decimal value)
        {
            if (!_inWrite)
            {
                throw new Exception(NOT_WRITE);
            }
            int[] intArray = decimal.GetBits(value);

            Write(intArray[0]);
            Write(intArray[1]);
            Write(intArray[2]);
            Write(intArray[3]);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(byte[] value)
        {
            Write(ref value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(ref byte[] value)
        {
            if (!_inWrite)
            {
                throw new Exception(NOT_WRITE);
            }
            _newBytes.AddRange(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EndWrite()
        {
            if (ByteBuffer != null)
            {
                _newBytes.InsertRange(0, ByteBuffer);
            }
            ByteBuffer = _newBytes.ToArray();
            _newBytes = null;
            _inWrite = false;
        }
        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetInt(int value, int pos)
        {
            byte[] byteInt = BitConverter.GetBytes(value);
            for (int i = 0; i < byteInt.Length; i++)
            {
                _newBytes[pos + i] = byteInt[i];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetLong(long value, int pos)
        {
            byte[] byteInt = BitConverter.GetBytes(value);
            for (int i = 0; i < byteInt.Length; i++)
            {
                _newBytes[pos + i] = byteInt[i];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int UncommitedLength()
        {
            return _newBytes == null ? 0 : _newBytes.Count;
        }
        #region Reading methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BeginRead()
        {
            if (_inWrite)
            {
                throw new Exception(CURR_WRITE);
            }
            _inRead = true;
            _pointer = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadByte()
        {
            if (!_inRead)
            {
                throw new Exception(NOT_READ);
            }
            if (EofBuffer())
            {
                throw new Exception(STR_EOF);
            }
            return ByteBuffer[_pointer++];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt()
        {
            if (!_inRead)
            {
                throw new Exception(NOT_READ);
            }
            if (EofBuffer(4))
            {
                throw new Exception(STR_EOF);
            }
            int startPointer = _pointer;
            _pointer += 4;

            return BitConverter.ToInt32(ByteBuffer, startPointer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float[] ReadFloatArray()
        {
            float[] dataFloats = new float[ReadInt()];
            for (int i = 0; i < dataFloats.Length; i++)
            {
                dataFloats[i] = ReadFloat();
            }
            return dataFloats;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ReadFloat()
        {
            if (!_inRead)
            {
                throw new Exception(NOT_READ);
            }
            if (EofBuffer(sizeof(float)))
            {
                throw new Exception(STR_EOF);
            }
            int startPointer = _pointer;
            _pointer += sizeof(float);

            return BitConverter.ToSingle(ByteBuffer, startPointer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public decimal ReadDecimal()
        {
            if (!_inRead)
            {
                throw new Exception(NOT_READ);
            }
            if (EofBuffer(16))
            {
                throw new Exception(STR_EOF);
            }
            return new decimal(new[] { ReadInt(),
                ReadInt(),
                ReadInt(),
                ReadInt()
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ReadLong()
        {
            if (!_inRead)
            {
                throw new Exception(NOT_READ);
            }
            if (EofBuffer(8))
            {
                throw new Exception(STR_EOF);
            }
            int startPointer = _pointer;
            _pointer += 8;

            return BitConverter.ToInt64(ByteBuffer, startPointer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ReadString(int size)
        {
            return Encoding.Default.GetString(ReadByteArray(size), 0, size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] ReadByteArray(int size)
        {
            if (!_inRead)
            {
                throw new Exception(NOT_READ);
            }
            if (EofBuffer(size))
            {
                throw new Exception(STR_EOF);
            }
            byte[] newBuffer = new byte[size];

            Array.Copy(ByteBuffer, _pointer, newBuffer, 0, size);

            _pointer += size;

            return newBuffer;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EndRead()
        {
            _inRead = false;
            _pointer = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ReadStringField()
        {
            return ReadString(ReadInt());
        }
        #endregion
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool EofBuffer(int over = 1)
        {
            return ByteBuffer == null || ((_pointer + over) > ByteBuffer.Length);
        }
    }
}
