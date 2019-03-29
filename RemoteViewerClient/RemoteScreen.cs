using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static RemoteViewerClient.ClientSocket;

namespace RemoteViewerClient {
	public partial class RemoteScreen : Form {

		public string _IP { set; get; }
		public int _PORT { set; get; }
		public int _IDX { set; get; }
		
		private Bitmap m_recv_bmp = null;
		private ClientSocket m_client_sock = null;
		public RemoteScreen() {
			InitializeComponent();
		}

		
		public void connect() {
			try {
				if( m_client_sock?.is_connected() == true )
					m_client_sock?.disconnect();
				
				m_client_sock = new ClientSocket( screen_recv_callback );

				if( m_client_sock?.is_connected() == false )
					m_client_sock.connect( _IP, _PORT );
			} catch( Exception ex ) {
				throw ex;
			}
		}

		public void disconnect() {
			try {
				m_client_sock?.disconnect();
			} catch( Exception ex ) {
				throw ex;
			}
		}

		private void btnConnect_Click( object sender, EventArgs e ) {
			try {
				connect();
			} catch( Exception ex ) {
				MessageBox.Show( ex.Message );
			}
		}

		private void btnDisconnect_Click( object sender, EventArgs e ) {
			try {
				disconnect();
			} catch(Exception ex ) {
				MessageBox.Show( ex.Message );
			}
		}

		private void screen_recv_callback( byte[] _data, int _size ) {
			try {
				MemoryStream mem = new MemoryStream( _data );
				m_recv_bmp = new Bitmap( mem );
				pbScreen.Image = m_recv_bmp;
			} catch( Exception ex ) {
				Console.WriteLine( ex.Message );
			}
		}

		private void pbScreen_Click( object sender, EventArgs e ) {
			try {
				m_client_sock?.send( "Give me the screen!" );
			} catch( Exception ex ) {
				Console.WriteLine( ex.Message );
			}
		}

		private void timer1_Tick( object sender, EventArgs e ) {
			try {
				bool conn_state = false;
				if( m_client_sock != null ) {
					conn_state = m_client_sock.is_connected();
					lvInfo.Items[ 2 ].UseItemStyleForSubItems = false;
					lvInfo.Items[ 2 ].SubItems[ 1 ].ForeColor = conn_state ? Color.Green : Color.Red;
					lvInfo.Items[ 2 ].SubItems[ 1 ].Text = conn_state ? "Connected" : "Disconnected";
				}

				lvInfo.Items[ 3 ].SubItems[ 1 ].Text = _IP;

				if( conn_state == true ) 
					m_client_sock?.send( "Give me the screen!" );
				
			} catch( Exception ex ) {
				Console.WriteLine( ex.Message );
			}
		}
	}
}
