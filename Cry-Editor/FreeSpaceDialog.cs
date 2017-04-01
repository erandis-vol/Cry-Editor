using System;
using System.Windows.Forms;
using GBAHL.IO;

namespace Crying
{
    public partial class FreeSpaceDialog : Form
    {
        ROM rom;

        public FreeSpaceDialog(ROM rom, int neededBytes, int searchStart)
        {
            InitializeComponent();
            this.rom = rom;

            tNeeded.Value = neededBytes;
            tNeeded.Enabled = false;
            tStart.Value = searchStart;
        }

        private void FreeSpaceDialog_Load(object sender, EventArgs e)
        {
            bOK.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var offset = rom.Find(0xFF, tNeeded.Value, tStart.Value);
            if (offset > 0)
            {
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
            get { return tRepointTo.Value; }
        }

        public int SearchStart
        {
            get { return tStart.Value; }
        }
    }
}
