using GBAHL;
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
        private Cry cry = new Cry();

        private int pokemonCount;
        private int cryTable;
        private int growlTable;
        private int hoennCryOrder;

        private Bitmap cryImage;

        private int GetCryIndex(int pokemonIndex, RomReader reader)
        {
            if (pokemonIndex == 0)
                return -1;

            // get table index
            int tableIndex = pokemonIndex - 1;

            // beyond Celebi, load from order table
            if (pokemonIndex >= 277)    // 277 is Treecko
            {
                // TODO: Pull this out.
                using (var gb = new RomReader(romFile.OpenRead()))
                {
                    gb.Seek(hoennCryOrder + (pokemonIndex - 277) * 2);
                    tableIndex = gb.ReadUInt16();
                }
            }

            // pokemon between Celebi and Treecko have no cry
            if (pokemonIndex > 251 && pokemonIndex < 277)
            {
                //tableIndex = 387 + (pokemonIndex - 251);
                return -1;
            }

            // pokemon beyond Chimecho skip the ???/Unown slots
            if (pokemonIndex > 411)
            {
                //tableIndex += 24; // TODO
            }

            return tableIndex;
        }

        private bool ReloadCry()
        {
            if (cry != null && cry.IsValid)
            {
                using (var reader = new RomReader(romFile.OpenRead()))
                {
                    return LoadCry(cry.Index, reader);
                }
            }

            return false;
        }

        private bool LoadCry(int index, RomReader reader)
        {
            // Load cry table entry
            reader.Seek(cryTable + index * 12);
            int someValue = reader.ReadInt32();
            Ptr cryOffset = reader.ReadPtr();
            int cryShape = reader.ReadInt32();

            if (!cryOffset.IsValid || cryOffset.IsZero)
                return false;

            // ------------------------------
            // load cry data
            reader.Seek(cryOffset);

            cry.Offset = cryOffset;
            cry.Index = index;
            cry.IsCompressed = reader.ReadUInt16() == 0x0001;
            cry.Looped = reader.ReadUInt16() == 0x4000;
            cry.SampleRate = reader.ReadInt32() >> 10;
            cry.LoopTo = reader.ReadInt32();
            int originalSize = reader.ReadInt32() + 1;

            if (!cry.IsCompressed)
            {
                // ------------------------------
                // uncompressed, 1 sample per 1 byte of size
                cry.Data = new sbyte[originalSize];
                for (int i = 0; i < originalSize; i++)
                    cry.Data[i] = reader.ReadSByte();
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
                        pcmLevel = reader.ReadSByte();
                        data.Add(pcmLevel);

                        alignment = 0x20;
                    }

                    var input = reader.ReadByte();
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
                    if (size >= originalSize)
                        break;

                    alignment--;
                }

                cry.Data = data.ToArray();
            }

            // total bytes used by the cry originally
            cry.OriginalSize = cry.Size;
            return true;
        }

        private bool SaveCry()
        {
            // copy cry data to be written
            var data = new List<byte>();
            if (cry.IsCompressed)
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
                        pcm = (sbyte)(pcm + lookup[lookupI]);
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
                using (var fsf = new FreeSpaceDialog(romFile, data.Count, lastSearch))
                {
                    if (fsf.ShowDialog() != DialogResult.OK)
                        return false;

                    // ------------------------------
                    // overwrite old cry with FF bytes
                    if (chkClean.Checked)
                    {
                        // TODO: Redo this.
                        //gb.Seek(cry.Offset);
                        //for (int i = 0; i < cry.OriginalSize - 1; i++)
                        //    gb.WriteByte(byte.MaxValue);
                    }

                    // ------------------------------
                    // set new cry offset
                    cry.Offset = (Ptr)fsf.Offset;

                    // ------------------------------
                    // remember search start
                    lastSearch = fsf.SearchStart;
                }
            }

            using (var rw = new RomWriter(romFile.OpenWrite()))
            {
                // ------------------------------
                // write cry
                rw.Seek(cry.Offset);
                rw.WriteUInt16((ushort)(cry.IsCompressed ? 1 : 0));  //
                rw.WriteUInt16((ushort)(cry.Looped ? 0x4000 : 0)); //
                rw.WriteInt32(cry.SampleRate << 10);               // 
                rw.WriteInt32(cry.LoopTo);                      // cries should always be 00
                rw.WriteInt32(cry.Data.Length - 1);                // length of cry uncompressed - 1
                rw.WriteBytes(data.ToArray());

                // ------------------------------
                // write cry table entry
                rw.Seek(cryTable + cry.Index * 12);
                rw.WriteUInt32(cry.IsCompressed ? 0x00003C20u : 0x00003C00u);
                rw.WritePtr(cry.Offset);
                rw.WriteUInt32(0x00FF00FFu);

                // ------------------------------
                // write growl table entry
                rw.Seek(growlTable + cry.Index * 12);
                rw.WriteUInt32(cry.IsCompressed ? 0x00003C30u : 0x00003C00u); // !!! not sure if 00 should be used for uncompressed
                rw.WritePtr(cry.Offset);
                rw.WriteUInt32(0x00FF00FFu);
            }
            return true;
        }

        private void ExportCry(string filename)
        {
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

        private enum CryImportResult
        {
            Success,
            Error,
            ErrorMissingFmt,
            ErrorMissingData,
            ErrorUnsupportedFormat,
            ErrorMultipleChannels,
            ErrorNot8Bits,
            WarningAdjustedSampleRate,
        }

        private CryImportResult ImportCry(string filename)
        {
            //if (cry.Offset == 0)
            //    return new Tuple<int, string>(0, "You haven't loaded a cry!");

            // temporary cry
            //var cry = new Cry();

            // load a wave file
            using (var reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                // read RIFF header
                if (reader.ReadUInt32() != 0x46464952)
                    return CryImportResult.Error;
                if (reader.ReadInt32() + 8 != reader.BaseStream.Length)
                    return CryImportResult.Error;
                if (reader.ReadUInt32() != 0x45564157)
                    return CryImportResult.Error;

                // read fmt chunk
                if (reader.ReadUInt32() != 0x20746D66)
                    return CryImportResult.ErrorMissingFmt;
                if (reader.ReadInt32() != 16)
                    return CryImportResult.ErrorUnsupportedFormat;
                if (reader.ReadInt16() != 1)          // only PCM format allowed
                    return CryImportResult.ErrorUnsupportedFormat;
                if (reader.ReadInt16() != 1)          // only 1 channel allowed
                    return CryImportResult.ErrorMultipleChannels;
                var sampleRate = reader.ReadInt32();
                if (reader.ReadInt32() != sampleRate)
                    return CryImportResult.ErrorUnsupportedFormat;
                reader.ReadUInt16();
                var bitsPerSample = reader.ReadUInt16();
                if (bitsPerSample != 8)              // for now, only 8 bit PCM data
                    return CryImportResult.ErrorNot8Bits;

                // data chunk
                if (reader.ReadUInt32() != 0x61746164)
                    return CryImportResult.ErrorMissingData;
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
            cry.LoopTo = 0;

            // send the user warnings:
            if (cry.SampleRate != ((cry.SampleRate << 10) >> 10))
            {
                cry.SampleRate = (cry.SampleRate << 10) >> 10;
                return CryImportResult.WarningAdjustedSampleRate;
            }

            // success, no message needed
            return CryImportResult.Success;
        }

        private void PlayCry()
        {
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

        private void DisplayCry()
        {
            lTable.Text = $"Table: 0x{(cryTable + cry.Index * 12):X6}";
            lOffset.Text = $"Offset: 0x{cry.Offset:X6}";
            lSampleRate.Text = $"Sample Rate: {cry.SampleRate} Hz";
            lSize.Text = $"Size: {cry.Data.Length} samples";
            gCry.Text = "Cry";

            chkCompressed.Checked = cry.IsCompressed;
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

        private void ClearCry()
        {
            lTable.Text = $"Table: 0x000000";
            lOffset.Text = $"Offset: 0x000000";
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
