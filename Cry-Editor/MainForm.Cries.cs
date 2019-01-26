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

            // Load cry
            reader.Seek(cryOffset);

            cry.Offset = cryOffset;
            cry.Index = index;
            cry.IsCompressed = reader.ReadUInt16() == 0x0001;
            cry.Looped = reader.ReadUInt16() == 0x4000;
            cry.SampleRate = reader.ReadInt32() >> 10;
            cry.LoopTo = reader.ReadInt32();
            int originalSize = reader.ReadInt32() + 1;

            if (cry.IsCompressed)
            {
                sbyte[] lookup = Cry.Lookup;

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
            else
            {
                // uncompressed, 1 sample per 1 byte of size
                cry.Data = new sbyte[originalSize];
                for (int i = 0; i < originalSize; i++)
                    cry.Data[i] = reader.ReadSByte();
            }

            // total bytes used by the cry originally
            cry.OriginalSize = reader.Position - cryOffset.Address;
            return true;
        }

        private void SaveCry()
        {
            // Grab the cry's data, optionally compressing it
            sbyte[] data = cry.Data;
            if (cry.IsCompressed)
                data = Cry.Compress(data) ?? throw new NullReferenceException();

            // Calculate number of bytes needed to save this cry
            int neededBytes = 16 + data.Length;

            // Prepare to save the cry
            Ptr originalOffset = cry.Offset;

            // Determine if cry requires repointing
            if (chkForceRepoint.Checked || neededBytes > cry.OriginalSize)
            {
                // Add a "freespace guard"
                // 00 byte added to a cry that ends with FF
                if (data[data.Length - 1] == unchecked((sbyte)0xFF))
                {
                    Array.Resize(ref data, data.Length + 1);
                    neededBytes++;
                }

                // Find a new offset for our cry
                using (var dialog = new FreeSpaceDialog(romFile, neededBytes, lastSearch))
                {
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        MessageBox.Show(
                            "Saving of cry was canceled.",
                            "Canceled",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }

                    // Set new cry offset
                    cry.Offset = (Ptr)dialog.Offset;

                    // Remember search start for next time
                    lastSearch = dialog.SearchStart;
                }
            }

            using (var writer = new RomWriter(romFile.OpenWrite()))
            {
                // Overwrite original cry, if needed
                if (originalOffset != cry.Offset && chkClean.Checked)
                {
                    writer.Seek(originalOffset);
                    for (int i = 0; i < cry.OriginalSize; i++)
                        writer.WriteByte(0xFF);
                }

                // Write cry
                writer.Seek(cry.Offset);
                writer.WriteUInt16((ushort)(cry.IsCompressed ? 1 : 0)); //
                writer.WriteUInt16((ushort)(cry.Looped ? 0x4000 : 0));  //
                writer.WriteInt32(cry.SampleRate << 10);                // 
                writer.WriteInt32(cry.LoopTo);                          // cries should always be 00
                writer.WriteInt32(cry.Data.Length - 1);                 // length of cry uncompressed - 1
                foreach (sbyte s in data)
                    writer.WriteSByte(s);

                // Write cry table entry
                writer.Seek(cryTable + cry.Index * 12);
                writer.WriteUInt32(cry.IsCompressed ? 0x00003C20u : 0x00003C00u);
                writer.WritePtr(cry.Offset);
                writer.WriteUInt32(0x00FF00FFu);

                // Write growl table entry
                writer.Seek(growlTable + cry.Index * 12);
                writer.WriteUInt32(cry.IsCompressed ? 0x00003C30u : 0x00003C00u); // !!! not sure if 00 should be used for uncompressed
                writer.WritePtr(cry.Offset);
                writer.WriteUInt32(0x00FF00FFu);
            }
        }

        private const uint WaveMagicRiff = 0x46464952u;
        private const uint WaveMagicWave = 0x45564157u;
        private const uint WaveMagicFmt_ = 0x20746D66u;
        private const uint WaveMagicData = 0x61746164u;

        private void ExportCry(string filename)
        {
            // http://www-mmsp.ece.mcgill.ca/documents/audioformats/wave/wave.html
            // http://soundfile.sapp.org/doc/WaveFormat/
            using (var writer = new BinaryWriter(File.Create(filename)))
            {
                // RIFF header
                writer.Write(WaveMagicRiff);                    // file ID
                writer.Write(0);                                // file size placeholder
                writer.Write(WaveMagicWave);                    // format

                // fmt chunk
                writer.Write(WaveMagicFmt_);                    // chunk ID
                writer.Write(16);                               // chunk size, 16 for PCM
                writer.Write((ushort)1);                        // format: 1 = wave_format_pcm
                writer.Write((ushort)1);                        // channel count
                writer.Write(cry.SampleRate);                   // sample rate
                writer.Write(cry.SampleRate/* * 1 * 8 / 8*/);   // SampleRate * NumChannels * BitsPerSample/8
                writer.Write((ushort)1 /* * 8 / 8*/);           // NumChannels * BitsPerSample/8
                writer.Write((ushort)8);                        // bits per sample

                // data chunk
                writer.Write(WaveMagicData);                    // chunk ID
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
            using (var reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                // read RIFF header
                if (reader.ReadUInt32() != WaveMagicRiff)
                    return CryImportResult.Error;
                if (reader.ReadInt32() + 8 != reader.BaseStream.Length)
                    return CryImportResult.Error;
                if (reader.ReadUInt32() != WaveMagicWave)
                    return CryImportResult.Error;

                // read fmt chunk
                if (reader.ReadUInt32() != WaveMagicFmt_)
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
                if (reader.ReadUInt32() != WaveMagicData)
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
                    writer.Write(WaveMagicRiff);
                    writer.Write(0);
                    writer.Write(WaveMagicWave);

                    // fmt chunk
                    writer.Write(WaveMagicFmt_);
                    writer.Write(16);
                    writer.Write((ushort)1);
                    writer.Write((ushort)1);
                    writer.Write(cry.SampleRate);
                    writer.Write(cry.SampleRate);
                    writer.Write((ushort)1);
                    writer.Write((ushort)8);

                    // data chunk
                    writer.Write(WaveMagicData);
                    writer.Write(cry.Data.Length);
                    foreach (var sample in cry.Data)
                        writer.Write((byte)(sample + 0x80));

                    // fix header
                    writer.Seek(4, SeekOrigin.Begin);
                    writer.Write((int)writer.BaseStream.Length - 8);
                }

                // play it via a soundplayer
                using (var player = new SoundPlayer(stream))
                {
                    stream.Seek(0L, SeekOrigin.Begin);
                    player.Play();
                }
            }
        }

        private void DisplayCry()
        {
            lTable.Text = $"Table: 0x" + (cryTable + cry.Index * 12).ToString("X7");
            lOffset.Text = $"Offset: 0x" + cry.Offset.ToString("X7");
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
                g.DrawLine(SystemPens.ControlLight, 0,   0, cry.Data.Length,   0);
                g.DrawLine(SystemPens.ControlLight, 0,  32, cry.Data.Length,  32);
                g.DrawLine(SystemPens.ControlLight, 0,  64, cry.Data.Length,  64);
                g.DrawLine(SystemPens.ControlLight, 0,  96, cry.Data.Length,  96);
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
            pSample.Image = cryImage = null;

            playToolStripMenuItem.Enabled = false;
            importToolStripMenuItem.Enabled = false;
            exportToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
        }
    }
}
