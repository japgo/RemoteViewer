using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace RemoteViewerServer {
	public partial class FrmMain : Form {
		
		ServerSocket m_server_sock = null;
		private delegate void delegate_CaptureImage( float _scale, bool _save_file = false );
		MemoryStream m_screen_mem = new MemoryStream();
		private static float m_scale = 0.3f;
		private object m_lock = new object();

		private int m_list_selected_idx = 0;
		private TextBox m_edit_TextBox = new TextBox();

		public FrmMain() {
			InitializeComponent();

			ServerStart( 54321 );


			m_edit_TextBox.KeyDown += edit_KeyDown;
			listView1.Controls.Add( m_edit_TextBox );
			m_edit_TextBox.Visible = false;
			m_edit_TextBox.SendToBack();
		}

		void ServerStart( int _port ) {
			m_server_sock?.stop();

			if( m_server_sock == null )
				m_server_sock = new ServerSocket( recv_callback );

			m_server_sock.start( _port );
		}

		public struct stInfo {
			[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 50 )]
			public string pj_name;
			[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 50 )]
			public string owner;
			public byte[] img;
			

			public byte[] getBytes() {
				try {
					int size = 50 + 50 + 102400;
					byte[] arr = new byte[ size ];
					
					Buffer.BlockCopy( this.pj_name.ToArray(), 0, arr, 0, this.pj_name.Length*2 );
					Buffer.BlockCopy( this.owner.ToArray(), 0, arr, 50, this.owner.Length*2 );
					Buffer.BlockCopy( this.img, 0, arr, 100, this.img.Length );

					
					return arr;
				} catch( Exception ex ) {
					throw ex;
				}
			}
		}


		void recv_callback( byte[] _data, int _size ) {
			try {
				if( this.InvokeRequired ) {
					var d = new ServerSocket.recv_callback( recv_callback );
					Invoke( d, _data, _size );
				}else {
					string msg = Encoding.ASCII.GetString( _data, 0, _size );
					if( msg.Contains( "Give me the screen!" ) ) {

						stInfo info = new stInfo();
						info.pj_name = listView1.Items[ 0 ].SubItems[ 1 ].Text;
						info.owner = listView1.Items[ 1 ].SubItems[ 1 ].Text;
						info.img = m_screen_mem.ToArray();
						m_server_sock.send( info.getBytes() );
					}
				}
			}catch(Exception ex ) {
				Console.WriteLine( ex );
			}
		}
		enum eCaptureType {
			eCaptureType_FormDrawToBitmap,
			eCaptureType_Win32_ScreenCapture,
			eCaptureType_Graphics,
			eCaptureType_Direct3D9,
			eCaptureType_Direct3D11,
		}

		public void CaptureImage( float _scale, bool _save_file = false ) {
			lock( m_lock ) {
				try {
					if( this.InvokeRequired ) {
						var d = new delegate_CaptureImage( CaptureImage );
						Invoke( d, _scale, _save_file );
					} else {
						eCaptureType type = eCaptureType.eCaptureType_Graphics;
						switch( type ) {
							case eCaptureType.eCaptureType_FormDrawToBitmap:
								CaptureImage_FormDrawToBitmap( _scale, _save_file );
								break;
							case eCaptureType.eCaptureType_Win32_ScreenCapture:
								CaptureImage_Win32_ScreenCapture( _scale, _save_file );
								break;
							case eCaptureType.eCaptureType_Graphics:
								CaptureImage_Graphics( _scale, _save_file );
								break;
						}
					}
				} catch( Exception ex ) {
					throw ex;
				}
			}
		}


		public void CaptureImage_FormDrawToBitmap( float _scale, bool _save_file = false ) {
			try {
				var frm = Form.ActiveForm;
				using( var bmp = new Bitmap( frm.Width, frm.Height ) ) {
					frm.DrawToBitmap( bmp, new Rectangle( 0, 0, bmp.Width, bmp.Height ) );
					
					Bitmap transform_bmp = new Bitmap( bmp, ( int )( bmp.Width * _scale ), ( int )( bmp.Height * _scale ) );
					transform_bmp.Save( m_screen_mem, ImageFormat.Jpeg );

					if( _save_file ) {
						bmp.Save( "C:\\test.jpeg" );
					}
				}
			} catch( Exception ex ) {
				throw ex;
			}
		}
		public void CaptureImage_Win32_ScreenCapture( float _scale, bool _save_file = false ) {
			try {
				ScreenCapture sc = new ScreenCapture();
				Image img = sc.CaptureScreen();
				
				Bitmap transform_bmp = new Bitmap( img, ( int )( img.Width * _scale ), ( int )( img.Height * _scale ) );
				transform_bmp.Save( m_screen_mem, ImageFormat.Jpeg );

				transform_bmp.Dispose();

				if( _save_file ) {
					FileStream file = new FileStream( "C:\\test.jpeg", FileMode.Create );
					file.Write( m_screen_mem.ToArray(), 0, ( int )m_screen_mem.Length );
					file.Close();
				}
			} catch (Exception ex ) {
				throw ex;
			}
		}

		public void CaptureImage_Graphics( float _scale, bool _save_file = false ) {
			Bitmap origin_bmp = null;
			Graphics origin_grp = null;
			Bitmap transform_bmp = null;
			try {
				origin_bmp = new Bitmap( Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height );
				origin_grp = Graphics.FromImage( origin_bmp );
				origin_grp.CopyFromScreen( new Point( 0, 0 ), new Point( 0, 0 ), new Size( Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height ) );

				using( m_screen_mem = new MemoryStream() )
				{
					transform_bmp = new Bitmap( origin_bmp, ( int )( Screen.PrimaryScreen.Bounds.Width * _scale ), ( int )( Screen.PrimaryScreen.Bounds.Height * _scale ) );
					transform_bmp.Save( m_screen_mem, ImageFormat.Jpeg );
					transform_bmp.Dispose();
					transform_bmp = null;
				}
					
				origin_grp.Dispose();
				origin_grp = null;
				
				origin_bmp.Dispose();
				origin_bmp = null;

				if( _save_file ) {
					FileStream file = new FileStream( "C:\\test.jpeg", FileMode.Create );
					file.Write( m_screen_mem.ToArray(), 0, ( int )m_screen_mem.Length );
					file.Close();
				}
			} catch( Exception ex ) {

				if( transform_bmp != null )
					transform_bmp.Dispose();

				if( origin_grp != null )
					origin_grp.Dispose();

				if( origin_bmp != null )
					origin_bmp.Dispose();

				throw ex;
			}
		}

		private void button1_Click( object sender, EventArgs e ) {
			CaptureImage( m_scale, true );
			if( m_screen_mem == null ) {
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

		private void timer1_Tick( object sender, EventArgs e ) {
			try {
				CaptureImage( m_scale );
			} catch( Exception ) {
			}
		}

		private void editToolStripMenuItem_Click( object sender, EventArgs e ) {
			m_list_selected_idx = listView1.SelectedItems[ 0 ].Index;

			
			Rectangle bounds = listView1.Items[ m_list_selected_idx ].SubItems[ 1 ].Bounds;
			m_edit_TextBox.Text = listView1.Items[ m_list_selected_idx ].SubItems[ 1 ].Text;
			m_edit_TextBox.Location = new Point( bounds.Left, bounds.Top );
			m_edit_TextBox.Size = new Size( bounds.Width, bounds.Height );
			m_edit_TextBox.Visible = true;
			m_edit_TextBox.BringToFront();
			m_edit_TextBox.Focus();
			m_edit_TextBox.SelectAll();
		}

		private void edit_KeyDown( object sender, KeyEventArgs e ) {
			if( e.KeyCode == Keys.Return ) {
				listView1.Items[ m_list_selected_idx ].SubItems[ 1 ].Text = m_edit_TextBox.Text;
				m_edit_TextBox.Visible = false;
				m_edit_TextBox.SendToBack();
			}else if( e.KeyCode == Keys.Escape ) {
				m_edit_TextBox.Visible = false;
				m_edit_TextBox.SendToBack();
			}
		}

		private void listView1_Click( object sender, EventArgs e ) {
			m_edit_TextBox.Visible = false;
			m_edit_TextBox.SendToBack();
		}
	}
}
