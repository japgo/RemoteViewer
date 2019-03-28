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

namespace RemoteViewerClient {
	public partial class RemoteScreen : UserControl {

		ClientSocket m_client_sock = null;
		object m_lock = new object();

		public RemoteScreen() {
			InitializeComponent();
		}

		private void RemoteScreen_Load( object sender, EventArgs e ) {

		}

		private void btnConnect_Click( object sender, EventArgs e ) {
			try {
				if( m_client_sock?.is_connected() == true )
					m_client_sock?.disconnect();

				if( m_client_sock == null )
					m_client_sock = new ClientSocket( recv_callback );

				if( m_client_sock?.is_connected() == false )
					m_client_sock.connect( "127.0.0.1", 54321 );
			} catch( Exception ex ) {
				MessageBox.Show( ex.Message );
			}
		}

		private void btnDisconnect_Click( object sender, EventArgs e ) {
			try {
				m_client_sock?.disconnect();
			} catch( Exception ex ) {
				MessageBox.Show( ex.Message );
			}
		}

		void recv_callback( byte[] _data, int _size ) {
			try {
				MemoryStream mem = new MemoryStream( _data );
				Bitmap recv_bmp = new Bitmap( mem );

				pbScreen.Image = recv_bmp;
			} catch( Exception ) {

			}			
		}

		private void pbScreen_Click( object sender, EventArgs e ) {
			try {
				m_client_sock?.send( "Give me the screen!" );
			}catch( Exception ) {

			}			
		}

		private void timer1_Tick( object sender, EventArgs e ) {
			try {
				bool conn_state = m_client_sock.is_connected();

				lvInfo.Items[ 2 ].SubItems[ 1 ].Text = conn_state ? "Connected" : "Disconnected";

				m_client_sock?.send( "Give me the screen!" );
			} catch( Exception ) {

			}
		}
	}
}
