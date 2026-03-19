namespace Minesweeper
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.новаяИграToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mMode = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.помощникToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mShowProbability = new System.Windows.Forms.ToolStripMenuItem();
            this.mShowPercentage = new System.Windows.Forms.ToolStripMenuItem();
            this.синхронизацияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mSaveState = new System.Windows.Forms.ToolStripMenuItem();
            this.mLoadState = new System.Windows.Forms.ToolStripMenuItem();
            this.mExit = new System.Windows.Forms.ToolStripMenuItem();
            this.lbCounter = new System.Windows.Forms.Label();
            this.lbTime = new System.Windows.Forms.Label();
            this.lbMines = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.btRest = new System.Windows.Forms.Button();
            this.lbGameRes = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.синхронизацияToolStripMenuItem,
            this.mExit});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1748, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.новаяИграToolStripMenuItem,
            this.mMode,
            this.toolStripMenuItem2,
            this.помощникToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(46, 20);
            this.toolStripMenuItem1.Text = "Game";
            // 
            // новаяИграToolStripMenuItem
            // 
            this.новаяИграToolStripMenuItem.Name = "новаяИграToolStripMenuItem";
            this.новаяИграToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.новаяИграToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.новаяИграToolStripMenuItem.Text = "New Game";
            this.новаяИграToolStripMenuItem.Click += new System.EventHandler(this.btRest_Click);
            // 
            // mMode
            // 
            this.mMode.Name = "mMode";
            this.mMode.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.mMode.Size = new System.Drawing.Size(199, 22);
            this.mMode.Text = "Select Mode";
            this.mMode.Click += new System.EventHandler(this.mMode_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(196, 6);
            // 
            // помощникToolStripMenuItem
            // 
            this.помощникToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mShowProbability,
            this.mShowPercentage});
            this.помощникToolStripMenuItem.Name = "помощникToolStripMenuItem";
            this.помощникToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.помощникToolStripMenuItem.Text = "Assistant";
            // 
            // mShowProbability
            // 
            this.mShowProbability.Name = "mShowProbability";
            this.mShowProbability.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.mShowProbability.Size = new System.Drawing.Size(298, 22);
            this.mShowProbability.Text = "Calculate success probability";
            this.mShowProbability.Click += new System.EventHandler(this.mShowProbablity_Click);
            // 
            // mShowPercentage
            // 
            this.mShowPercentage.Checked = true;
            this.mShowPercentage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mShowPercentage.Enabled = false;
            this.mShowPercentage.Name = "mShowPercentage";
            this.mShowPercentage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.mShowPercentage.Size = new System.Drawing.Size(298, 22);
            this.mShowPercentage.Text = "Show in percentage";
            this.mShowPercentage.Click += new System.EventHandler(this.mShowPercentage_Click);
            // 
            // синхронизацияToolStripMenuItem
            // 
            this.синхронизацияToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mSaveState,
            this.mLoadState});
            this.синхронизацияToolStripMenuItem.Name = "синхронизацияToolStripMenuItem";
            this.синхронизацияToolStripMenuItem.Size = new System.Drawing.Size(106, 20);
            this.синхронизацияToolStripMenuItem.Text = "Synchronization";
            // 
            // mSaveState
            // 
            this.mSaveState.Name = "mSaveState";
            this.mSaveState.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mSaveState.Size = new System.Drawing.Size(233, 22);
            this.mSaveState.Text = "Save State";
            this.mSaveState.Click += new System.EventHandler(this.mSaveState_Click);
            // 
            // mLoadState
            // 
            this.mLoadState.Name = "mLoadState";
            this.mLoadState.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mLoadState.Size = new System.Drawing.Size(233, 22);
            this.mLoadState.Text = "Load State";
            this.mLoadState.Click += new System.EventHandler(this.mLoadState_Click);
            // 
            // mExit
            // 
            this.mExit.Name = "mExit";
            this.mExit.Size = new System.Drawing.Size(54, 20);
            this.mExit.Text = "Exit";
            this.mExit.Click += new System.EventHandler(this.mExit_Click);
            // 
            // lbCounter
            // 
            this.lbCounter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbCounter.AutoSize = true;
            this.lbCounter.Font = new System.Drawing.Font("Perpetua Titling MT", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCounter.Location = new System.Drawing.Point(12, 829);
            this.lbCounter.Name = "lbCounter";
            this.lbCounter.Size = new System.Drawing.Size(45, 44);
            this.lbCounter.TabIndex = 3;
            this.lbCounter.Text = "0";
            // 
            // lbTime
            // 
            this.lbTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTime.AutoSize = true;
            this.lbTime.Font = new System.Drawing.Font("Perpetua Titling MT", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTime.Location = new System.Drawing.Point(1566, 24);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(45, 44);
            this.lbTime.TabIndex = 3;
            this.lbTime.Text = "0";
            this.lbTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbMines
            // 
            this.lbMines.AutoSize = true;
            this.lbMines.Font = new System.Drawing.Font("Perpetua Titling MT", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMines.Location = new System.Drawing.Point(12, 24);
            this.lbMines.Name = "lbMines";
            this.lbMines.Size = new System.Drawing.Size(45, 44);
            this.lbMines.TabIndex = 4;
            this.lbMines.Text = "0";
            // 
            // timer
            // 
            this.timer.Interval = 10;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // btRest
            // 
            this.btRest.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btRest.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btRest.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btRest.Location = new System.Drawing.Point(827, 27);
            this.btRest.Name = "btRest";
            this.btRest.Size = new System.Drawing.Size(80, 57);
            this.btRest.TabIndex = 2;
            this.btRest.Text = "New Game";
            this.btRest.UseVisualStyleBackColor = true;
            this.btRest.Click += new System.EventHandler(this.btRest_Click);
            // 
            // lbGameRes
            // 
            this.lbGameRes.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lbGameRes.AutoSize = true;
            this.lbGameRes.Font = new System.Drawing.Font("Gabriola", 26.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbGameRes.Location = new System.Drawing.Point(778, 823);
            this.lbGameRes.Name = "lbGameRes";
            this.lbGameRes.Size = new System.Drawing.Size(160, 65);
            this.lbGameRes.TabIndex = 5;
            this.lbGameRes.Text = "lbGameRes";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(1748, 882);
            this.Controls.Add(this.lbGameRes);
            this.Controls.Add(this.lbMines);
            this.Controls.Add(this.lbTime);
            this.Controls.Add(this.lbCounter);
            this.Controls.Add(this.btRest);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Minesweeper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.Form_Load);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.SizeChanged += new System.EventHandler(this.Form_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Button btRest;
        private System.Windows.Forms.Label lbCounter;
        private System.Windows.Forms.ToolStripMenuItem новаяИграToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mMode;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem помощникToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem  mShowProbability;
        private System.Windows.Forms.ToolStripMenuItem mShowPercentage;
        private System.Windows.Forms.ToolStripMenuItem синхронизацияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mSaveState;
        private System.Windows.Forms.ToolStripMenuItem mLoadState;
        private System.Windows.Forms.ToolStripMenuItem mExit;
        private System.Windows.Forms.Label lbTime;
        private System.Windows.Forms.Label lbMines;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label lbGameRes;
    }
}

