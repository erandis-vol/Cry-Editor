namespace Crying
{
    partial class FreeSpaceDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FreeSpaceDialog));
            this.lNeeded = new System.Windows.Forms.Label();
            this.listOffsets = new System.Windows.Forms.ListBox();
            this.tNeeded = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tStart = new System.Windows.Forms.TextBox();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lNeeded
            // 
            this.lNeeded.AutoSize = true;
            this.lNeeded.Location = new System.Drawing.Point(11, 14);
            this.lNeeded.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lNeeded.Name = "lNeeded";
            this.lNeeded.Size = new System.Drawing.Size(82, 13);
            this.lNeeded.TabIndex = 0;
            this.lNeeded.Text = "Needed Space:";
            // 
            // listOffsets
            // 
            this.listOffsets.FormattingEnabled = true;
            this.listOffsets.Location = new System.Drawing.Point(11, 59);
            this.listOffsets.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listOffsets.Name = "listOffsets";
            this.listOffsets.Size = new System.Drawing.Size(194, 160);
            this.listOffsets.TabIndex = 2;
            this.listOffsets.SelectedIndexChanged += new System.EventHandler(this.listOffsets_SelectedIndexChanged);
            // 
            // tNeeded
            // 
            this.tNeeded.Location = new System.Drawing.Point(97, 11);
            this.tNeeded.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tNeeded.Name = "tNeeded";
            this.tNeeded.Size = new System.Drawing.Size(108, 20);
            this.tNeeded.TabIndex = 3;
            this.tNeeded.TextChanged += new System.EventHandler(this.tNeeded_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Search Start:";
            // 
            // tStart
            // 
            this.tStart.Location = new System.Drawing.Point(97, 35);
            this.tStart.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tStart.Name = "tStart";
            this.tStart.Size = new System.Drawing.Size(108, 20);
            this.tStart.TabIndex = 5;
            this.tStart.TextChanged += new System.EventHandler(this.tStart_TextChanged);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(130, 224);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 6;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOK
            // 
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Location = new System.Drawing.Point(49, 224);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 7;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            // 
            // FreeSpaceDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(216, 257);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.tStart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tNeeded);
            this.Controls.Add(this.listOffsets);
            this.Controls.Add(this.lNeeded);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FreeSpaceDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Find Free Space";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FreeSpaceDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lNeeded;
        private System.Windows.Forms.ListBox listOffsets;
        private System.Windows.Forms.TextBox tNeeded;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tStart;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
    }
}