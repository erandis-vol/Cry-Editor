using System;
using System.IO;
using System.Windows.Forms;
using GBAHL.IO;

namespace Crying
{
    public partial class FreeSpaceDialog : Form
    {
        private GbaBinaryStream rom;

        public FreeSpaceDialog(string romFileName, int neededBytes, int searchStart)
        {
            InitializeComponent();
            rom = new GbaBinaryStream(File.OpenRead(romFileName));

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
