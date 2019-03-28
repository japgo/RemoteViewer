namespace RemoteViewerClient {
	partial class FrmMain {
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.remoteScreen1 = new RemoteViewerClient.RemoteScreen();
			this.remoteScreen2 = new RemoteViewerClient.RemoteScreen();
			this.remoteScreen3 = new RemoteViewerClient.RemoteScreen();
			this.remoteScreen4 = new RemoteViewerClient.RemoteScreen();
			this.remoteScreen5 = new RemoteViewerClient.RemoteScreen();
			this.remoteScreen6 = new RemoteViewerClient.RemoteScreen();
			this.remoteScreen7 = new RemoteViewerClient.RemoteScreen();
			this.remoteScreen8 = new RemoteViewerClient.RemoteScreen();
			this.SuspendLayout();
			// 
			// remoteScreen1
			// 
			this.remoteScreen1.Location = new System.Drawing.Point(12, 12);
			this.remoteScreen1.Name = "remoteScreen1";
			this.remoteScreen1.Size = new System.Drawing.Size(345, 320);
			this.remoteScreen1.TabIndex = 2;
			// 
			// remoteScreen2
			// 
			this.remoteScreen2.Location = new System.Drawing.Point(363, 12);
			this.remoteScreen2.Name = "remoteScreen2";
			this.remoteScreen2.Size = new System.Drawing.Size(345, 320);
			this.remoteScreen2.TabIndex = 3;
			// 
			// remoteScreen3
			// 
			this.remoteScreen3.Location = new System.Drawing.Point(714, 12);
			this.remoteScreen3.Name = "remoteScreen3";
			this.remoteScreen3.Size = new System.Drawing.Size(345, 320);
			this.remoteScreen3.TabIndex = 4;
			// 
			// remoteScreen4
			// 
			this.remoteScreen4.Location = new System.Drawing.Point(1065, 12);
			this.remoteScreen4.Name = "remoteScreen4";
			this.remoteScreen4.Size = new System.Drawing.Size(345, 320);
			this.remoteScreen4.TabIndex = 5;
			// 
			// remoteScreen5
			// 
			this.remoteScreen5.Location = new System.Drawing.Point(12, 338);
			this.remoteScreen5.Name = "remoteScreen5";
			this.remoteScreen5.Size = new System.Drawing.Size(345, 320);
			this.remoteScreen5.TabIndex = 6;
			// 
			// remoteScreen6
			// 
			this.remoteScreen6.Location = new System.Drawing.Point(363, 338);
			this.remoteScreen6.Name = "remoteScreen6";
			this.remoteScreen6.Size = new System.Drawing.Size(345, 320);
			this.remoteScreen6.TabIndex = 7;
			// 
			// remoteScreen7
			// 
			this.remoteScreen7.Location = new System.Drawing.Point(714, 338);
			this.remoteScreen7.Name = "remoteScreen7";
			this.remoteScreen7.Size = new System.Drawing.Size(345, 320);
			this.remoteScreen7.TabIndex = 8;
			// 
			// remoteScreen8
			// 
			this.remoteScreen8.Location = new System.Drawing.Point(1065, 338);
			this.remoteScreen8.Name = "remoteScreen8";
			this.remoteScreen8.Size = new System.Drawing.Size(345, 320);
			this.remoteScreen8.TabIndex = 9;
			// 
			// FrmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1420, 665);
			this.Controls.Add(this.remoteScreen8);
			this.Controls.Add(this.remoteScreen7);
			this.Controls.Add(this.remoteScreen6);
			this.Controls.Add(this.remoteScreen5);
			this.Controls.Add(this.remoteScreen4);
			this.Controls.Add(this.remoteScreen3);
			this.Controls.Add(this.remoteScreen2);
			this.Controls.Add(this.remoteScreen1);
			this.Name = "FrmMain";
			this.Text = "Remote Viewer Client";
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Timer timer1;
		private RemoteScreen remoteScreen1;
		private RemoteScreen remoteScreen2;
		private RemoteScreen remoteScreen3;
		private RemoteScreen remoteScreen4;
		private RemoteScreen remoteScreen5;
		private RemoteScreen remoteScreen6;
		private RemoteScreen remoteScreen7;
		private RemoteScreen remoteScreen8;
	}
}

