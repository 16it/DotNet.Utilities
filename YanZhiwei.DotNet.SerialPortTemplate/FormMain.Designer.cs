namespace YanZhiwei.DotNet.SerialPortTemplate
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("单灯远程升级指令");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("功能列表", new System.Windows.Forms.TreeNode[]
            {
                treeNode1
            });
            this.menuFunction = new System.Windows.Forms.MenuStrip();
            this.btnItemFunctionList = new System.Windows.Forms.ToolStripMenuItem();
            this.btnItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.spMain = new System.Windows.Forms.SplitContainer();
            this.panelSerilportParm = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.btnSerilportOpt = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.cbSeriportParitys = new System.Windows.Forms.ComboBox();
            this.kryptonLabel4 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.cbSeriportStopBits = new System.Windows.Forms.ComboBox();
            this.kryptonLabel3 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.cbSeriportBaudRates = new System.Windows.Forms.ComboBox();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.cbSeriportDataBits = new System.Windows.Forms.ComboBox();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.cbSeriportName = new System.Windows.Forms.ComboBox();
            this.lblSeriportName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.spFunction = new System.Windows.Forms.SplitContainer();
            this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.treeFunctionList = new ComponentFactory.Krypton.Toolkit.KryptonTreeView();
            this.spFunDetail = new System.Windows.Forms.SplitContainer();
            this.kryptonPanel2 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.tabFunctionList = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.kryptonPanel4 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.kryptonPanel3 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.listLog = new ComponentFactory.Krypton.Toolkit.KryptonListBox();
            this.serialPort = new System.IO.Ports.SerialPort(this.components);
            this.menuFunction.SuspendLayout();
            this.spMain.Panel1.SuspendLayout();
            this.spMain.Panel2.SuspendLayout();
            this.spMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelSerilportParm)).BeginInit();
            this.panelSerilportParm.SuspendLayout();
            this.spFunction.Panel1.SuspendLayout();
            this.spFunction.Panel2.SuspendLayout();
            this.spFunction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.spFunDetail.Panel1.SuspendLayout();
            this.spFunDetail.Panel2.SuspendLayout();
            this.spFunDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).BeginInit();
            this.kryptonPanel2.SuspendLayout();
            this.tabFunctionList.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel3)).BeginInit();
            this.kryptonPanel3.SuspendLayout();
            this.SuspendLayout();
            //
            // menuFunction
            //
            this.menuFunction.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuFunction.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                this.btnItemFunctionList,
                this.btnItemAbout
            });
            this.menuFunction.Location = new System.Drawing.Point(0, 0);
            this.menuFunction.Name = "menuFunction";
            this.menuFunction.Size = new System.Drawing.Size(784, 24);
            this.menuFunction.TabIndex = 1;
            this.menuFunction.Text = "menuStrip1";
            //
            // btnItemFunctionList
            //
            this.btnItemFunctionList.Name = "btnItemFunctionList";
            this.btnItemFunctionList.Size = new System.Drawing.Size(45, 20);
            this.btnItemFunctionList.Text = "功能";
            //
            // btnItemAbout
            //
            this.btnItemAbout.Name = "btnItemAbout";
            this.btnItemAbout.Size = new System.Drawing.Size(45, 20);
            this.btnItemAbout.Text = "关于";
            //
            // spMain
            //
            this.spMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.spMain.Location = new System.Drawing.Point(0, 24);
            this.spMain.Name = "spMain";
            this.spMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            //
            // spMain.Panel1
            //
            this.spMain.Panel1.Controls.Add(this.panelSerilportParm);
            this.spMain.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            //
            // spMain.Panel2
            //
            this.spMain.Panel2.Controls.Add(this.spFunction);
            this.spMain.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.spMain.Size = new System.Drawing.Size(784, 537);
            this.spMain.SplitterDistance = 32;
            this.spMain.TabIndex = 2;
            //
            // panelSerilportParm
            //
            this.panelSerilportParm.Controls.Add(this.btnSerilportOpt);
            this.panelSerilportParm.Controls.Add(this.cbSeriportParitys);
            this.panelSerilportParm.Controls.Add(this.kryptonLabel4);
            this.panelSerilportParm.Controls.Add(this.cbSeriportStopBits);
            this.panelSerilportParm.Controls.Add(this.kryptonLabel3);
            this.panelSerilportParm.Controls.Add(this.cbSeriportBaudRates);
            this.panelSerilportParm.Controls.Add(this.kryptonLabel2);
            this.panelSerilportParm.Controls.Add(this.cbSeriportDataBits);
            this.panelSerilportParm.Controls.Add(this.kryptonLabel1);
            this.panelSerilportParm.Controls.Add(this.cbSeriportName);
            this.panelSerilportParm.Controls.Add(this.lblSeriportName);
            this.panelSerilportParm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSerilportParm.Location = new System.Drawing.Point(0, 0);
            this.panelSerilportParm.Name = "panelSerilportParm";
            this.panelSerilportParm.Size = new System.Drawing.Size(784, 32);
            this.panelSerilportParm.TabIndex = 0;
            //
            // btnSerilportOpt
            //
            this.btnSerilportOpt.Location = new System.Drawing.Point(683, 3);
            this.btnSerilportOpt.Name = "btnSerilportOpt";
            this.btnSerilportOpt.Size = new System.Drawing.Size(90, 25);
            this.btnSerilportOpt.TabIndex = 10;
            this.btnSerilportOpt.Tag = "1";
            this.btnSerilportOpt.Values.Text = "打开串口";
            this.btnSerilportOpt.Click += new System.EventHandler(this.btnSerilportOpt_Click);
            //
            // cbSeriportParitys
            //
            this.cbSeriportParitys.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSeriportParitys.FormattingEnabled = true;
            this.cbSeriportParitys.Location = new System.Drawing.Point(615, 3);
            this.cbSeriportParitys.Name = "cbSeriportParitys";
            this.cbSeriportParitys.Size = new System.Drawing.Size(62, 20);
            this.cbSeriportParitys.TabIndex = 9;
            //
            // kryptonLabel4
            //
            this.kryptonLabel4.Location = new System.Drawing.Point(570, 3);
            this.kryptonLabel4.Name = "kryptonLabel4";
            this.kryptonLabel4.Size = new System.Drawing.Size(39, 20);
            this.kryptonLabel4.TabIndex = 8;
            this.kryptonLabel4.Values.Text = "校验:";
            //
            // cbSeriportStopBits
            //
            this.cbSeriportStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSeriportStopBits.FormattingEnabled = true;
            this.cbSeriportStopBits.Location = new System.Drawing.Point(502, 3);
            this.cbSeriportStopBits.Name = "cbSeriportStopBits";
            this.cbSeriportStopBits.Size = new System.Drawing.Size(62, 20);
            this.cbSeriportStopBits.TabIndex = 7;
            //
            // kryptonLabel3
            //
            this.kryptonLabel3.Location = new System.Drawing.Point(444, 3);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(52, 20);
            this.kryptonLabel3.TabIndex = 6;
            this.kryptonLabel3.Values.Text = "停止位:";
            //
            // cbSeriportBaudRates
            //
            this.cbSeriportBaudRates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSeriportBaudRates.FormattingEnabled = true;
            this.cbSeriportBaudRates.Location = new System.Drawing.Point(376, 3);
            this.cbSeriportBaudRates.Name = "cbSeriportBaudRates";
            this.cbSeriportBaudRates.Size = new System.Drawing.Size(62, 20);
            this.cbSeriportBaudRates.TabIndex = 5;
            //
            // kryptonLabel2
            //
            this.kryptonLabel2.Location = new System.Drawing.Point(318, 3);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(52, 20);
            this.kryptonLabel2.TabIndex = 4;
            this.kryptonLabel2.Values.Text = "波特率:";
            //
            // cbSeriportDataBits
            //
            this.cbSeriportDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSeriportDataBits.FormattingEnabled = true;
            this.cbSeriportDataBits.Location = new System.Drawing.Point(242, 3);
            this.cbSeriportDataBits.Name = "cbSeriportDataBits";
            this.cbSeriportDataBits.Size = new System.Drawing.Size(62, 20);
            this.cbSeriportDataBits.TabIndex = 3;
            //
            // kryptonLabel1
            //
            this.kryptonLabel1.Location = new System.Drawing.Point(184, 3);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(52, 20);
            this.kryptonLabel1.TabIndex = 2;
            this.kryptonLabel1.Values.Text = "数据位:";
            //
            // cbSeriportName
            //
            this.cbSeriportName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSeriportName.FormattingEnabled = true;
            this.cbSeriportName.Location = new System.Drawing.Point(57, 3);
            this.cbSeriportName.Name = "cbSeriportName";
            this.cbSeriportName.Size = new System.Drawing.Size(121, 20);
            this.cbSeriportName.TabIndex = 1;
            //
            // lblSeriportName
            //
            this.lblSeriportName.Location = new System.Drawing.Point(12, 3);
            this.lblSeriportName.Name = "lblSeriportName";
            this.lblSeriportName.Size = new System.Drawing.Size(39, 20);
            this.lblSeriportName.TabIndex = 0;
            this.lblSeriportName.Values.Text = "串口:";
            //
            // spFunction
            //
            this.spFunction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spFunction.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.spFunction.Location = new System.Drawing.Point(0, 0);
            this.spFunction.Name = "spFunction";
            //
            // spFunction.Panel1
            //
            this.spFunction.Panel1.Controls.Add(this.kryptonPanel1);
            //
            // spFunction.Panel2
            //
            this.spFunction.Panel2.Controls.Add(this.spFunDetail);
            this.spFunction.Size = new System.Drawing.Size(784, 501);
            this.spFunction.SplitterDistance = 158;
            this.spFunction.TabIndex = 0;
            //
            // kryptonPanel1
            //
            this.kryptonPanel1.Controls.Add(this.treeFunctionList);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(158, 501);
            this.kryptonPanel1.TabIndex = 0;
            //
            // treeFunctionList
            //
            this.treeFunctionList.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.InputControlStandalone;
            this.treeFunctionList.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.InputControlStandalone;
            this.treeFunctionList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeFunctionList.ItemHeight = 21;
            this.treeFunctionList.ItemStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.ListItem;
            this.treeFunctionList.Location = new System.Drawing.Point(0, 0);
            this.treeFunctionList.Name = "treeFunctionList";
            treeNode1.Name = "1";
            treeNode1.Text = "单灯远程升级指令";
            treeNode2.Name = "0";
            treeNode2.Text = "功能列表";
            this.treeFunctionList.Nodes.AddRange(new System.Windows.Forms.TreeNode[]
            {
                treeNode2
            });
            this.treeFunctionList.Size = new System.Drawing.Size(158, 501);
            this.treeFunctionList.Sorted = true;
            this.treeFunctionList.TabIndex = 0;
            this.treeFunctionList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeFunctionList_AfterSelect);
            //
            // spFunDetail
            //
            this.spFunDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spFunDetail.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.spFunDetail.Location = new System.Drawing.Point(0, 0);
            this.spFunDetail.Name = "spFunDetail";
            this.spFunDetail.Orientation = System.Windows.Forms.Orientation.Horizontal;
            //
            // spFunDetail.Panel1
            //
            this.spFunDetail.Panel1.Controls.Add(this.kryptonPanel2);
            //
            // spFunDetail.Panel2
            //
            this.spFunDetail.Panel2.Controls.Add(this.kryptonPanel3);
            this.spFunDetail.Size = new System.Drawing.Size(622, 501);
            this.spFunDetail.SplitterDistance = 384;
            this.spFunDetail.TabIndex = 0;
            //
            // kryptonPanel2
            //
            this.kryptonPanel2.Controls.Add(this.tabFunctionList);
            this.kryptonPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel2.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel2.Name = "kryptonPanel2";
            this.kryptonPanel2.Size = new System.Drawing.Size(622, 384);
            this.kryptonPanel2.TabIndex = 0;
            //
            // tabFunctionList
            //
            this.tabFunctionList.Controls.Add(this.tabPage1);
            this.tabFunctionList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabFunctionList.Location = new System.Drawing.Point(0, 0);
            this.tabFunctionList.Name = "tabFunctionList";
            this.tabFunctionList.SelectedIndex = 0;
            this.tabFunctionList.Size = new System.Drawing.Size(622, 384);
            this.tabFunctionList.TabIndex = 0;
            //
            // tabPage1
            //
            this.tabPage1.Controls.Add(this.kryptonPanel4);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(614, 358);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "单灯远程升级指令";
            this.tabPage1.UseVisualStyleBackColor = true;
            //
            // kryptonPanel4
            //
            this.kryptonPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel4.Location = new System.Drawing.Point(3, 3);
            this.kryptonPanel4.Name = "kryptonPanel4";
            this.kryptonPanel4.Size = new System.Drawing.Size(608, 352);
            this.kryptonPanel4.TabIndex = 0;
            //
            // kryptonPanel3
            //
            this.kryptonPanel3.Controls.Add(this.listLog);
            this.kryptonPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel3.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel3.Name = "kryptonPanel3";
            this.kryptonPanel3.Size = new System.Drawing.Size(622, 113);
            this.kryptonPanel3.TabIndex = 0;
            //
            // listLog
            //
            this.listLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listLog.Location = new System.Drawing.Point(0, 0);
            this.listLog.Name = "listLog";
            this.listLog.Size = new System.Drawing.Size(622, 113);
            this.listLog.TabIndex = 0;
            //
            // serialPort
            //
            this.serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort_DataReceived);
            //
            // FormMain
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.spMain);
            this.Controls.Add(this.menuFunction);
            this.MaximumSize = new System.Drawing.Size(800, 600);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DotNet.SerialPortTemplate";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuFunction.ResumeLayout(false);
            this.menuFunction.PerformLayout();
            this.spMain.Panel1.ResumeLayout(false);
            this.spMain.Panel2.ResumeLayout(false);
            this.spMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelSerilportParm)).EndInit();
            this.panelSerilportParm.ResumeLayout(false);
            this.panelSerilportParm.PerformLayout();
            this.spFunction.Panel1.ResumeLayout(false);
            this.spFunction.Panel2.ResumeLayout(false);
            this.spFunction.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.spFunDetail.Panel1.ResumeLayout(false);
            this.spFunDetail.Panel2.ResumeLayout(false);
            this.spFunDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).EndInit();
            this.kryptonPanel2.ResumeLayout(false);
            this.tabFunctionList.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel3)).EndInit();
            this.kryptonPanel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuFunction;
        private System.Windows.Forms.ToolStripMenuItem btnItemFunctionList;
        private System.Windows.Forms.ToolStripMenuItem btnItemAbout;
        private System.Windows.Forms.SplitContainer spMain;
        private System.Windows.Forms.SplitContainer spFunction;
        private System.Windows.Forms.SplitContainer spFunDetail;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel panelSerilportParm;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSeriportName;
        private System.Windows.Forms.ComboBox cbSeriportName;
        private System.Windows.Forms.ComboBox cbSeriportDataBits;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private System.Windows.Forms.ComboBox cbSeriportBaudRates;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private System.Windows.Forms.ComboBox cbSeriportStopBits;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private System.Windows.Forms.ComboBox cbSeriportParitys;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel4;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSerilportOpt;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel2;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel3;
        private ComponentFactory.Krypton.Toolkit.KryptonTreeView treeFunctionList;
        private ComponentFactory.Krypton.Toolkit.KryptonListBox listLog;
        private System.Windows.Forms.TabControl tabFunctionList;
        private System.Windows.Forms.TabPage tabPage1;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel4;
        private System.IO.Ports.SerialPort serialPort;
    }
}

