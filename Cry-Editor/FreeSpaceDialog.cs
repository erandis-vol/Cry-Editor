using System;
using System.IO;
using System.Windows.Forms;
using GBAHL;

namespace Crying
{
    public partial class FreeSpaceDialog : Form
    {
        private RomReader rom;

        public FreeSpaceDialog(RomFileInfo romFile, int neededBytes, int searchStart)
        {
            InitializeComponent();

            try
            {
                rom = new RomReader(romFile.OpenRead());
            }
            catch
            {
                MessageBox.Show(
                    "There was an error opening the ROM.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Close();
                return;
            }

            tNeeded.Value = neededBytes;
            tNeeded.Enabled = false;
            tStart.Value = searchStart;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (rom != null)
            {
                rom.Dispose();
                rom = null;
            }

            base.OnFormClosed(e);
        }

        private void FreeSpaceDialog_Load(object sender, EventArgs e)
        {
            bOK.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var offset = rom.Find(0xFF, tNeeded.Value, tStart.Value, 4);
            if (offset > 0)
            {
                offset += 3;
                offset -= offset % 4;
                bOK.Enabled = true;
                tRepointTo.Value = offset;
            }
            else
            {
                bOK.Enabled = false;
                tRepointTo.Value = 0;
            }
        }

        private void tRepointTo_TextChanged(object sender, EventArgs e)
        {
            bOK.Enabled = tRepointTo.Value > 0;
        }

        public int Offset
        {
            get => tRepointTo.Value;
            set => tRepointTo.Value = value;
        }

        public int SearchStart
        {
            get => tStart.Value;
            set => tStart.Value = value;
        }
    }
}
