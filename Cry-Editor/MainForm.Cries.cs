using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace Crying
{
    partial class MainForm
    {
        Cry cry = new Cry();

        int pokemonCount;
        int cryTable;
        int growlTable;
        int hoennCryOrder;

        Bitmap cryImage;

        Tuple<int, bool> GetCryIndex(int pokemonIndex)
        {
            if (pokemonIndex == 0)
                return new Tuple<int, bool>(-1, false);

            // get table index
            int tableIndex = pokemonIndex - 1;

            // beyond Celebi, load from order table
            if (pokemonIndex >= 277)    // 277 is Treecko
            {
                rom.Seek(hoennCryOrder + (pokemonIndex - 277) * 2);
                tableIndex = rom.ReadUInt16();
            }

            // pokemon between Celebi and Treecko have no cry
            if (pokemonIndex > 251 && pokemonIndex < 277)
            {
                //tableIndex = 387 + (pokemonIndex - 251);
                return new Tuple<int, bool>(-1, false); ;
            }

            // pokemon beyond Chimecho skip the ???/Unown slots
            if (pokemonIndex > 411)
            {
                //tableIndex += 24;
                // TODO: someone needs to tell me if this works
            }

            return new Tuple<int, bool>(tableIndex, true);
        }

        bool LoadCry(int index)
        {
            if (rom == null)
                return false;

            // ------------------------------
            // load cry table entry
            rom.Seek(cryTable + index * 12);
            var someValue = rom.ReadInt32();
            var cryOffset = rom.ReadPointer();
            var cryShape = rom.ReadInt32();

            if (cryOffset == 0)
                return false;

            // ------------------------------
            // load cry data
            rom.Seek(cryOffset);
            var start = rom.Position;

            cry.Offset = cryOffset;
            cry.Index = index;
            cry.Compressed = rom.ReadUInt16() == 0x0001;
            cry.Looped = rom.ReadUInt16() == 0x4000;
            cry.SampleRate = rom.ReadInt32() >> 10;
            cry.LoopStart = rom.ReadInt32();
            var originalSize = rom.ReadInt32() + 1;

            if (!cry.Compressed)
            {
                // ------------------------------
                // uncompressed, 1 sample per 1 byte of size
                cry.Data = new sbyte[originalSize];
                for (int i = 0; i < originalSize; i++)
                    cry.Data[i] = rom.ReadSByte();
            }
            else
            {
                // ------------------------------
                // compressed, a bit of a hassle
                var lookup = new sbyte[] { 0, 1, 4, 9, 16, 25, 36, 49, -64, -49, -36, -25, -16, -9, -4, -1 };

                int alignment = 0, size = 0;
                sbyte pcmLevel = 0;

                var data = new List<sbyte>();
                while (true)
                {
                    if (alignment == 0)
                    {
                        pcmLevel = rom.ReadSByte();
                        data.Add(pcmLevel);

                        alignment = 0x20;
                    }

                    var input = rom.ReadByte();
                    if (alignment < 0x20)
                    {
                        // first nybble
                        pcmLevel += lookup[input >> 4];
                        data.Add(pcmLevel);
                    }

                    // second nybble
                    pcmLevel += lookup[input & 0xF];
                    data.Add(pcmLevel);

                    // exit when currentSize >= cry.Size
                    size += 2;
                    if (size >= originalSize) break;

                    alignment--;
                }

                cry.Data = data.ToArray();
            }

            cry.OriginalSize = rom.Position - start; // total bytes used by the cry originally
            return true;
        }

        bool SaveCry()
        {
            if (rom == null || cry.Offset == 0) return false;

            // copy cry data to be written
            var data = new List<byte>();
            if (cry.Compressed)
            {
                // ------------------------------
                // data is compressed in blocks of 1 + 0x20 bytes at a time
                // first byte is normal signed PCM data
                // following 0x20 bytes are compressed based on previous value

                // compression lookup table
                // { 0x0, 0x1, 0x4, 0x9, 0x10, 0x19, 0x24, 0x31, 0xC0, 0xCF, 0xDC, 0xE7, 0xF0, 0xF7, 0xFC, 0xFF };
                var lookup = new sbyte[] { 0, 1, 4, 9, 16, 25, 36, 49, -64, -49, -36, -25, -16, -9, -4, -1 };

                // ------------------------------
                // calculate number of needed blocks
                var blockCount = cry.Data.Length / 0x40;
                if (cry.Data.Length % 0x40 > 0) blockCount++;

                // ------------------------------
                // calculate the length of the last block to save space
                var lastBlockSize = cry.Data.Length - cry.Data.Length / 0x40 * 0x40;
                if (lastBlockSize == 0)
                    lastBlockSize = 0x21;
                else
                    lastBlockSize = 1 + (lastBlockSize / 2) + (lastBlockSize % 2 == 0 ? 0 : 1);

                // ------------------------------
                // compress all blocks
                var blocks = new byte[blockCount][];
                for (int n = 0; n < blockCount; n++)
                {
                    // ------------------------------
                    // create new block
                    if (n < blockCount - 1)
                        blocks[n] = new byte[0x21];
                    else
                        blocks[n] = new byte[lastBlockSize];

                    int i = n * 0x40;   // position in source
                    int k = 0;          // position in block

                    // ------------------------------
                    // set first sample
                    // upper nybble is skipped
                    blocks[n][k++] = (byte)cry.Data[i];
                    sbyte pcm = cry.Data[i++];

                    // ------------------------------
                    // compress rest of block
                    for (int j = 1; j < 0x40 && i < cry.Data.Length; j++)
                    {
                        // get current sample
                        var sample = cry.Data[i++];

                        // difference between previous sample and this
                        var diff = sample - pcm;

                        // ------------------------------
                        // check for a perfect match in lookup table
                        var lookupI = -1;
                        for (int x = 0; x < 16; x++)
                        {
                            if (lookup[x] == diff)
                            {
                                lookupI = x;
                                break;
                            }
                        }

                        // ------------------------------
                        // search for the closest match in the table (perfect match not found)
                        if (lookupI == -1)
                        {
                            var bestDiff = 255;
                            for (int x = 0; x < 16; x++)
                            {
                                if (Math.Abs(lookup[x] - diff) < bestDiff)
                                {
                                    lookupI = x;
                                    bestDiff = Math.Abs(lookup[x] - diff);
                                }
                            }
                        }

                        // set value in block
                        // on an odd value, increase position in block
                        if (j % 2 == 0)
                            blocks[n][k] |= (byte)(lookupI << 4);
                        else
                            blocks[n][k++] |= (byte)lookupI;

                        // set previous
                        pcm = sample;
                    }
                }

                // ------------------------------
                // copy blocks to output list
                for (int n = 0; n < blockCount; n++)
                    data.AddRange(blocks[n]);
            }
            else
            {
                // ------------------------------
                // uncompressed, copy directly to output
                foreach (var s in cry.Data)
                    data.Add((byte)s);
            }

            // ------------------------------
            // calculate number of bytes needed to save this cry
            // length of data + 16 bytes for the header
            var neededBytes = data.Count + 16;

            // ------------------------------
            // determine if cry requires repointing
            if (chkForceRepoint.Checked || neededBytes > cry.OriginalSize)
            {
                // ------------------------------
                // add a "freespace guard"
                // 00 byte added to a cry that ends with FF
                if (data[data.Count - 1] == 0xFF)
                {
                    data.Add(0x00);
                    neededBytes++;
                }

                // ------------------------------
                // find a new offset for our cry
                using (var fsf = new FreeSpaceDialog(rom, data.Count, lastSearch))
                {
                    if (fsf.ShowDialog() != DialogResult.OK)
                        return false;

                    // ------------------------------
                    // overwrite old cry with FF bytes
                    if (chkClean.Checked)
                    {
                        rom.Seek(cry.Offset);
                        for (int i = 0; i < cry.OriginalSize - 1; i++)
                            rom.WriteByte(byte.MaxValue);
                    }

                    // ------------------------------
                    // set new cry offset
                    cry.Offset = fsf.Offset;

                    // ------------------------------
                    // remember search start
                    lastSearch = fsf.SearchStart;
                }
            }

            // ------------------------------
            // write cry
            rom.Seek(cry.Offset);
            rom.WriteUInt16((ushort)(cry.Compressed ? 1 : 0));  //
            rom.WriteUInt16((ushort)(cry.Looped ? 0x4000 : 0)); //
            rom.WriteInt32(cry.SampleRate << 10);               // 
            rom.WriteInt32(cry.LoopStart);                      // cries should always be 00
            rom.WriteInt32(cry.Data.Length - 1);                // length of cry uncompressed - 1
            rom.WriteBytes(data.ToArray());

            // ------------------------------
            // write cry table entry
            rom.Seek(cryTable + cry.Index * 12);
            rom.WriteUInt32(cry.Compressed ? 0x00003C20u : 0x00003C00u);
            rom.WritePointer(cry.Offset);
            rom.WriteUInt32(0x00FF00FFu);

            // ------------------------------
            // write growl table entry
            rom.Seek(growlTable + cry.Index * 12);
            rom.WriteUInt32(cry.Compressed ? 0x00003C30u : 0x00003C00u); // !!! not sure if 00 should be used for uncompressed
            rom.WritePointer(cry.Offset);
            rom.WriteUInt32(0x00FF00FFu);
            return true;
        }

        void ExportCry(string filename)
        {
            if (cry.Offset == 0) return;

            // http://www-mmsp.ece.mcgill.ca/documents/audioformats/wave/wave.html
            // http://soundfile.sapp.org/doc/WaveFormat/
            using (var writer = new BinaryWriter(File.Create(filename)))
            {
                // RIFF header
                writer.Write(Encoding.ASCII.GetBytes("RIFF"));  // file ID
                writer.Write(0);                                // file size placeholder
                writer.Write(Encoding.ASCII.GetBytes("WAVE"));  // format

                // fmt chunk
                writer.Write(Encoding.ASCII.GetBytes("fmt "));  // chunk ID
                writer.Write(16);                               // chunk size, 16 for PCM
                writer.Write((ushort)1);                        // format: 1 = wave_format_pcm
                writer.Write((ushort)1);                        // channel count
                writer.Write(cry.SampleRate);                   // sample rate
                writer.Write(cry.SampleRate/* * 1 * 8 / 8*/);   // SampleRate * NumChannels * BitsPerSample/8
                writer.Write((ushort)1 /* * 8 / 8*/);           // NumChannels * BitsPerSample/8
                writer.Write((ushort)8);                        // bits per sample

                // data chunk
                writer.Write(Encoding.ASCII.GetBytes("data"));  // chunk ID
                writer.Write(cry.Data.Length);                  // chunk size
                foreach (var sample in cry.Data)
                    writer.Write((byte)(sample + 0x80));        // wave PCM is unsigned unlike GBA PCM which is

                // fix header
                writer.Seek(4, SeekOrigin.Begin);
                writer.Write((int)writer.BaseStream.Length - 8);
            }
        }

        Tuple<int, string> ImportCry(string filename)
        {
            if (this.cry.Offset == 0)
                return new Tuple<int, string>(0, "You haven't loaded a cry!");

            // temporary cry
            //var cry = new Cry();

            // load a wave file
            using (var reader = new BinaryReader(File.OpenRead(filename)))
            {
                // read RIFF header
                if (reader.ReadUInt32() != 0x46464952)
                    return new Tuple<int, string>(0, "This is not a WAV file!");
                if (reader.ReadInt32() + 8 != reader.BaseStream.Length)
                    return new Tuple<int, string>(0, "Invalid file length!");
                if (reader.ReadUInt32() != 0x45564157)
                    return new Tuple<int, string>(0, "This is not a WAV file!");

                // read fmt chunk
                if (reader.ReadUInt32() != 0x20746D66)
                    return new Tuple<int, string>(0, "Expected fmt chunk!");
                if (reader.ReadInt32() != 16)
                    return new Tuple<int, string>(0, "WAV is an unsupported format.\nEnsure your file is an 8-bit PCM WAV file.");
                if (reader.ReadInt16() != 1)          // only PCM format allowed
                    return new Tuple<int, string>(0, "WAV is an unsupported format.\nEnsure your file is an 8-bit PCM WAV file.");
                if (reader.ReadInt16() != 1)          // only 1 channel allowed
                    return new Tuple<int, string>(0, "WAB cannot have more than one channel!");
                var sampleRate = reader.ReadInt32();
                if (reader.ReadInt32() != sampleRate)
                    return new Tuple<int, string>(0, "WAV is an unsupported format.\nEnsure your file is an 8-bit PCM WAV file.");
                reader.ReadUInt16();
                var bitsPerSample = reader.ReadUInt16();
                if (bitsPerSample != 8)              // for now, only 8 bit PCM data
                    return new Tuple<int, string>(0, $"WAV must be 8-bit PCM WAV files!\nGot {bitsPerSample}-bit instead.\nEnsure your file is an 8-bit PCM WAV file.");

                // data chunk
                if (reader.ReadUInt32() != 0x61746164)
                    return new Tuple<int, string>(0, "Expected data chunk, but got something else!\nYour WAV likely is an unsupported format.\nEnsure your file is an 8-bit PCM WAV file.");
                var dataSize = reader.ReadInt32();

                var data = new sbyte[dataSize];
                for (int i = 0; i < dataSize; i++)  // read 8-bit unsigned PCM and convert to GBA signed form
                    data[i] = (sbyte)(reader.ReadByte() - 128);

                // success, copy
                cry.SampleRate = sampleRate;
                cry.Data = data;
            }

            // resetting some other properties just in case
            cry.Looped = false;
            cry.LoopStart = 0;

            // send the user warnings:
            if (cry.SampleRate != ((cry.SampleRate << 10) >> 10))
            {
                cry.SampleRate = (cry.SampleRate << 10) >> 10;
                return new Tuple<int, string>(1, "Your cry was imported correctly, but the sample rate was not suited for the GBA and will be altered a bit.");
            }

            // success, no message needed
            return new Tuple<int, string>(2, string.Empty);
        }

        void PlayCry()
        {
            // TODO: we could do this in another thread :O
            if (cry.Offset == 0) return;

            using (var stream = new MemoryStream())
            {
                // "save" the cry to a memorystream
                using (var writer = new BinaryWriter(stream, Encoding.ASCII, true))
                {
                    // RIFF header
                    writer.Write(Encoding.ASCII.GetBytes("RIFF"));
                    writer.Write(0);
                    writer.Write(Encoding.ASCII.GetBytes("WAVE"));

                    // fmt chunk
                    writer.Write(Encoding.ASCII.GetBytes("fmt "));
                    writer.Write(16);
                    writer.Write((ushort)1);
                    writer.Write((ushort)1);
                    writer.Write(cry.SampleRate);
                    writer.Write(cry.SampleRate);
                    writer.Write((ushort)1);
                    writer.Write((ushort)8);

                    // data chunk
                    writer.Write(Encoding.ASCII.GetBytes("data"));
                    writer.Write(cry.Data.Length);
                    foreach (var sample in cry.Data)
                        writer.Write((byte)(sample + 0x80));

                    // fix header
                    writer.Seek(4, SeekOrigin.Begin);
                    writer.Write((int)writer.BaseStream.Length - 8);
                }

                // play it via a soundplayer
                stream.Seek(0L, SeekOrigin.Begin);
                using (var player = new SoundPlayer(stream))
                {
                    player.Load();
                    player.Play();
                }
            }
        }

        void DisplayCry()
        {
            lTable.Text = $"Table: 0x{(cryTable + cry.Index * 12):X6}";
            lOffset.Text = $"Offset: 0x{cry.Offset:X6}";
            lSampleRate.Text = $"Sample Rate: {cry.SampleRate} Hz";
            lSize.Text = $"Size: {cry.Data.Length} samples";
            gCry.Text = "Cry";

            chkCompressed.Checked = cry.Compressed;
            chkLooped.Checked = cry.Looped;

            // draw the cry
            cryImage?.Dispose();
            cryImage = new Bitmap(cry.Data.Length + 1, 129);

            using (var g = Graphics.FromImage(cryImage))
            {
                g.DrawLine(SystemPens.ControlLight, 0, 0, cry.Data.Length, 0);
                g.DrawLine(SystemPens.ControlLight, 0, 32, cry.Data.Length, 32);
                g.DrawLine(SystemPens.ControlLight, 0, 64, cry.Data.Length, 64);
                g.DrawLine(SystemPens.ControlLight, 0, 96, cry.Data.Length, 96);
                g.DrawLine(SystemPens.ControlLight, 0, 128, cry.Data.Length, 128);

                for (int i = 0; i < cry.Data.Length / 32 + 1; i++)
                {
                    g.DrawLine(SystemPens.ControlLight, i * 32, 0, i * 32, 128);
                }

                for (int i = 1; i < cry.Data.Length; i++)
                {
                    g.DrawLine(Pens.Red, i - 1, 64 + (cry.Data[i - 1] / 2), i, 64 + (cry.Data[i] / 2));
                }
            }

            pSample.Image = cryImage;

            playToolStripMenuItem.Enabled = true;
            importToolStripMenuItem.Enabled = true;
            exportToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
        }

        void ClearCry()
        {
            lTable.Text = $"Table: 0x{0:X6}";
            lOffset.Text = $"Offset: 0x{0:X6}";
            lSampleRate.Text = "Sample Rate: 0 Hz";
            lSize.Text = "Size: 0 samples";
            gCry.Text = "Cry";

            chkCompressed.Checked = false;
            chkLooped.Checked = false;

            cryImage?.Dispose();
            cryImage = new Bitmap(1, 1);
            pSample.Image = cryImage;

            playToolStripMenuItem.Enabled = false;
            importToolStripMenuItem.Enabled = false;
            exportToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
        }
    }
}
