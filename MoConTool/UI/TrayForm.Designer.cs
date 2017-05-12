namespace net.r_eg.MoConTool.UI
{
    partial class TrayForm
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
            if(disposing && (components != null)) {
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
            this.notifyIconMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.menuTray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuCaption = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuTweaks = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSrcCode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.chkDebug = new System.Windows.Forms.CheckBox();
            this.listBoxDebug = new System.Windows.Forms.ListBox();
            this.contextMenuDebug = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuClear = new System.Windows.Forms.ToolStripMenuItem();
            this.panelSmallLine = new System.Windows.Forms.Panel();
            this.chkPlug = new System.Windows.Forms.CheckBox();
            this.panelMain = new System.Windows.Forms.Panel();
            this.numIntClickDeltaMin = new System.Windows.Forms.NumericUpDown();
            this.numIntClickDeltaMax = new System.Windows.Forms.NumericUpDown();
            this.chkMixedClicksOnlyDown = new System.Windows.Forms.CheckBox();
            this.numHyperScrollLimit = new System.Windows.Forms.NumericUpDown();
            this.chkDoubleBtnR = new System.Windows.Forms.CheckBox();
            this.chkDoubleBtnM = new System.Windows.Forms.CheckBox();
            this.chkDoubleBtnL = new System.Windows.Forms.CheckBox();
            this.chkMixedBtnR = new System.Windows.Forms.CheckBox();
            this.chkMixedBtnM = new System.Windows.Forms.CheckBox();
            this.chkMixedBtnL = new System.Windows.Forms.CheckBox();
            this.chkInterruptedBtnR = new System.Windows.Forms.CheckBox();
            this.chkInterruptedBtnM = new System.Windows.Forms.CheckBox();
            this.chkInterruptedBtnL = new System.Windows.Forms.CheckBox();
            this.labelHyperactiveScroll = new System.Windows.Forms.Label();
            this.numHyperScrollCapacity = new System.Windows.Forms.NumericUpDown();
            this.chkHyperactiveScroll = new System.Windows.Forms.CheckBox();
            this.labelDoubleClick = new System.Windows.Forms.Label();
            this.numDoubleClick = new System.Windows.Forms.NumericUpDown();
            this.chkDoubleClick = new System.Windows.Forms.CheckBox();
            this.labelMixedClicks = new System.Windows.Forms.Label();
            this.chkMixedClicks = new System.Windows.Forms.CheckBox();
            this.labelInterruptedClick = new System.Windows.Forms.Label();
            this.numInterruptedClick = new System.Windows.Forms.NumericUpDown();
            this.chkInterruptedClick = new System.Windows.Forms.CheckBox();
            this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
            this.btnCmd = new System.Windows.Forms.Button();
            this.menuTray.SuspendLayout();
            this.contextMenuDebug.SuspendLayout();
            this.panelSmallLine.SuspendLayout();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIntClickDeltaMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIntClickDeltaMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHyperScrollLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHyperScrollCapacity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDoubleClick)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInterruptedClick)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIconMain
            // 
            this.notifyIconMain.ContextMenuStrip = this.menuTray;
            this.notifyIconMain.Text = "MoConTool";
            this.notifyIconMain.Visible = true;
            this.notifyIconMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.notifyIconMain_MouseUp);
            // 
            // menuTray
            // 
            this.menuTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCaption,
            this.toolStripSeparator1,
            this.menuTweaks,
            this.toolStripSeparator2,
            this.menuSrcCode,
            this.menuAbout,
            this.menuExit});
            this.menuTray.Name = "menuTray";
            this.menuTray.Size = new System.Drawing.Size(189, 126);
            // 
            // menuCaption
            // 
            this.menuCaption.Enabled = false;
            this.menuCaption.Name = "menuCaption";
            this.menuCaption.Size = new System.Drawing.Size(188, 22);
            this.menuCaption.Text = "MoConTool v~";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(185, 6);
            // 
            // menuTweaks
            // 
            this.menuTweaks.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.menuTweaks.Name = "menuTweaks";
            this.menuTweaks.Size = new System.Drawing.Size(188, 22);
            this.menuTweaks.Text = "Tweaks";
            this.menuTweaks.Click += new System.EventHandler(this.menuTweaks_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(185, 6);
            // 
            // menuSrcCode
            // 
            this.menuSrcCode.Name = "menuSrcCode";
            this.menuSrcCode.Size = new System.Drawing.Size(188, 22);
            this.menuSrcCode.Text = "Source code [GitHub]";
            this.menuSrcCode.Click += new System.EventHandler(this.menuSrcCode_Click);
            // 
            // menuAbout
            // 
            this.menuAbout.Name = "menuAbout";
            this.menuAbout.Size = new System.Drawing.Size(188, 22);
            this.menuAbout.Text = "About";
            this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(188, 22);
            this.menuExit.Text = "Exit";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // chkDebug
            // 
            this.chkDebug.AutoSize = true;
            this.chkDebug.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkDebug.Location = new System.Drawing.Point(54, 3);
            this.chkDebug.Name = "chkDebug";
            this.chkDebug.Size = new System.Drawing.Size(56, 17);
            this.chkDebug.TabIndex = 13;
            this.chkDebug.Text = "Debug";
            this.chkDebug.UseVisualStyleBackColor = true;
            this.chkDebug.CheckedChanged += new System.EventHandler(this.chkDebug_CheckedChanged);
            // 
            // listBoxDebug
            // 
            this.listBoxDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxDebug.ContextMenuStrip = this.contextMenuDebug;
            this.listBoxDebug.FormattingEnabled = true;
            this.listBoxDebug.HorizontalScrollbar = true;
            this.listBoxDebug.Location = new System.Drawing.Point(3, 167);
            this.listBoxDebug.Name = "listBoxDebug";
            this.listBoxDebug.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxDebug.Size = new System.Drawing.Size(556, 184);
            this.listBoxDebug.TabIndex = 14;
            this.listBoxDebug.MouseLeave += new System.EventHandler(this.listBoxDebug_MouseLeave);
            this.listBoxDebug.MouseHover += new System.EventHandler(this.listBoxDebug_MouseHover);
            // 
            // contextMenuDebug
            // 
            this.contextMenuDebug.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCopy,
            this.menuSelectAll,
            this.toolStripSeparator3,
            this.menuClear});
            this.contextMenuDebug.Name = "contextMenuDebug";
            this.contextMenuDebug.Size = new System.Drawing.Size(149, 76);
            this.contextMenuDebug.MouseLeave += new System.EventHandler(this.contextMenuDebug_MouseLeave);
            this.contextMenuDebug.MouseHover += new System.EventHandler(this.contextMenuDebug_MouseHover);
            // 
            // menuCopy
            // 
            this.menuCopy.Name = "menuCopy";
            this.menuCopy.Size = new System.Drawing.Size(148, 22);
            this.menuCopy.Text = "Copy selected";
            this.menuCopy.Click += new System.EventHandler(this.menuCopy_Click);
            // 
            // menuSelectAll
            // 
            this.menuSelectAll.Name = "menuSelectAll";
            this.menuSelectAll.Size = new System.Drawing.Size(148, 22);
            this.menuSelectAll.Text = "Select All";
            this.menuSelectAll.Click += new System.EventHandler(this.menuSelectAll_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(145, 6);
            // 
            // menuClear
            // 
            this.menuClear.Name = "menuClear";
            this.menuClear.Size = new System.Drawing.Size(148, 22);
            this.menuClear.Text = "Clear";
            this.menuClear.Click += new System.EventHandler(this.menuClear_Click);
            // 
            // panelSmallLine
            // 
            this.panelSmallLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSmallLine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSmallLine.Controls.Add(this.chkPlug);
            this.panelSmallLine.Controls.Add(this.chkDebug);
            this.panelSmallLine.Location = new System.Drawing.Point(448, 142);
            this.panelSmallLine.Name = "panelSmallLine";
            this.panelSmallLine.Size = new System.Drawing.Size(111, 24);
            this.panelSmallLine.TabIndex = 15;
            // 
            // chkPlug
            // 
            this.chkPlug.AutoSize = true;
            this.chkPlug.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkPlug.Location = new System.Drawing.Point(3, 3);
            this.chkPlug.Name = "chkPlug";
            this.chkPlug.Size = new System.Drawing.Size(45, 17);
            this.chkPlug.TabIndex = 16;
            this.chkPlug.Text = "Plug";
            this.chkPlug.UseVisualStyleBackColor = true;
            this.chkPlug.CheckedChanged += new System.EventHandler(this.chkPlug_CheckedChanged);
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.numIntClickDeltaMin);
            this.panelMain.Controls.Add(this.numIntClickDeltaMax);
            this.panelMain.Controls.Add(this.chkMixedClicksOnlyDown);
            this.panelMain.Controls.Add(this.numHyperScrollLimit);
            this.panelMain.Controls.Add(this.chkDoubleBtnR);
            this.panelMain.Controls.Add(this.chkDoubleBtnM);
            this.panelMain.Controls.Add(this.chkDoubleBtnL);
            this.panelMain.Controls.Add(this.chkMixedBtnR);
            this.panelMain.Controls.Add(this.chkMixedBtnM);
            this.panelMain.Controls.Add(this.chkMixedBtnL);
            this.panelMain.Controls.Add(this.chkInterruptedBtnR);
            this.panelMain.Controls.Add(this.chkInterruptedBtnM);
            this.panelMain.Controls.Add(this.chkInterruptedBtnL);
            this.panelMain.Controls.Add(this.labelHyperactiveScroll);
            this.panelMain.Controls.Add(this.numHyperScrollCapacity);
            this.panelMain.Controls.Add(this.chkHyperactiveScroll);
            this.panelMain.Controls.Add(this.labelDoubleClick);
            this.panelMain.Controls.Add(this.numDoubleClick);
            this.panelMain.Controls.Add(this.chkDoubleClick);
            this.panelMain.Controls.Add(this.labelMixedClicks);
            this.panelMain.Controls.Add(this.chkMixedClicks);
            this.panelMain.Controls.Add(this.labelInterruptedClick);
            this.panelMain.Controls.Add(this.numInterruptedClick);
            this.panelMain.Controls.Add(this.chkInterruptedClick);
            this.panelMain.Location = new System.Drawing.Point(12, 1);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(538, 135);
            this.panelMain.TabIndex = 16;
            // 
            // numIntClickDeltaMin
            // 
            this.numIntClickDeltaMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numIntClickDeltaMin.Location = new System.Drawing.Point(430, 12);
            this.numIntClickDeltaMin.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.numIntClickDeltaMin.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numIntClickDeltaMin.Name = "numIntClickDeltaMin";
            this.numIntClickDeltaMin.Size = new System.Drawing.Size(49, 20);
            this.numIntClickDeltaMin.TabIndex = 36;
            this.toolTipMain.SetToolTip(this.numIntClickDeltaMin, "Delta-Min of user click. Recommended: 10 - 70 ms");
            this.numIntClickDeltaMin.ValueChanged += new System.EventHandler(this.numIntClickDeltaMin_ValueChanged);
            // 
            // numIntClickDeltaMax
            // 
            this.numIntClickDeltaMax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numIntClickDeltaMax.Location = new System.Drawing.Point(481, 12);
            this.numIntClickDeltaMax.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.numIntClickDeltaMax.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numIntClickDeltaMax.Name = "numIntClickDeltaMax";
            this.numIntClickDeltaMax.Size = new System.Drawing.Size(54, 20);
            this.numIntClickDeltaMax.TabIndex = 35;
            this.toolTipMain.SetToolTip(this.numIntClickDeltaMax, "Delta-Max of user click. Recommended: 150 - 400 ms");
            this.numIntClickDeltaMax.ValueChanged += new System.EventHandler(this.numIntClickDeltaMax_ValueChanged);
            // 
            // chkMixedClicksOnlyDown
            // 
            this.chkMixedClicksOnlyDown.AutoSize = true;
            this.chkMixedClicksOnlyDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkMixedClicksOnlyDown.Location = new System.Drawing.Point(372, 44);
            this.chkMixedClicksOnlyDown.Name = "chkMixedClicksOnlyDown";
            this.chkMixedClicksOnlyDown.Size = new System.Drawing.Size(107, 17);
            this.chkMixedClicksOnlyDown.TabIndex = 34;
            this.chkMixedClicksOnlyDown.Text = "Only Down-codes";
            this.chkMixedClicksOnlyDown.UseVisualStyleBackColor = true;
            this.chkMixedClicksOnlyDown.CheckedChanged += new System.EventHandler(this.chkMixedClicksOnlyDown_CheckedChanged);
            // 
            // numHyperScrollLimit
            // 
            this.numHyperScrollLimit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numHyperScrollLimit.Location = new System.Drawing.Point(453, 108);
            this.numHyperScrollLimit.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.numHyperScrollLimit.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numHyperScrollLimit.Name = "numHyperScrollLimit";
            this.numHyperScrollLimit.Size = new System.Drawing.Size(82, 20);
            this.numHyperScrollLimit.TabIndex = 33;
            this.toolTipMain.SetToolTip(this.numHyperScrollLimit, "Range of limit in ms. Recommended: 150 - 300 ms");
            this.numHyperScrollLimit.ValueChanged += new System.EventHandler(this.numHyperScrollLimit_ValueChanged);
            // 
            // chkDoubleBtnR
            // 
            this.chkDoubleBtnR.AutoSize = true;
            this.chkDoubleBtnR.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkDoubleBtnR.Location = new System.Drawing.Point(247, 78);
            this.chkDoubleBtnR.Name = "chkDoubleBtnR";
            this.chkDoubleBtnR.Size = new System.Drawing.Size(13, 12);
            this.chkDoubleBtnR.TabIndex = 32;
            this.toolTipMain.SetToolTip(this.chkDoubleBtnR, "Right button");
            this.chkDoubleBtnR.UseVisualStyleBackColor = true;
            this.chkDoubleBtnR.CheckedChanged += new System.EventHandler(this.chkDoubleBtnR_CheckedChanged);
            // 
            // chkDoubleBtnM
            // 
            this.chkDoubleBtnM.AutoSize = true;
            this.chkDoubleBtnM.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkDoubleBtnM.Location = new System.Drawing.Point(235, 78);
            this.chkDoubleBtnM.Name = "chkDoubleBtnM";
            this.chkDoubleBtnM.Size = new System.Drawing.Size(13, 12);
            this.chkDoubleBtnM.TabIndex = 31;
            this.toolTipMain.SetToolTip(this.chkDoubleBtnM, "Middle button");
            this.chkDoubleBtnM.UseVisualStyleBackColor = true;
            this.chkDoubleBtnM.CheckedChanged += new System.EventHandler(this.chkDoubleBtnM_CheckedChanged);
            // 
            // chkDoubleBtnL
            // 
            this.chkDoubleBtnL.AutoSize = true;
            this.chkDoubleBtnL.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkDoubleBtnL.Location = new System.Drawing.Point(223, 78);
            this.chkDoubleBtnL.Name = "chkDoubleBtnL";
            this.chkDoubleBtnL.Size = new System.Drawing.Size(13, 12);
            this.chkDoubleBtnL.TabIndex = 30;
            this.toolTipMain.SetToolTip(this.chkDoubleBtnL, "Left button");
            this.chkDoubleBtnL.UseVisualStyleBackColor = true;
            this.chkDoubleBtnL.CheckedChanged += new System.EventHandler(this.chkDoubleBtnL_CheckedChanged);
            // 
            // chkMixedBtnR
            // 
            this.chkMixedBtnR.AutoSize = true;
            this.chkMixedBtnR.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkMixedBtnR.Location = new System.Drawing.Point(247, 46);
            this.chkMixedBtnR.Name = "chkMixedBtnR";
            this.chkMixedBtnR.Size = new System.Drawing.Size(13, 12);
            this.chkMixedBtnR.TabIndex = 29;
            this.toolTipMain.SetToolTip(this.chkMixedBtnR, "Right button");
            this.chkMixedBtnR.UseVisualStyleBackColor = true;
            this.chkMixedBtnR.CheckedChanged += new System.EventHandler(this.chkMixedBtnR_CheckedChanged);
            // 
            // chkMixedBtnM
            // 
            this.chkMixedBtnM.AutoSize = true;
            this.chkMixedBtnM.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkMixedBtnM.Location = new System.Drawing.Point(235, 46);
            this.chkMixedBtnM.Name = "chkMixedBtnM";
            this.chkMixedBtnM.Size = new System.Drawing.Size(13, 12);
            this.chkMixedBtnM.TabIndex = 28;
            this.toolTipMain.SetToolTip(this.chkMixedBtnM, "Middle button");
            this.chkMixedBtnM.UseVisualStyleBackColor = true;
            this.chkMixedBtnM.CheckedChanged += new System.EventHandler(this.chkMixedBtnM_CheckedChanged);
            // 
            // chkMixedBtnL
            // 
            this.chkMixedBtnL.AutoSize = true;
            this.chkMixedBtnL.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkMixedBtnL.Location = new System.Drawing.Point(223, 46);
            this.chkMixedBtnL.Name = "chkMixedBtnL";
            this.chkMixedBtnL.Size = new System.Drawing.Size(13, 12);
            this.chkMixedBtnL.TabIndex = 27;
            this.toolTipMain.SetToolTip(this.chkMixedBtnL, "Left button");
            this.chkMixedBtnL.UseVisualStyleBackColor = true;
            this.chkMixedBtnL.CheckedChanged += new System.EventHandler(this.chkMixedBtnL_CheckedChanged);
            // 
            // chkInterruptedBtnR
            // 
            this.chkInterruptedBtnR.AutoSize = true;
            this.chkInterruptedBtnR.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkInterruptedBtnR.Location = new System.Drawing.Point(248, 14);
            this.chkInterruptedBtnR.Name = "chkInterruptedBtnR";
            this.chkInterruptedBtnR.Size = new System.Drawing.Size(13, 12);
            this.chkInterruptedBtnR.TabIndex = 26;
            this.toolTipMain.SetToolTip(this.chkInterruptedBtnR, "Right button");
            this.chkInterruptedBtnR.UseVisualStyleBackColor = true;
            this.chkInterruptedBtnR.CheckedChanged += new System.EventHandler(this.chkInterruptedBtnR_CheckedChanged);
            // 
            // chkInterruptedBtnM
            // 
            this.chkInterruptedBtnM.AutoSize = true;
            this.chkInterruptedBtnM.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkInterruptedBtnM.Location = new System.Drawing.Point(236, 14);
            this.chkInterruptedBtnM.Name = "chkInterruptedBtnM";
            this.chkInterruptedBtnM.Size = new System.Drawing.Size(13, 12);
            this.chkInterruptedBtnM.TabIndex = 25;
            this.toolTipMain.SetToolTip(this.chkInterruptedBtnM, "Middle button");
            this.chkInterruptedBtnM.UseVisualStyleBackColor = true;
            this.chkInterruptedBtnM.CheckedChanged += new System.EventHandler(this.chkInterruptedBtnM_CheckedChanged);
            // 
            // chkInterruptedBtnL
            // 
            this.chkInterruptedBtnL.AutoSize = true;
            this.chkInterruptedBtnL.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkInterruptedBtnL.Location = new System.Drawing.Point(224, 14);
            this.chkInterruptedBtnL.Name = "chkInterruptedBtnL";
            this.chkInterruptedBtnL.Size = new System.Drawing.Size(13, 12);
            this.chkInterruptedBtnL.TabIndex = 24;
            this.toolTipMain.SetToolTip(this.chkInterruptedBtnL, "Left button");
            this.chkInterruptedBtnL.UseVisualStyleBackColor = true;
            this.chkInterruptedBtnL.CheckedChanged += new System.EventHandler(this.chkInterruptedBtnL_CheckedChanged);
            // 
            // labelHyperactiveScroll
            // 
            this.labelHyperactiveScroll.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelHyperactiveScroll.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelHyperactiveScroll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelHyperactiveScroll.Location = new System.Drawing.Point(266, 105);
            this.labelHyperactiveScroll.Name = "labelHyperactiveScroll";
            this.labelHyperactiveScroll.Size = new System.Drawing.Size(100, 23);
            this.labelHyperactiveScroll.TabIndex = 23;
            this.labelHyperactiveScroll.Text = "0";
            this.labelHyperactiveScroll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelHyperactiveScroll.Click += new System.EventHandler(this.labelHyperactiveScroll_Click);
            // 
            // numHyperScrollCapacity
            // 
            this.numHyperScrollCapacity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numHyperScrollCapacity.Location = new System.Drawing.Point(372, 108);
            this.numHyperScrollCapacity.Margin = new System.Windows.Forms.Padding(3, 3, 1, 3);
            this.numHyperScrollCapacity.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numHyperScrollCapacity.Name = "numHyperScrollCapacity";
            this.numHyperScrollCapacity.Size = new System.Drawing.Size(80, 20);
            this.numHyperScrollCapacity.TabIndex = 22;
            this.toolTipMain.SetToolTip(this.numHyperScrollCapacity, "Limit of codes. Recommended: 9-18");
            this.numHyperScrollCapacity.ValueChanged += new System.EventHandler(this.numHyperScrollCapacity_ValueChanged);
            // 
            // chkHyperactiveScroll
            // 
            this.chkHyperactiveScroll.AutoSize = true;
            this.chkHyperactiveScroll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkHyperactiveScroll.Location = new System.Drawing.Point(12, 109);
            this.chkHyperactiveScroll.Name = "chkHyperactiveScroll";
            this.chkHyperactiveScroll.Size = new System.Drawing.Size(236, 17);
            this.chkHyperactiveScroll.TabIndex = 21;
            this.chkHyperactiveScroll.Text = "Hyperactive scroll (Unexplained acceleration)";
            this.chkHyperactiveScroll.UseVisualStyleBackColor = true;
            this.chkHyperactiveScroll.CheckedChanged += new System.EventHandler(this.chkHyperactiveScroll_CheckedChanged);
            // 
            // labelDoubleClick
            // 
            this.labelDoubleClick.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelDoubleClick.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelDoubleClick.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelDoubleClick.Location = new System.Drawing.Point(266, 73);
            this.labelDoubleClick.Name = "labelDoubleClick";
            this.labelDoubleClick.Size = new System.Drawing.Size(100, 23);
            this.labelDoubleClick.TabIndex = 20;
            this.labelDoubleClick.Text = "0";
            this.labelDoubleClick.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelDoubleClick.Click += new System.EventHandler(this.labelDoubleClick_Click);
            // 
            // numDoubleClick
            // 
            this.numDoubleClick.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numDoubleClick.DecimalPlaces = 2;
            this.numDoubleClick.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numDoubleClick.Location = new System.Drawing.Point(372, 76);
            this.numDoubleClick.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numDoubleClick.Name = "numDoubleClick";
            this.numDoubleClick.Size = new System.Drawing.Size(163, 20);
            this.numDoubleClick.TabIndex = 19;
            this.toolTipMain.SetToolTip(this.numDoubleClick, "False positives. Recommended: 80 - 120 ms");
            this.numDoubleClick.ValueChanged += new System.EventHandler(this.numDoubleClick_ValueChanged);
            // 
            // chkDoubleClick
            // 
            this.chkDoubleClick.AutoSize = true;
            this.chkDoubleClick.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkDoubleClick.Location = new System.Drawing.Point(12, 76);
            this.chkDoubleClick.Name = "chkDoubleClick";
            this.chkDoubleClick.Size = new System.Drawing.Size(188, 17);
            this.chkDoubleClick.TabIndex = 18;
            this.chkDoubleClick.Text = "Double click (Classical extra clicks)";
            this.chkDoubleClick.UseVisualStyleBackColor = true;
            this.chkDoubleClick.CheckedChanged += new System.EventHandler(this.chkDoubleClick_CheckedChanged);
            // 
            // labelMixedClicks
            // 
            this.labelMixedClicks.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelMixedClicks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelMixedClicks.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelMixedClicks.Location = new System.Drawing.Point(267, 41);
            this.labelMixedClicks.Name = "labelMixedClicks";
            this.labelMixedClicks.Size = new System.Drawing.Size(99, 23);
            this.labelMixedClicks.TabIndex = 17;
            this.labelMixedClicks.Text = "0";
            this.labelMixedClicks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelMixedClicks.Click += new System.EventHandler(this.labelMixedClicks_Click);
            // 
            // chkMixedClicks
            // 
            this.chkMixedClicks.AutoSize = true;
            this.chkMixedClicks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkMixedClicks.Location = new System.Drawing.Point(12, 44);
            this.chkMixedClicks.Name = "chkMixedClicks";
            this.chkMixedClicks.Size = new System.Drawing.Size(191, 17);
            this.chkMixedClicks.TabIndex = 16;
            this.chkMixedClicks.Text = "Mixed clicks (Non-sequential clicks)";
            this.chkMixedClicks.UseVisualStyleBackColor = true;
            this.chkMixedClicks.CheckedChanged += new System.EventHandler(this.chkMixedClicks_CheckedChanged);
            // 
            // labelInterruptedClick
            // 
            this.labelInterruptedClick.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelInterruptedClick.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelInterruptedClick.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelInterruptedClick.Location = new System.Drawing.Point(267, 9);
            this.labelInterruptedClick.Name = "labelInterruptedClick";
            this.labelInterruptedClick.Size = new System.Drawing.Size(99, 23);
            this.labelInterruptedClick.TabIndex = 15;
            this.labelInterruptedClick.Text = "0";
            this.labelInterruptedClick.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelInterruptedClick.Click += new System.EventHandler(this.labelInterruptedClick_Click);
            // 
            // numInterruptedClick
            // 
            this.numInterruptedClick.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numInterruptedClick.Location = new System.Drawing.Point(372, 12);
            this.numInterruptedClick.Margin = new System.Windows.Forms.Padding(3, 3, 1, 3);
            this.numInterruptedClick.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numInterruptedClick.Name = "numInterruptedClick";
            this.numInterruptedClick.Size = new System.Drawing.Size(56, 20);
            this.numInterruptedClick.TabIndex = 14;
            this.toolTipMain.SetToolTip(this.numInterruptedClick, "Correction time. Recommended: 20 - 200 ms");
            this.numInterruptedClick.ValueChanged += new System.EventHandler(this.numInterruptedClick_ValueChanged);
            // 
            // chkInterruptedClick
            // 
            this.chkInterruptedClick.AutoSize = true;
            this.chkInterruptedClick.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkInterruptedClick.Location = new System.Drawing.Point(12, 12);
            this.chkInterruptedClick.Name = "chkInterruptedClick";
            this.chkInterruptedClick.Size = new System.Drawing.Size(206, 17);
            this.chkInterruptedClick.TabIndex = 13;
            this.chkInterruptedClick.Text = "Interrupted click (Connection recovery)";
            this.chkInterruptedClick.UseVisualStyleBackColor = true;
            this.chkInterruptedClick.CheckedChanged += new System.EventHandler(this.chkInterruptedClick_CheckedChanged);
            // 
            // btnCmd
            // 
            this.btnCmd.Location = new System.Drawing.Point(405, 142);
            this.btnCmd.Name = "btnCmd";
            this.btnCmd.Size = new System.Drawing.Size(41, 23);
            this.btnCmd.TabIndex = 17;
            this.btnCmd.Text = "cmd";
            this.toolTipMain.SetToolTip(this.btnCmd, "Actual arguments from current values to use in command-line");
            this.btnCmd.UseVisualStyleBackColor = true;
            this.btnCmd.Click += new System.EventHandler(this.btnCmd_Click);
            // 
            // TrayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 351);
            this.Controls.Add(this.btnCmd);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelSmallLine);
            this.Controls.Add(this.listBoxDebug);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TrayForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MoConTool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TrayForm_FormClosing);
            this.Load += new System.EventHandler(this.TrayForm_Load);
            this.Resize += new System.EventHandler(this.TrayForm_Resize);
            this.menuTray.ResumeLayout(false);
            this.contextMenuDebug.ResumeLayout(false);
            this.panelSmallLine.ResumeLayout(false);
            this.panelSmallLine.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIntClickDeltaMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIntClickDeltaMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHyperScrollLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHyperScrollCapacity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDoubleClick)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInterruptedClick)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIconMain;
        private System.Windows.Forms.ContextMenuStrip menuTray;
        private System.Windows.Forms.ToolStripMenuItem menuCaption;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuTweaks;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuSrcCode;
        private System.Windows.Forms.ToolStripMenuItem menuAbout;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.CheckBox chkDebug;
        private System.Windows.Forms.ListBox listBoxDebug;
        private System.Windows.Forms.Panel panelSmallLine;
        private System.Windows.Forms.CheckBox chkPlug;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label labelHyperactiveScroll;
        private System.Windows.Forms.NumericUpDown numHyperScrollCapacity;
        private System.Windows.Forms.CheckBox chkHyperactiveScroll;
        private System.Windows.Forms.Label labelDoubleClick;
        private System.Windows.Forms.NumericUpDown numDoubleClick;
        private System.Windows.Forms.CheckBox chkDoubleClick;
        private System.Windows.Forms.Label labelMixedClicks;
        private System.Windows.Forms.CheckBox chkMixedClicks;
        private System.Windows.Forms.Label labelInterruptedClick;
        private System.Windows.Forms.NumericUpDown numInterruptedClick;
        private System.Windows.Forms.CheckBox chkInterruptedClick;
        private System.Windows.Forms.ContextMenuStrip contextMenuDebug;
        private System.Windows.Forms.ToolStripMenuItem menuCopy;
        private System.Windows.Forms.ToolStripMenuItem menuSelectAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuClear;
        private System.Windows.Forms.CheckBox chkDoubleBtnR;
        private System.Windows.Forms.ToolTip toolTipMain;
        private System.Windows.Forms.CheckBox chkDoubleBtnM;
        private System.Windows.Forms.CheckBox chkDoubleBtnL;
        private System.Windows.Forms.CheckBox chkMixedBtnR;
        private System.Windows.Forms.CheckBox chkMixedBtnM;
        private System.Windows.Forms.CheckBox chkMixedBtnL;
        private System.Windows.Forms.CheckBox chkInterruptedBtnR;
        private System.Windows.Forms.CheckBox chkInterruptedBtnM;
        private System.Windows.Forms.CheckBox chkInterruptedBtnL;
        private System.Windows.Forms.NumericUpDown numHyperScrollLimit;
        private System.Windows.Forms.CheckBox chkMixedClicksOnlyDown;
        private System.Windows.Forms.NumericUpDown numIntClickDeltaMin;
        private System.Windows.Forms.NumericUpDown numIntClickDeltaMax;
        private System.Windows.Forms.Button btnCmd;
    }
}

