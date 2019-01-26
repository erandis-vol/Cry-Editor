using GBAHL;
using GBAHL.Text.Pokemon;
using System;
using System.Collections.Generic;
using System.IO;
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
        private RomFileInfo romFile = null;
        private IniFile romInfo = null;
        private int lastSearch = 0x720000;

        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            // Load ROMs info at startup
            try
            {
                romInfo = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ROMs.ini"));
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(
                    "Unable to locate \"ROMs.ini\".",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Application.Exit();
            }

            // on success, prepare
            ClearCry();

            base.OnLoad(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            cryImage?.Dispose();
            cryImage = null;

            base.OnFormClosed(e);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog { Title = "Open Game", Filter = "GBA ROMs|*.gba" })
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                RomFileInfo tempFileInfo = new RomFileInfo(dialog.FileName);
                string tempCode = tempFileInfo.Code;

                int nameTableOffset;
                string[] names;
                string encoding;

                // Grab ROM information from settings
                try
                {
                    pokemonCount    = romInfo.GetInt32(tempCode, "NumberOfPokemon");
                    cryTable        = romInfo.GetInt32(tempCode, "CryData");
                    growlTable      = romInfo.GetInt32(tempCode, "GrowlData");
                    hoennCryOrder   = romInfo.GetInt32(tempCode, "HoennCryOrder");
                    nameTableOffset = romInfo.GetInt32(tempCode, "PokemonNames");
                    encoding        = romInfo.GetString(tempCode, "TextTable");
                }
                catch (KeyNotFoundException ke)
                {
                    // Uh-oh! The key could not be found.
                    MessageBox.Show(ke.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Perform initial loading of Pokemon names
                try
                {
                    using (var rr = new RomReader(tempFileInfo.OpenRead()))
                    {
                        rr.Seek(nameTableOffset);

                        // TODO: Support more encodings.
                        if (encoding == "jpn")
                        {
                            names = rr.ReadTextTable(11, pokemonCount, FireRedEncoding.Japanese);
                        }
                        else
                        {
                            names = rr.ReadTextTable(11, pokemonCount, FireRedEncoding.International);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show(
                        "There was an error loading the Pokémon names.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                listPokemon.Items.Clear();
                listPokemon.Items.AddRange(names);

                // Display ROM info
                lROM.Text = $"Name: {tempFileInfo.Title}{Environment.NewLine}Code: {tempFileInfo.Code}{Environment.NewLine}Cry Table: 0x{cryTable:X6}{Environment.NewLine}Number of Pokémon: {pokemonCount}";

                // Assuming everything loaded correctly, we are good to go
                romFile = tempFileInfo;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cry.IsValid)
            {
                if (SaveCry())
                {
                    ReloadCry();
                    DisplayCry();
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            romFile = null;
            listPokemon.Items.Clear();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cry.IsValid)
            {
                PlayCry();
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!cry.IsValid)
                return;

            using (var dialog = new OpenFileDialog { Title = "Import Cry", Filter = "Wave Files|*.wav" })
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                CryImportResult result = ImportCry(dialog.FileName);
                switch (result)
                {
                    case CryImportResult.Error:
                        MessageBox.Show(
                            "The WAVE file could not be loaded.",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                        return;

                    case CryImportResult.ErrorMissingFmt:
                        MessageBox.Show(
                            $"Expected the fmt chunk.{Environment.NewLine}The WAVE file is likely invalid.",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                        return;

                    case CryImportResult.ErrorMissingData:
                        MessageBox.Show(
                            $"Expected the data chunk.{Environment.NewLine}The WAVE file is likely invalid.",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                        return;

                    case CryImportResult.ErrorUnsupportedFormat:
                        MessageBox.Show(
                            "The WAVE file has an unsupported format.",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                        return;

                    case CryImportResult.ErrorMultipleChannels:
                        MessageBox.Show(
                            $"The WAVE file contains multiple channels.{Environment.NewLine}Ensure that imported cries only contain one channel.",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                        return;

                    case CryImportResult.ErrorNot8Bits:
                        MessageBox.Show(
                            "The WAVE file does not contain 8 bits per sample.",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                        return;

                    case CryImportResult.WarningAdjustedSampleRate:
                        MessageBox.Show(
                            "The cry was imported successfully, but the sample rate was adjusted for the GBA.",
                            "Warning",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        goto default;

                    default:
                        DisplayCry();
                        gCry.Text = "Cry*";
                        break;
                }
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cry.IsValid)
            {
                using (var dialog = new SaveFileDialog { Title = "Export Cry", Filter = "Wave Files|*.wav" })
                {
                    if (dialog.ShowDialog() != DialogResult.OK)
                        return;

                    ExportCry(dialog.FileName);
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new AboutDialog())
            {
                dialog.ShowDialog();
            }
        }

        private void listPokemon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (romFile == null)
                return;

            try
            {
                // Get pokemon index
                int pokemonIndex = listPokemon.SelectedIndex;

                // Determine cry index and load the cry
                using (var reader = new RomReader(romFile.OpenRead()))
                {
                    var tableIndex = GetCryIndex(pokemonIndex, reader);
                    if (tableIndex == -1)
                    {
                        ClearCry();
                        return;
                    }

                    LoadCry(tableIndex, reader);
                }

                // cry loaded, output
                DisplayCry();
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine("ERROR: {0} {1}", ex.Message, ex.StackTrace);
#endif

                ClearCry();
            }
        }

        private void chkCompressed_CheckedChanged(object sender, EventArgs e)
        {
            if (cry.IsValid)
            {
                cry.IsCompressed = chkCompressed.Checked;
            }
        }
    }
}
