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
            this.label1 = new System.Windows.Forms.Label();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.tNeeded = new Crying.DecimalBox();
            this.tStart = new Crying.HexBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tRepointTo = new Crying.HexBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lNeeded
            // 
            this.lNeeded.AutoSize = true;
            this.lNeeded.Location = new System.Drawing.Point(11, 15);
            this.lNeeded.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lNeeded.Name = "lNeeded";
            this.lNeeded.Size = new System.Drawing.Size(82, 13);
            this.lNeeded.TabIndex = 0;
            this.lNeeded.Text = "Needed Space:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 41);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Search Start:";
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(111, 126);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(93, 23);
            this.bCancel.TabIndex = 6;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOK
            // 
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Location = new System.Drawing.Point(12, 126);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(93, 23);
            this.bOK.TabIndex = 7;
            this.bOK.Text = "Repoint";
            this.bOK.UseVisualStyleBackColor = true;
            // 
            // tNeeded
            // 
            this.tNeeded.Location = new System.Drawing.Point(97, 12);
            this.tNeeded.MaximumValue = 2147483646;
            this.tNeeded.MinimumValue = 0;
            this.tNeeded.Name = "tNeeded";
            this.tNeeded.Size = new System.Drawing.Size(107, 20);
            this.tNeeded.TabIndex = 8;
            this.tNeeded.Text = "0";
            this.tNeeded.Value = 0;
            // 
            // tStart
            // 
            this.tStart.Location = new System.Drawing.Point(113, 38);
            this.tStart.MaximumValue = 2147483646;
            this.tStart.MinimumValue = 0;
            this.tStart.Name = "tStart";
            this.tStart.Size = new System.Drawing.Size(91, 20);
            this.tStart.TabIndex = 9;
            this.tStart.Text = "0";
            this.tStart.Value = 0;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(97, 38);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(17, 20);
            this.textBox1.TabIndex = 10;
            this.textBox1.Text = "0x";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 64);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(192, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Find Free Space";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.Location = new System.Drawing.Point(12, 93);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(192, 1);
            this.panel1.TabIndex = 12;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(97, 100);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(17, 20);
            this.textBox2.TabIndex = 15;
            this.textBox2.Text = "0x";
            // 
            // tRepointTo
            // 
            this.tRepointTo.Location = new System.Drawing.Point(113, 100);
            this.tRepointTo.MaximumValue = 2147483646;
            this.tRepointTo.MinimumValue = 0;
            this.tRepointTo.Name = "tRepointTo";
            this.tRepointTo.Size = new System.Drawing.Size(91, 20);
            this.tRepointTo.TabIndex = 14;
            this.tRepointTo.Text = "0";
            this.tRepointTo.Value = 0;
            this.tRepointTo.TextChanged += new System.EventHandler(this.tRepointTo_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 103);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Repoint To:";
            // 
            // FreeSpaceDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(216, 161);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.tRepointTo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.tStart);
            this.Controls.Add(this.tNeeded);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lNeeded);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
        private DecimalBox tNeeded;
        private HexBox tStart;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox2;
        private HexBox tRepointTo;
        private System.Windows.Forms.Label label2;
    }
}