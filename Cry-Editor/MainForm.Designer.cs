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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.gROM = new System.Windows.Forms.GroupBox();
            this.lROM = new System.Windows.Forms.Label();
            this.gCry = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pSample = new System.Windows.Forms.PictureBox();
            this.lSize = new System.Windows.Forms.Label();
            this.listPokemon = new System.Windows.Forms.ListBox();
            this.chkLooped = new System.Windows.Forms.CheckBox();
            this.chkCompressed = new System.Windows.Forms.CheckBox();
            this.lSampleRate = new System.Windows.Forms.Label();
            this.lOffset = new System.Windows.Forms.Label();
            this.lTable = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkClean = new System.Windows.Forms.CheckBox();
            this.chkForceRepoint = new System.Windows.Forms.CheckBox();
            this.gROM.SuspendLayout();
            this.gCry.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pSample)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gROM
            // 
            this.gROM.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gROM.Controls.Add(this.lROM);
            this.gROM.Location = new System.Drawing.Point(9, 25);
            this.gROM.Margin = new System.Windows.Forms.Padding(2);
            this.gROM.Name = "gROM";
            this.gROM.Padding = new System.Windows.Forms.Padding(2);
            this.gROM.Size = new System.Drawing.Size(245, 74);
            this.gROM.TabIndex = 1;
            this.gROM.TabStop = false;
            this.gROM.Text = "ROM";
            // 
            // lROM
            // 
            this.lROM.AutoSize = true;
            this.lROM.Location = new System.Drawing.Point(4, 15);
            this.lROM.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lROM.Name = "lROM";
            this.lROM.Size = new System.Drawing.Size(77, 13);
            this.lROM.TabIndex = 0;
            this.lROM.Text = "Load a ROM...";
            // 
            // gCry
            // 
            this.gCry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gCry.Controls.Add(this.panel1);
            this.gCry.Controls.Add(this.lSize);
            this.gCry.Controls.Add(this.listPokemon);
            this.gCry.Controls.Add(this.chkLooped);
            this.gCry.Controls.Add(this.chkCompressed);
            this.gCry.Controls.Add(this.lSampleRate);
            this.gCry.Controls.Add(this.lOffset);
            this.gCry.Controls.Add(this.lTable);
            this.gCry.Location = new System.Drawing.Point(9, 103);
            this.gCry.Margin = new System.Windows.Forms.Padding(2);
            this.gCry.Name = "gCry";
            this.gCry.Padding = new System.Windows.Forms.Padding(2);
            this.gCry.Size = new System.Drawing.Size(245, 344);
            this.gCry.TabIndex = 2;
            this.gCry.TabStop = false;
            this.gCry.Text = "Cry";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pSample);
            this.panel1.Location = new System.Drawing.Point(7, 192);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(232, 148);
            this.panel1.TabIndex = 7;
            // 
            // pSample
            // 
            this.pSample.Location = new System.Drawing.Point(0, 0);
            this.pSample.Margin = new System.Windows.Forms.Padding(2);
            this.pSample.Name = "pSample";
            this.pSample.Size = new System.Drawing.Size(33, 129);
            this.pSample.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pSample.TabIndex = 0;
            this.pSample.TabStop = false;
            // 
            // lSize
            // 
            this.lSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lSize.AutoSize = true;
            this.lSize.Location = new System.Drawing.Point(137, 154);
            this.lSize.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lSize.Name = "lSize";
            this.lSize.Size = new System.Drawing.Size(80, 13);
            this.lSize.TabIndex = 4;
            this.lSize.Text = "Size: 0 samples";
            // 
            // listPokemon
            // 
            this.listPokemon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listPokemon.FormattingEnabled = true;
            this.listPokemon.Location = new System.Drawing.Point(7, 17);
            this.listPokemon.Margin = new System.Windows.Forms.Padding(2);
            this.listPokemon.Name = "listPokemon";
            this.listPokemon.Size = new System.Drawing.Size(233, 121);
            this.listPokemon.TabIndex = 6;
            this.listPokemon.SelectedIndexChanged += new System.EventHandler(this.listPokemon_SelectedIndexChanged);
            // 
            // chkLooped
            // 
            this.chkLooped.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkLooped.AutoSize = true;
            this.chkLooped.Enabled = false;
            this.chkLooped.Location = new System.Drawing.Point(140, 170);
            this.chkLooped.Margin = new System.Windows.Forms.Padding(2);
            this.chkLooped.Name = "chkLooped";
            this.chkLooped.Size = new System.Drawing.Size(62, 17);
            this.chkLooped.TabIndex = 5;
            this.chkLooped.Text = "Looped";
            this.chkLooped.UseVisualStyleBackColor = true;
            // 
            // chkCompressed
            // 
            this.chkCompressed.AutoSize = true;
            this.chkCompressed.Location = new System.Drawing.Point(7, 170);
            this.chkCompressed.Margin = new System.Windows.Forms.Padding(2);
            this.chkCompressed.Name = "chkCompressed";
            this.chkCompressed.Size = new System.Drawing.Size(84, 17);
            this.chkCompressed.TabIndex = 4;
            this.chkCompressed.Text = "Compressed";
            this.chkCompressed.UseVisualStyleBackColor = true;
            this.chkCompressed.CheckedChanged += new System.EventHandler(this.chkCompressed_CheckedChanged);
            // 
            // lSampleRate
            // 
            this.lSampleRate.AutoSize = true;
            this.lSampleRate.Location = new System.Drawing.Point(4, 154);
            this.lSampleRate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lSampleRate.Name = "lSampleRate";
            this.lSampleRate.Size = new System.Drawing.Size(96, 13);
            this.lSampleRate.TabIndex = 3;
            this.lSampleRate.Text = "Sample Rate: 0 Hz";
            // 
            // lOffset
            // 
            this.lOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lOffset.AutoSize = true;
            this.lOffset.Location = new System.Drawing.Point(137, 140);
            this.lOffset.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lOffset.Name = "lOffset";
            this.lOffset.Size = new System.Drawing.Size(58, 13);
            this.lOffset.TabIndex = 2;
            this.lOffset.Text = "Offset: 0x0";
            // 
            // lTable
            // 
            this.lTable.AutoSize = true;
            this.lTable.Location = new System.Drawing.Point(4, 140);
            this.lTable.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lTable.Name = "lTable";
            this.lTable.Size = new System.Drawing.Size(57, 13);
            this.lTable.TabIndex = 1;
            this.lTable.Text = "Table: 0x0";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.cryToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(263, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::Crying.Properties.Resources.OpenFolder_16x;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::Crying.Properties.Resources.Save_16x;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::Crying.Properties.Resources.Close_16x;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // cryToolStripMenuItem
            // 
            this.cryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripMenuItem,
            this.toolStripSeparator2,
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.cryToolStripMenuItem.Name = "cryToolStripMenuItem";
            this.cryToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.cryToolStripMenuItem.Text = "Cry";
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Image = global::Crying.Properties.Resources.SoundFile_16x;
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.playToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.playToolStripMenuItem.Text = "Play";
            this.playToolStripMenuItem.Click += new System.EventHandler(this.playToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(144, 6);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Image = global::Crying.Properties.Resources.Import_16x;
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.importToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Image = global::Crying.Properties.Resources.Export_16x;
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = global::Crying.Properties.Resources.InformationSymbol_16x;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.A)));
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // chkClean
            // 
            this.chkClean.AutoSize = true;
            this.chkClean.Checked = true;
            this.chkClean.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkClean.Location = new System.Drawing.Point(108, 452);
            this.chkClean.Name = "chkClean";
            this.chkClean.Size = new System.Drawing.Size(123, 17);
            this.chkClean.TabIndex = 4;
            this.chkClean.Text = "Clean Repointed Cry";
            this.chkClean.UseVisualStyleBackColor = true;
            // 
            // chkForceRepoint
            // 
            this.chkForceRepoint.AutoSize = true;
            this.chkForceRepoint.Location = new System.Drawing.Point(9, 452);
            this.chkForceRepoint.Name = "chkForceRepoint";
            this.chkForceRepoint.Size = new System.Drawing.Size(93, 17);
            this.chkForceRepoint.TabIndex = 5;
            this.chkForceRepoint.Text = "Force Repoint";
            this.chkForceRepoint.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(263, 475);
            this.Controls.Add(this.chkForceRepoint);
            this.Controls.Add(this.chkClean);
            this.Controls.Add(this.gCry);
            this.Controls.Add(this.gROM);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(279, 504);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cry Editor";
            this.gROM.ResumeLayout(false);
            this.gROM.PerformLayout();
            this.gCry.ResumeLayout(false);
            this.gCry.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pSample)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
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
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkClean;
        private System.Windows.Forms.CheckBox chkForceRepoint;
    }
}

