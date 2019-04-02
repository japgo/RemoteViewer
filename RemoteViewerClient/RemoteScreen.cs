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
using System.Runtime.InteropServices;

namespace RemoteViewerClient {
	public partial class RemoteScreen : Form {

		public string _IP { set; get; }
		public int _PORT { set; get; }
		public int _IDX { set; get; }
		
		private Bitmap m_recv_bmp = null;
		private ClientSocket m_client_sock = null;
		private string m_ProjectName = null;
		private string m_Owner = null;
		private object m_lock = new object();

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

		struct stInfo {
			[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 50 )]
			public string pj_name;
			[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 50 )]
			public string owner;
			public byte[] img;

			public stInfo( byte[] _src ) {
				byte[] pj_name = new byte[ 50 ];
				Buffer.BlockCopy( _src, 0, pj_name, 0, 50 );
				this.pj_name = Encoding.Unicode.GetString( pj_name );

				byte[] owner = new byte[ 50 ];
				Buffer.BlockCopy( _src, 50, owner, 0, 50 );
				this.owner = Encoding.Unicode.GetString( owner );

				byte[] img = new byte[ 102400 ];
				Buffer.BlockCopy( _src, 100, img, 0, _src.Length - 100 );
				this.img = img;				
			}
		}

		private void screen_recv_callback( byte[] _data, int _size ) {
			lock( m_lock ) {
				try {
					stInfo info = new stInfo( _data );
					//display_info( info );
					this.m_ProjectName = info.pj_name;
					this.m_Owner = info.owner;

					MemoryStream mem = new MemoryStream( info.img );
					m_recv_bmp = new Bitmap( mem );
					pbScreen.Image = m_recv_bmp;
				} catch( Exception ex ) {
					Console.WriteLine( ex.Message );
				}
			}
		}

		private delegate void delegate_display_info( stInfo _info );
		private void display_info( stInfo _info ) {
			
			if( lvInfo.InvokeRequired || pbScreen.InvokeRequired ) {
				var d = new delegate_display_info( display_info );
				Invoke( d, _info );
			} else {
				m_ProjectName = _info.pj_name;
				m_Owner = _info.owner;

				MemoryStream mem = new MemoryStream( _info.img );
				m_recv_bmp = new Bitmap( mem );
				pbScreen.Image = m_recv_bmp;
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
			lock( m_lock ) {
				try {
					bool conn_state = false;
					if( m_client_sock != null ) {
						lvInfo.Items[ 0 ].SubItems[ 1 ].Text = m_ProjectName;
						lvInfo.Items[ 1 ].SubItems[ 1 ].Text = m_Owner;

						conn_state = m_client_sock.is_connected();
						lvInfo.Items[ 2 ].UseItemStyleForSubItems = false;
						lvInfo.Items[ 2 ].SubItems[ 1 ].ForeColor = conn_state ? Color.Green : Color.Red;
						lvInfo.Items[ 2 ].SubItems[ 1 ].Text = conn_state ? "Connected" : "Disconnected";
					}

					lvInfo.Items[ 3 ].SubItems[ 1 ].Text = _IP;

					gbTitle.Text = this.Text;

					if( conn_state == true ) {
						m_client_sock?.send( "Give me the screen!" );
					}
				} catch( Exception ex ) {
					Console.WriteLine( ex.Message );
				}
			}
		}
	}
}
