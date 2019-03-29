using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteViewerServer {
	public partial class FrmMain : Form {

		object m_lock = new object();
		ServerSocket m_server_sock = null;

		public FrmMain() {
			InitializeComponent();

			ServerStart( 54321 );
		}

		void ServerStart( int _port ) {
			m_server_sock?.stop();

			if( m_server_sock == null )
				m_server_sock = new ServerSocket( recv_callback );

			m_server_sock.start( _port );
		}
		void recv_callback( byte[] _data, int _size ) {
			string msg = Encoding.ASCII.GetString( _data, 0, _size );
			if( msg.Contains( "Give me the screen!" ) ) { 
				MemoryStream screen_mem = CaptureImage( 0.3f );
				m_server_sock.send( screen_mem.ToArray() );
			}
		}

		public MemoryStream CaptureImage( float _scale, bool _save_file = false ) {

			lock( m_lock ) {
				try {
					MemoryStream screen_mem = new MemoryStream();

					Bitmap origin_bmp = new Bitmap( Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height );
					Graphics origin_grp = Graphics.FromImage( origin_bmp );
					origin_grp.CopyFromScreen( new Point( 0, 0 ), new Point( 0, 0 ), new Size( Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height ) );

					Bitmap transform_bmp = new Bitmap( origin_bmp, ( int )( Screen.PrimaryScreen.Bounds.Width * _scale ), ( int )( Screen.PrimaryScreen.Bounds.Height * _scale ) );
					transform_bmp.Save( screen_mem, ImageFormat.Jpeg );

					origin_grp.Dispose();
					origin_grp = null;

					if( _save_file ) {
						FileStream file = new FileStream( "C:\\test.jpeg", FileMode.Create );
						file.Write( screen_mem.ToArray(), 0, ( int )screen_mem.Length );
						file.Close();
					}

					return screen_mem;
				} catch( Exception e ) {
					Console.WriteLine( e );
					return null;
				}
			}
		}

		private void button1_Click( object sender, EventArgs e ) {
			MemoryStream success = CaptureImage( 0.2f, true );
			if( success == null ) {
				MessageBox.Show( "Capture Fail" );
			} else {
				MessageBox.Show( "Capture Success ( path = C:\\test.jpeg )" );
			}
		}

		private void FrmMain_FormClosing( object sender, FormClosingEventArgs e ) {
			e.Cancel = true;
			this.Hide();
		}
		
		private void exitToolStripMenuItem_Click( object sender, EventArgs e ) {
			m_server_sock.stop();
			this.Dispose();
		}

		private void notifyIcon1_DoubleClick( object sender, EventArgs e ) {
			this.Show();
		}

		private void FrmMain_Shown( object sender, EventArgs e ) {
			this.Hide();
		}

		private void btnServerStart_Click( object sender, EventArgs e ) {
			ServerStart( Convert.ToInt32( tbPort.Text ) );
		}
	}
}
