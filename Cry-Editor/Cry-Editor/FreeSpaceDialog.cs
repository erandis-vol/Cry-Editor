using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crying
{
    public partial class FreeSpaceDialog : Form
    {
        const int MaximumSearchResults = 8;

        ROM rom;
        int[] offsets = new int[MaximumSearchResults];
        int offset;

        bool ignore;

        public FreeSpaceDialog(ROM rom, string message, int neededBytes)
        {
            InitializeComponent();
            this.rom = rom;

            ignore = true;
            tNeeded.Text = neededBytes.ToString();
            tNeeded.Enabled = false;

            tStart.Text = "0x720000";
            ignore = false;
        }

        private void FreeSpaceDialog_Load(object sender, EventArgs e)
        {
            FindAFewOffsets();

            bOK.Enabled = false;
        }

        private void listOffsets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ignore) return;

            offset = listOffsets.SelectedIndex;
            bOK.Enabled = true;
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            
        }

        private void tNeeded_TextChanged(object sender, EventArgs e)
        {
            if (ignore) return;

            FindAFewOffsets();
        }

        private void tStart_TextChanged(object sender, EventArgs e)
        {
            if (ignore) return;

            FindAFewOffsets();
        }

        void FindAFewOffsets()
        {
            // get search information
            int searchStart = 0;
            int neededBytes = 0;

            try
            {
                neededBytes = Convert.ToInt32(tNeeded.Text);
            }
            catch
            { }

            try
            {
                searchStart = Convert.ToInt32(tStart.Text, 16);
            }
            catch
            { }


            // clear existing results
            for (int i = 0; i < MaximumSearchResults; i++)
                offsets[i] = 0;

            // search up to X times
            int size = 0;
            while (size < MaximumSearchResults)
            {
                var offset = rom.FindFreeSpace(neededBytes, startOffset: searchStart, alignment: 4);
                if (offset == -1)
                    break;

                offsets[size++] = offset;
                searchStart = offset + neededBytes;
            }

            // fill display
            ignore = true;
            listOffsets.Items.Clear();
            if (size == 0)
            {
                MessageBox.Show("Could not find any free space!", "Out of space!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                bOK.Enabled = false;
            }
            else
            {
                for (int i = 0; i < size; i++)
                    listOffsets.Items.Add($"0x{offsets[i]:X7}");
            }
            ignore = false;
        }

        public int Offset
        {
            get { return offsets[offset]; }
        }
    }
}
