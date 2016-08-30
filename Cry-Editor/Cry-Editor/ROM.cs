using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Crying
{
    public class ROM : IDisposable
    {
        private byte[] buffer;
        private int pos = 0;
        private string filePath;
        private FileSystemWatcher fileWatcher;
        private bool ignoreChange;

        private bool disposed;

        public ROM(string filePath)
        {
            try
            {
                // read contents of file into buffer
                using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                }

                // create file watcher to monitor ROM being modified
                fileWatcher = new FileSystemWatcher();
                fileWatcher.Path = Path.GetDirectoryName(filePath);
                fileWatcher.Filter = "*.gba";

                fileWatcher.Changed += OnSourceFileChanged;
                fileWatcher.Renamed += OnSourceFileRenamed;

                fileWatcher.EnableRaisingEvents = true;
            }
            catch
            {
                throw new Exception($"Unable to open {filePath}!");
            }

            this.filePath = filePath;
        }

        ~ROM()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            fileWatcher.Dispose();
            buffer = null;
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(filePath)) return;

            using (var fs = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            {
                fs.Write(buffer, 0, buffer.Length);
            }

            ignoreChange = true;
        }

        public void Seek(int offset)
        {
            if (offset < 0 || offset > buffer.Length)
            {
                throw new IndexOutOfRangeException();
            }

            pos = offset;
        }

        public void Skip(int bytes)
        {
            pos += bytes;

            if (pos < 0 || pos > buffer.Length)
            {
                throw new IndexOutOfRangeException();
            }
        }

        #region Read

        public byte ReadByte()
        {
            return buffer[pos++];
        }

        public byte PeekByte()
        {
            return buffer[pos];
        }

        public sbyte ReadSByte()
        {
            return (sbyte)ReadByte();
        }

        public ushort ReadUInt16()
        {
            return (ushort)(buffer[pos++] | (buffer[pos++] << 8));
        }

        public int ReadInt32()
        {
            return buffer[pos++] | (buffer[pos++] << 8) | (buffer[pos++] << 16) | (buffer[pos++] << 24);
        }

        public uint ReadUInt32()
        {
            return (uint)(buffer[pos++] | (buffer[pos++] << 8) | (buffer[pos++] << 16) | (buffer[pos++] << 24));
        }

        public ulong ReadUInt64()
        {
            return (ulong)(buffer[pos++] | (buffer[pos++] << 8) | (buffer[pos++] << 16) | (buffer[pos++] << 24) |
                (buffer[pos++] << 32) | (buffer[pos++] << 40) | (buffer[pos++] << 48) | (buffer[pos++] << 56));
        }

        public byte[] ReadBytes(int count)
        {
            var b = new byte[count];
            for (int i = 0; i < count; i++)
            {
                b[i] = buffer[pos++];
            }
            return b;
        }

        // Read a UTF8 encoded string
        public string ReadString(int length)
        {
            return Encoding.UTF8.GetString(ReadBytes(length));
        }

        public int ReadPointer()
        {
            // read value
            var ptr = ReadInt32();

            // return on blank pointer
            if (ptr == 0) return 0;

            // a pointer must be between 0x0 and 0x1FFFFFF to be valid ROM bank 1 pointer
            // bank 1 is 0x08-0x09, so 0x8000000 <= POINTER <= 0x9FFFFFF
            if (ptr < 0x8000000 || ptr > 0x9FFFFFF) throw new Exception(string.Format("Bad pointer at 0x{0:X6}", pos - 4));

            // easy way to extract
            return ptr & 0x1FFFFFF;
        }

        public string ReadText(int length, CharacterEncoding encoding = CharacterEncoding.English)
        {
            return TextTable.GetString(ReadBytes(length), encoding);
        }

        public string[] ReadTextTable(int stringLength, int tableSize, CharacterEncoding encoding = CharacterEncoding.English)
        {
            var table = new string[tableSize];
            for (int i = 0; i < tableSize; i++)
                table[i] = ReadText(stringLength, encoding);

            return table;
        }

        #endregion

        #region Write

        public void WriteByte(byte value)
        {
            buffer[pos++] = value;
        }

        public void WriteSByte(sbyte value)
        {
            WriteByte((byte)value);
        }

        public void WriteUInt16(ushort value)
        {
            buffer[pos++] = (byte)value;
            buffer[pos++] = (byte)(value >> 8);
        }

        public void WriteInt32(int value)
        {
            for (int i = 0; i < 4; i++)
            {
                buffer[pos++] = (byte)(value >> (i * 8));
            }
        }

        public void WriteUInt32(uint value)
        {
            for (int i = 0; i < 4; i++)
            {
                buffer[pos++] = (byte)(value >> (i * 8));
            }
        }

        public void WriteUInt64(ulong value)
        {
            for (int i = 0; i < 8; i++)
            {
                buffer[pos++] = (byte)(value >> (i * 8));
            }
        }

        public void WriteBytes(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++) buffer[pos++] = bytes[i];
        }

        // Write a byte value count number of times
        public void WriteBytes(byte value, int count)
        {
            for (int i = 0; i < count; i++) buffer[pos++] = value;
        }

        public void WritePointer(int offset)
        {
            if (offset > 0x1FFFFFF)
            {
                throw new Exception(string.Format("Offset 0x{0:X6} too large for a ROM pointer (0 <= offset <= 0x1FFFFFF)!"));
            }

            WriteInt32(offset | 0x8000000);
        }

        // Write a UTF8 encoded string
        public void WriteString(string str)
        {
            WriteBytes(Encoding.UTF8.GetBytes(str));
        }

        #endregion

        #region Search

        public int FindFreeSpace(int length, byte freespace = 0xFF, int startOffset = 0, int alignment = 1)
        {
            // The simpest freespace finder I could think of
            if (alignment > 1 && startOffset % alignment != 0)
            {
                startOffset += alignment - (startOffset % alignment);
            }

            for (int i = startOffset; i < buffer.Length - length; i += alignment)
            {
                bool match = true;
                for (int j = 0; j < length; j++)
                {
                    if (buffer[i + j] != freespace)
                    {
                        match = false;
                        break;
                    }
                }

                if (match) return i;
            }
            return -1;
        }

        #endregion

        #region Properties

        public string FilePath
        {
            get { return filePath; }
        }

        public int Length
        {
            get { return buffer.Length; }
        }

        public int Position
        {
            get { return pos; }
            set
            {
                pos = value;

                if (pos < 0 || pos > buffer.Length)
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public bool EndOfStream
        {
            get
            {
                return pos >= buffer.Length;
            }
        }

        // ROM specific properties
        // TODO: better to store these as existing strings

        public string Name
        {
            get
            {
                return Encoding.UTF8.GetString(buffer, 0xA0, 12);
            }
        }

        public string Code
        {
            get
            {
                return Encoding.UTF8.GetString(buffer, 0xAC, 4);
            }
        }

        public string Maker
        {
            get
            {
                return Encoding.UTF8.GetString(buffer, 0xB0, 2);
            }
        }

        #endregion

        void OnSourceFileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath == filePath)
            {
                // ignore the source file changing if we caused it
                if (ignoreChange)
                {
                    ignoreChange = false;
                    return;
                }

                // otherwise reload buffer
                using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    if (buffer.Length != fs.Length)
                        buffer = new byte[fs.Length];

                    fs.Read(buffer, 0, buffer.Length);
                }
            }
        }

        void OnSourceFileRenamed(object sender, RenamedEventArgs e)
        {
            // source ROM file was renamed, track the change
            if (e.OldFullPath == filePath)
            {
                filePath = e.FullPath;
            }
        }
    }
}
