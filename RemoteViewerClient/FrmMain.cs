using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteViewerClient {
	public partial class FrmMain : Form {

		public FrmMain() {
			InitializeComponent();
		}

		private List<RemoteScreen> m_list_rs = new List<RemoteScreen>();
		private void FrmMain_Load( object sender, EventArgs e ) {
			List<string> ip_list = new List<string>();
			ip_list.Add( "192.168.30.35" );
			ip_list.Add( "192.168.30.36" );
			ip_list.Add( "192.168.30.37" );
			ip_list.Add( "192.168.30.38" );
			ip_list.Add( "192.168.30.39" );
			ip_list.Add( "192.168.30.40" );
			ip_list.Add( "192.168.30.41" );
			ip_list.Add( "192.168.30.42" );
#if DEBUG
			ip_list.Add( "127.0.0.1" );
#endif

			int idx = 1;
			foreach( var ip in ip_list ) {
				RemoteScreen rs = new RemoteScreen();
				rs.Text = string.Format( "Screen{0}", idx );
				rs._IP = ip;
				rs._PORT = 54321;
				rs._IDX = idx++;
				rs.TopLevel = false;

				try {
					rs.connect();
				} catch( Exception ) {

				}

				flowLayoutPanel1.Controls.Add( rs );
				m_list_rs.Add( rs );
				rs.Show();
			}
		}

		private void FrmMain_FormClosing( object sender, FormClosingEventArgs e ) {
			foreach( var rs in m_list_rs ) {
				rs.disconnect();
			}
		}
	}
}
