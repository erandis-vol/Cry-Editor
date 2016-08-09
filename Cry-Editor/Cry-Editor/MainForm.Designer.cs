namespace Crying
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.gROM = new System.Windows.Forms.GroupBox();
            this.gCry = new System.Windows.Forms.GroupBox();
            this.lROM = new System.Windows.Forms.Label();
            this.lTable = new System.Windows.Forms.Label();
            this.lOffset = new System.Windows.Forms.Label();
            this.lSampleRate = new System.Windows.Forms.Label();
            this.chkCompressed = new System.Windows.Forms.CheckBox();
            this.chkLooped = new System.Windows.Forms.CheckBox();
            this.listPokemon = new System.Windows.Forms.ListBox();
            this.lSize = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pSample = new System.Windows.Forms.PictureBox();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.gROM.SuspendLayout();
            this.gCry.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pSample)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.cryToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(351, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(178, 6);
            // 
            // cryToolStripMenuItem
            // 
            this.cryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripMenuItem,
            this.toolStripSeparator2,
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.cryToolStripMenuItem.Name = "cryToolStripMenuItem";
            this.cryToolStripMenuItem.Size = new System.Drawing.Size(42, 24);
            this.cryToolStripMenuItem.Text = "Cry";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.importToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // gROM
            // 
            this.gROM.Controls.Add(this.lROM);
            this.gROM.Location = new System.Drawing.Point(12, 31);
            this.gROM.Name = "gROM";
            this.gROM.Size = new System.Drawing.Size(327, 100);
            this.gROM.TabIndex = 1;
            this.gROM.TabStop = false;
            this.gROM.Text = "ROM";
            // 
            // gCry
            // 
            this.gCry.Controls.Add(this.panel1);
            this.gCry.Controls.Add(this.lSize);
            this.gCry.Controls.Add(this.listPokemon);
            this.gCry.Controls.Add(this.chkLooped);
            this.gCry.Controls.Add(this.chkCompressed);
            this.gCry.Controls.Add(this.lSampleRate);
            this.gCry.Controls.Add(this.lOffset);
            this.gCry.Controls.Add(this.lTable);
            this.gCry.Location = new System.Drawing.Point(12, 137);
            this.gCry.Name = "gCry";
            this.gCry.Size = new System.Drawing.Size(327, 464);
            this.gCry.TabIndex = 2;
            this.gCry.TabStop = false;
            this.gCry.Text = "Cry";
            // 
            // lROM
            // 
            this.lROM.AutoSize = true;
            this.lROM.Location = new System.Drawing.Point(6, 18);
            this.lROM.Name = "lROM";
            this.lROM.Size = new System.Drawing.Size(100, 17);
            this.lROM.TabIndex = 0;
            this.lROM.Text = "Load a ROM...";
            // 
            // lTable
            // 
            this.lTable.AutoSize = true;
            this.lTable.Location = new System.Drawing.Point(6, 172);
            this.lTable.Name = "lTable";
            this.lTable.Size = new System.Drawing.Size(74, 17);
            this.lTable.TabIndex = 1;
            this.lTable.Text = "Table: 0x0";
            // 
            // lOffset
            // 
            this.lOffset.AutoSize = true;
            this.lOffset.Location = new System.Drawing.Point(183, 172);
            this.lOffset.Name = "lOffset";
            this.lOffset.Size = new System.Drawing.Size(76, 17);
            this.lOffset.TabIndex = 2;
            this.lOffset.Text = "Offset: 0x0";
            // 
            // lSampleRate
            // 
            this.lSampleRate.AutoSize = true;
            this.lSampleRate.Location = new System.Drawing.Point(6, 189);
            this.lSampleRate.Name = "lSampleRate";
            this.lSampleRate.Size = new System.Drawing.Size(126, 17);
            this.lSampleRate.TabIndex = 3;
            this.lSampleRate.Text = "Sample Rate: 0 Hz";
            // 
            // chkCompressed
            // 
            this.chkCompressed.AutoSize = true;
            this.chkCompressed.Location = new System.Drawing.Point(9, 209);
            this.chkCompressed.Name = "chkCompressed";
            this.chkCompressed.Size = new System.Drawing.Size(109, 21);
            this.chkCompressed.TabIndex = 4;
            this.chkCompressed.Text = "Compressed";
            this.chkCompressed.UseVisualStyleBackColor = true;
            this.chkCompressed.CheckedChanged += new System.EventHandler(this.chkCompressed_CheckedChanged);
            // 
            // chkLooped
            // 
            this.chkLooped.AutoSize = true;
            this.chkLooped.Enabled = false;
            this.chkLooped.Location = new System.Drawing.Point(186, 209);
            this.chkLooped.Name = "chkLooped";
            this.chkLooped.Size = new System.Drawing.Size(78, 21);
            this.chkLooped.TabIndex = 5;
            this.chkLooped.Text = "Looped";
            this.chkLooped.UseVisualStyleBackColor = true;
            // 
            // listPokemon
            // 
            this.listPokemon.FormattingEnabled = true;
            this.listPokemon.ItemHeight = 16;
            this.listPokemon.Location = new System.Drawing.Point(9, 21);
            this.listPokemon.Name = "listPokemon";
            this.listPokemon.Size = new System.Drawing.Size(309, 148);
            this.listPokemon.TabIndex = 6;
            this.listPokemon.SelectedIndexChanged += new System.EventHandler(this.listPokemon_SelectedIndexChanged);
            // 
            // lSize
            // 
            this.lSize.AutoSize = true;
            this.lSize.Location = new System.Drawing.Point(183, 189);
            this.lSize.Name = "lSize";
            this.lSize.Size = new System.Drawing.Size(107, 17);
            this.lSize.TabIndex = 4;
            this.lSize.Text = "Size: 0 samples";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pSample);
            this.panel1.Location = new System.Drawing.Point(9, 236);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(309, 222);
            this.panel1.TabIndex = 7;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(178, 6);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // pSample
            // 
            this.pSample.Location = new System.Drawing.Point(0, 0);
            this.pSample.Name = "pSample";
            this.pSample.Size = new System.Drawing.Size(16, 16);
            this.pSample.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pSample.TabIndex = 0;
            this.pSample.TabStop = false;
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::Crying.Properties.Resources.folder;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::Crying.Properties.Resources.disk;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::Crying.Properties.Resources.cross;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Image = global::Crying.Properties.Resources.sound;
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.playToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.playToolStripMenuItem.Text = "Play";
            this.playToolStripMenuItem.Click += new System.EventHandler(this.playToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = global::Crying.Properties.Resources.information;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.A)));
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 613);
            this.Controls.Add(this.gCry);
            this.Controls.Add(this.gROM);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cry Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.gROM.ResumeLayout(false);
            this.gROM.PerformLayout();
            this.gCry.ResumeLayout(false);
            this.gCry.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pSample)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.GroupBox gROM;
        private System.Windows.Forms.GroupBox gCry;
        private System.Windows.Forms.Label lROM;
        private System.Windows.Forms.Label lOffset;
        private System.Windows.Forms.Label lTable;
        private System.Windows.Forms.Label lSampleRate;
        private System.Windows.Forms.CheckBox chkCompressed;
        private System.Windows.Forms.CheckBox chkLooped;
        private System.Windows.Forms.ListBox listPokemon;
        private System.Windows.Forms.Label lSize;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pSample;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}

