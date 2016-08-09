using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crying
{
    // http://www.romhacking.net/documents/[462]sappy.txt
    // http://www.pokecommunity.com/showthread.php?t=321868

    // cry table format:
    // 12 bytes entries
    // 4 (20 3c 00 00) changing to 00 3c 00 00 allows higher quality??
    // 4 pointer to cry sample
    // 4 sample shape (ff 00 ff 00 preferred)

    // sample format:
    // 16 byte header followed by variable PCM data
    // 1 (00)
    // 1 (00 = uncompressed, 01 = compressed)
    // 1 (00)
    // 1 (00 = unlooped, 40 = looped)
    // 4 pitch adjustment = sample rate * 1024
    // 4 loop relative to start (not allowed for cries!)
    // 4 size of sample

    public partial class MainForm : Form
    {
        ROM rom;
        Settings roms;

        int pokemonCount;
        int cryTable;
        int growlTable;
        int hoennCryOrder;

        Cry cry = new Cry();
        Bitmap cryImage;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // load ROMs info at startup
            try
            {
                roms = Settings.FromFile("ROMs.ini", "ini");
            }
            catch
            {
                MessageBox.Show("Unable to load ROMs.ini!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
        
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            rom?.Dispose();
            cryImage?.Dispose();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Open ROM";
            openFileDialog1.Filter = "GBA ROMs|*.gba";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // open the ROM file
                var temp = new ROM(openFileDialog1.FileName);

                // check for a valid ROM code
                if (!roms.ContainsSection(temp.Code))
                {
                    MessageBox.Show($"ROM type {temp.Code} is not supported!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    temp.Dispose();
                    return;
                }

                // copy temp to rom, closing old one first
                rom?.Dispose();
                rom = temp;

                // get some basic info
                pokemonCount = roms.GetInt32(rom.Code, "NumberOfPokemon", 10);
                cryTable = roms.GetInt32(rom.Code, "CryData", 16);
                growlTable = roms.GetInt32(rom.Code, "GrowlData", 16);
                hoennCryOrder = roms.GetInt32(rom.Code, "HoennCryOrder", 16);

                // valid ROM opened, load all necessary data
                {
                    // load Pokemon names
                    var language = (roms.GetString(rom.Code, "Language") == "jap" ?
                        CharacterEncoding.Japanese : CharacterEncoding.English);

                    var firstPokemonName = roms.GetInt32(rom.Code, "PokemonNames", 16);
                    rom.Seek(firstPokemonName);

                    listPokemon.Items.Clear();
                    listPokemon.Items.AddRange(rom.ReadTextTable(11, pokemonCount, language));
                }

                // display ROM info
                lROM.Text = $"Name: {rom.Name}\nCode: {rom.Code}\nCry Table: 0x{cryTable:X6}\nNumber of Pokémon: {pokemonCount}";
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rom == null || cry.Offset == 0) return;

            if (SaveCry())
                rom.Save();

            DisplayCry();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rom?.Dispose();
            rom = null;

            listPokemon.Items.Clear();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cry.Offset == 0) return;

            PlayCry();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cry.Offset == 0) return;

            openFileDialog1.Title = "Import Cry";
            openFileDialog1.Filter = "Wave Files|*.wav";

            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

            // TODO: support more formats
            try
            {
                ImportCry(openFileDialog1.FileName);
                DisplayCry();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"error: {ex.Message}");
                //ClearCry();
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cry.Offset == 0) return;

            saveFileDialog1.Title = "Export Cry";
            saveFileDialog1.Filter = "Wave Files|*.wav";

            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;

            // TODO: support more formats
            ExportCry(saveFileDialog1.FileName);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Cry Editor v1.0 (8/8/2016)\nCopyright (c) 2016 Lost" +
                "\n\nA program to edit cries in 3rd generation Pokémon games.",
                "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void listPokemon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rom == null || rom.FilePath == string.Empty) return;

            // get pokemon index
            int pokemonIndex = listPokemon.SelectedIndex;

            // get cry index
            int tableIndex = GetCryIndex(pokemonIndex);
            if (tableIndex == -1)
            {
                ClearCry();
                return;
            }

            // load cry at index
            LoadCry(tableIndex);

            // cry loaded, output
            DisplayCry();
        }

        private void chkCompressed_CheckedChanged(object sender, EventArgs e)
        {
            if (cry.Offset == 0) return;

            cry.Compressed = chkCompressed.Checked;
        }

        int GetCryIndex(int pokemonIndex)
        {
            if (pokemonIndex == 0)
                return -1;

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
                return -1;
            }

            // pokemon beyond Chimecho skip the ???/Unown slots
            if (pokemonIndex > 411)
            {
                //tableIndex += 24;
            }

            return tableIndex;
        }

        bool LoadCry(int index)
        {
            if (rom == null)
                return false;

            // load cry table entry
            rom.Seek(cryTable + index * 12);
            var someValue = rom.ReadInt32();
            var cryOffset = rom.ReadPointer();
            var cryShape = rom.ReadInt32();

            if (cryOffset == 0)
                return false;

            // load cry data
            rom.Seek(cryOffset);

            cry.Offset = cryOffset;
            cry.Index = index;
            cry.Compressed = rom.ReadUInt16() == 0x0001;
            cry.Looped = rom.ReadUInt16() == 0x4000;
            cry.SampleRate = rom.ReadInt32() >> 10;
            cry.LoopStart = rom.ReadInt32();
            cry.Size = rom.ReadInt32() + 1;

            if (!cry.Compressed)
            {
                // uncompressed, 1 sample per 1 byte of size
                cry.Data = new sbyte[cry.Size];
                for (int i = 0; i < cry.Size; i++)
                    cry.Data[i] = rom.ReadSByte();
            }
            else
            {
                // compressed, a bit of a hassle
                var lookup = new sbyte[] { 0, 1, 4, 9, 16, 25, 36, 49, -64, -49, -36, -25, -16, -9, -4, -1 };
                var start = rom.Position;

                int alignment = 0, size = 0;
                sbyte pcmLevel = 0;

                var data = new List<sbyte>();
                for (;;)
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
                    if (size >= cry.Size) break;

                    alignment--;
                }

                cry.Data = data.ToArray();
                cry.Size = rom.Position - start; // bytes needed to recompress
            }
            return true;
        }

        bool SaveCry()
        {
            if (rom == null || cry.Offset == 0) return false;
            //var lookup = new byte[] { 0x0, 0x1, 0x4, 0x9, 0x10, 0x19, 0x24, 0x31, 0xC0, 0xCF, 0xDC, 0xE7, 0xF0, 0xF7, 0xFC, 0xFF };
            var lookup = new sbyte[] { 0, 1, 4, 9, 16, 25, 36, 49, -64, -49, -36, -25, -16, -9, -4, -1 };

            // copy cry data to be written
            var data = new List<byte>();
            if (cry.Compressed)
            {
                // data is compressed in blocks of 1 + 0x20 bytes at a time
                // first byte is normal signed PCM data
                // following 0x20 bytes are compressed based on previous value
                // (for a value not in lookup table, closest value will be chosen instead)
                //Console.WriteLine("compressed");

                // each block has 0x40 samples
                var blockCount = cry.Data.Length / 0x40;
                if (cry.Data.Length % 0x40 > 0) blockCount++;

                // truncates the length of the last block
                // so we don't waste space
                var lastBlockSize = cry.Data.Length - cry.Data.Length / 0x40 * 0x40;
                if (lastBlockSize == 0)
                    lastBlockSize = 0x21;
                else
                    lastBlockSize = 1 + (lastBlockSize / 2);

                var blocks = new byte[blockCount][];
                for (int n = 0; n < blockCount; n++)
                {
                    // create new block
                    if (n < blockCount - 1)
                        blocks[n] = new byte[0x21];
                    else
                        blocks[n] = new byte[lastBlockSize];

                    int i = n * 0x40;
                    int k = 0;

                    // set first value
                    blocks[n][k++] = (byte)cry.Data[i];
                    sbyte pcm = cry.Data[i++];

                    for (int j = 1; j < 0x40 && i < cry.Data.Length; j++)
                    {
                        // get current sample
                        var sample = cry.Data[i++];

                        // difference between previous sample and this
                        var diff = sample - pcm;

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

                        // search for the closest match in the table
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

                for (int n = 0; n < blockCount; n++)
                    data.AddRange(blocks[n]);
            }
            else
            {
                // uncompressed, copy directly to data
                //Console.WriteLine("uncompressed");
                foreach (var s in cry.Data)
                    data.Add((byte)s);
            }

            // determine if cry requires repointing
            if (cry.Size < data.Count)
            {
                // find a new offset for our cry
                using (var fsf = new FreeSpaceDialog(rom, "The cry needs to be repointed.", data.Count))
                {
                    if (fsf.ShowDialog() != DialogResult.OK)
                        return false;

                    // overwrite old cry with FF bytes
                    rom.Seek(cry.Offset);
                    for (int i = 0; i < 16; i++)        // header
                        rom.WriteByte(byte.MaxValue);
                    for (int i = 0; i < cry.Size; i++)  // cry data
                        rom.WriteByte(byte.MaxValue);

                    // set new cry offset
                    cry.Offset = fsf.Offset;
                }
            }

            // write cry
            rom.Seek(cry.Offset);
            rom.WriteUInt16((ushort)(cry.Compressed ? 1 : 0));
            rom.WriteUInt16((ushort)(cry.Looped ? 0x4000 : 0));
            rom.WriteInt32(cry.SampleRate << 10);
            rom.WriteInt32(cry.LoopStart);
            rom.WriteInt32(cry.Data.Length - 1);
            rom.WriteBytes(data.ToArray());

            // write cry table entry
            rom.Seek(cryTable + cry.Index * 12);
            rom.WriteUInt32(cry.Compressed ? 0x00003C20u : 0x00003C00u);
            rom.WritePointer(cry.Offset);
            rom.WriteUInt32(0x00FF00FFu);

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

        void ImportCry(string filename)
        {
            if (cry.Offset == 0) return;

            // load a wave file
            using (var reader = new BinaryReader(File.OpenRead(filename)))
            {
                // read RIFF header
                if (reader.ReadUInt32() != 0x46464952)
                    throw new Exception("This is not a WAVE file!");
                if (reader.ReadInt32() + 8 != reader.BaseStream.Length)
                    throw new Exception("Invalid file length!");
                if (reader.ReadUInt32() != 0x45564157)
                    throw new Exception("This is not a WAVE file!");

                // read fmt chunk
                if (reader.ReadUInt32() != 0x20746D66)
                    throw new Exception("Expected fmt chunk!");
                if (reader.ReadInt32() != 16)
                    throw new Exception("Invalid fmt chunk!");
                if (reader.ReadInt16() != 1)          // only PCM format allowed
                    throw new Exception("Cry must be in PCM format!");
                if (reader.ReadInt16() != 1)          // only 1 channel allowed
                    throw new Exception("Cry cannot have more than one channel!");
                cry.SampleRate = reader.ReadInt32();
                if (reader.ReadInt32() != cry.SampleRate)
                    throw new Exception("Invalid fmt chunk!");
                reader.ReadUInt16();
                var bitsPerSample = reader.ReadUInt16();
                if (bitsPerSample != 8)              // for now, only 8 bit PCM data
                    throw new Exception($"Cries must be 8-bit WAVE files! Got {bitsPerSample}-bit instead.");

                // data chunk
                if (reader.ReadUInt32() != 0x61746164)
                    throw new Exception("Expected data chunk!!");
                var dataSize = reader.ReadInt32();

                cry.Data = new sbyte[dataSize];
                for (int i = 0; i < dataSize; i++)  // read 8-bit unsigned PCM and convert to GBA signed form
                    cry.Data[i] = (sbyte)(reader.ReadByte() - 128);
            }

            // resetting some other properties just in case
            cry.Looped = false;
            cry.LoopStart = 0;
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

            chkCompressed.Checked = cry.Compressed;
            chkLooped.Checked = cry.Looped;

            // draw the cry
            cryImage?.Dispose();
            cryImage = new Bitmap(cry.Data.Length, 128);

            using (var g = Graphics.FromImage(cryImage))
            {
                for (int i = 1; i < cry.Data.Length; i++)
                {
                    g.DrawLine(Pens.Green, i - 1, 64 + cry.Data[i - 1], i, 64 + cry.Data[i]);
                }
            }

            pSample.Image = cryImage;
        }

        void ClearCry()
        {
            lTable.Text = $"Table: 0x{0:X6}";
            lOffset.Text = $"Offset: 0x{0:X6}";
            lSampleRate.Text = "Sample Rate: 0 Hz";
            lSize.Text = "Size: 0 samples";

            chkCompressed.Checked = false;
            chkLooped.Checked = false;

            cryImage?.Dispose();
            cryImage = new Bitmap(1, 1);
            pSample.Image = cryImage;
        }
    }
}
