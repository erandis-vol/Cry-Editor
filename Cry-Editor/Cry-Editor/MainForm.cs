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

            // on success, prepare
            ClearCry();
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
                    var firstPokemonName = roms.GetInt32(rom.Code, "PokemonNames", 16);
                    rom.Seek(firstPokemonName);

                    listPokemon.Items.Clear();
                    switch (roms.GetString(rom.Code, "TextTable"))
                    {
                        case "jap":
                            listPokemon.Items.AddRange(rom.ReadTextTable(6, pokemonCount, CharacterEncoding.Japanese));
                            break;
                        case "eng":
                        default:
                            listPokemon.Items.AddRange(rom.ReadTextTable(11, pokemonCount, CharacterEncoding.English));
                            break;
                    }
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

            openFileDialog1.FileName = "";
            openFileDialog1.Title = "Import Cry";
            openFileDialog1.Filter = "Wave Files|*.wav";

            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

            // TODO: support more formats
            var result = ImportCry(openFileDialog1.FileName);
            if (result.Item1 == 0)      // error
                MessageBox.Show(result.Item2, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (result.Item1 == 1) // warning
                MessageBox.Show(result.Item2, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            if (result.Item1 != 0)
                DisplayCry();
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
            MessageBox.Show("Cry Editor v1.2 (8/9/2016)\nCopyright (c) 2016 Lost" +
                "\n\nA program to edit cries in 3rd generation Pokémon games.",
                "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void listPokemon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rom == null || rom.FilePath == string.Empty) return;

            // get pokemon index
            int pokemonIndex = listPokemon.SelectedIndex;

            // get cry index
            var tableIndex = GetCryIndex(pokemonIndex);
            if (!tableIndex.Item2)
            {
                ClearCry();
                return;
            }

            // load cry at index
            LoadCry(tableIndex.Item1);

            // cry loaded, output
            DisplayCry();
        }

        private void chkCompressed_CheckedChanged(object sender, EventArgs e)
        {
            if (cry.Offset == 0) return;

            cry.Compressed = chkCompressed.Checked;
        }

        
    }
}
