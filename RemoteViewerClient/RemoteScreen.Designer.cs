namespace RemoteViewerClient {
	partial class RemoteScreen {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing ) {
			if( disposing && ( components != null ) ) {
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Project Name",
            "TW-TT101"}, -1);
			System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "Owner",
            "Tester"}, -1);
			System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new System.Windows.Forms.ListViewItem.ListViewSubItem[] {
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "Link State"),
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "Disconnected", System.Drawing.Color.Red, System.Drawing.SystemColors.Window, new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129))))}, -1);
			System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "IP Address",
            "127.0.0.1"}, -1);
			this.gbTitle = new System.Windows.Forms.GroupBox();
			this.pbScreen = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.btnDisconnect = new System.Windows.Forms.Button();
			this.btnConnect = new System.Windows.Forms.Button();
			this.lvInfo = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.gbTitle.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbScreen)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbTitle
			// 
			this.gbTitle.Controls.Add(this.pbScreen);
			this.gbTitle.Controls.Add(this.tableLayoutPanel1);
			this.gbTitle.Controls.Add(this.lvInfo);
			this.gbTitle.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gbTitle.Location = new System.Drawing.Point(0, 0);
			this.gbTitle.Name = "gbTitle";
			this.gbTitle.Size = new System.Drawing.Size(376, 322);
			this.gbTitle.TabIndex = 1;
			this.gbTitle.TabStop = false;
			this.gbTitle.Text = "Screen1";
			// 
			// pbScreen
			// 
			this.pbScreen.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pbScreen.Location = new System.Drawing.Point(3, 85);
			this.pbScreen.Name = "pbScreen";
			this.pbScreen.Size = new System.Drawing.Size(370, 201);
			this.pbScreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pbScreen.TabIndex = 2;
			this.pbScreen.TabStop = false;
			this.pbScreen.Click += new System.EventHandler(this.pbScreen_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.btnDisconnect, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnConnect, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 286);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(370, 33);
			this.tableLayoutPanel1.TabIndex = 3;
			// 
			// btnDisconnect
			// 
			this.btnDisconnect.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnDisconnect.Location = new System.Drawing.Point(188, 3);
			this.btnDisconnect.Name = "btnDisconnect";
			this.btnDisconnect.Size = new System.Drawing.Size(179, 27);
			this.btnDisconnect.TabIndex = 1;
			this.btnDisconnect.Text = "Disconnect";
			this.btnDisconnect.UseVisualStyleBackColor = true;
			this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
			// 
			// btnConnect
			// 
			this.btnConnect.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnConnect.Location = new System.Drawing.Point(3, 3);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(179, 27);
			this.btnConnect.TabIndex = 0;
			this.btnConnect.Text = "Connect";
			this.btnConnect.UseVisualStyleBackColor = true;
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// lvInfo
			// 
			this.lvInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
			this.lvInfo.Dock = System.Windows.Forms.DockStyle.Top;
			this.lvInfo.GridLines = true;
			this.lvInfo.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			listViewItem1.StateImageIndex = 0;
			listViewItem3.StateImageIndex = 0;
			listViewItem3.UseItemStyleForSubItems = false;
			this.lvInfo.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4});
			this.lvInfo.Location = new System.Drawing.Point(3, 17);
			this.lvInfo.MultiSelect = false;
			this.lvInfo.Name = "lvInfo";
			this.lvInfo.Scrollable = false;
			this.lvInfo.Size = new System.Drawing.Size(370, 68);
			this.lvInfo.TabIndex = 1;
			this.lvInfo.UseCompatibleStateImageBehavior = false;
			this.lvInfo.View = System.Windows.Forms.View.Details;
			this.lvInfo.VirtualListSize = 1;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "ColumnHeader1";
			this.columnHeader1.Width = 114;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Width = 264;
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 500;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// RemoteScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(376, 322);
			this.Controls.Add(this.gbTitle);
			this.DoubleBuffered = true;
			this.Name = "RemoteScreen";
			this.gbTitle.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pbScreen)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gbTitle;
		private System.Windows.Forms.ListView lvInfo;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.PictureBox pbScreen;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button btnDisconnect;
		private System.Windows.Forms.Button btnConnect;
		private System.Windows.Forms.Timer timer1;
	}
}
