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
            this.lMessage = new System.Windows.Forms.Label();
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
            this.lNeeded.Location = new System.Drawing.Point(12, 93);
            this.lNeeded.Name = "lNeeded";
            this.lNeeded.Size = new System.Drawing.Size(106, 17);
            this.lNeeded.TabIndex = 0;
            this.lNeeded.Text = "Needed Space:";
            // 
            // lMessage
            // 
            this.lMessage.AutoSize = true;
            this.lMessage.Location = new System.Drawing.Point(12, 9);
            this.lMessage.Name = "lMessage";
            this.lMessage.Size = new System.Drawing.Size(149, 17);
            this.lMessage.TabIndex = 1;
            this.lMessage.Text = "Search for free space.";
            // 
            // listOffsets
            // 
            this.listOffsets.FormattingEnabled = true;
            this.listOffsets.ItemHeight = 16;
            this.listOffsets.Location = new System.Drawing.Point(12, 146);
            this.listOffsets.Name = "listOffsets";
            this.listOffsets.Size = new System.Drawing.Size(258, 244);
            this.listOffsets.TabIndex = 2;
            this.listOffsets.SelectedIndexChanged += new System.EventHandler(this.listOffsets_SelectedIndexChanged);
            // 
            // tNeeded
            // 
            this.tNeeded.Location = new System.Drawing.Point(124, 90);
            this.tNeeded.Name = "tNeeded";
            this.tNeeded.Size = new System.Drawing.Size(146, 22);
            this.tNeeded.TabIndex = 3;
            this.tNeeded.TextChanged += new System.EventHandler(this.tNeeded_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Search Start:";
            // 
            // tStart
            // 
            this.tStart.Location = new System.Drawing.Point(124, 118);
            this.tStart.Name = "tStart";
            this.tStart.Size = new System.Drawing.Size(146, 22);
            this.tStart.TabIndex = 5;
            this.tStart.TextChanged += new System.EventHandler(this.tStart_TextChanged);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(195, 396);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 6;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bOK
            // 
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Location = new System.Drawing.Point(114, 396);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 7;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // FreeSpaceDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 431);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.tStart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tNeeded);
            this.Controls.Add(this.listOffsets);
            this.Controls.Add(this.lMessage);
            this.Controls.Add(this.lNeeded);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
        private System.Windows.Forms.Label lMessage;
        private System.Windows.Forms.ListBox listOffsets;
        private System.Windows.Forms.TextBox tNeeded;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tStart;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
    }
}