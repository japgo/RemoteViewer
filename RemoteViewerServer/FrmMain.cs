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
using SlimDX;


namespace RemoteViewerServer {
	public partial class FrmMain : Form {

		object m_lock = new object();
		ServerSocket m_server_sock = null;
		private delegate void SafeCallDelegate( float _scale, bool _save_file = false );
		MemoryStream m_screen_mem = null;
		private static float m_scale = 0.3f;

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
			try {
				string msg = Encoding.ASCII.GetString( _data, 0, _size );
				if( msg.Contains( "Give me the screen!" ) ) {

					CaptureImage( m_scale );
					m_server_sock.send( m_screen_mem.ToArray() );

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
			try {
				if( this.InvokeRequired ) {
					var d = new SafeCallDelegate( CaptureImage );
					Invoke( d, _scale, _save_file );
				} else {
					eCaptureType type = eCaptureType.eCaptureType_Win32_ScreenCapture;
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
						case eCaptureType.eCaptureType_Direct3D9:
							CaptrueImage_Direct3D9( _scale, _save_file );
							break;
						case eCaptureType.eCaptureType_Direct3D11:
							CaptrueImage_Direct3D11( _scale, _save_file );
							break;
					}
				}
			} catch( Exception ex) {
				throw ex;
			}
		}


		public void CaptureImage_FormDrawToBitmap( float _scale, bool _save_file = false ) {

			try {
				m_screen_mem = null;

				var frm = Form.ActiveForm;
				using( var bmp = new Bitmap( frm.Width, frm.Height ) ) {
					frm.DrawToBitmap( bmp, new Rectangle( 0, 0, bmp.Width, bmp.Height ) );

					m_screen_mem = new MemoryStream();
					Bitmap transform_bmp = new Bitmap( bmp, ( int )( bmp.Width * _scale ), ( int )( bmp.Height * _scale ) );
					transform_bmp.Save( m_screen_mem, ImageFormat.Jpeg );

					if( _save_file ) {
						bmp.Save( "C:\\test.jpeg" );
					}
				}
			} catch( Exception ex ) {
				m_screen_mem = null;
				throw ex;
			}
		}
		public void CaptureImage_Win32_ScreenCapture( float _scale, bool _save_file = false ) {
			try {
				m_screen_mem = null;

				ScreenCapture sc = new ScreenCapture();
				Image img = sc.CaptureScreen( this.Handle );


				m_screen_mem = new MemoryStream();
				Bitmap transform_bmp = new Bitmap( img, ( int )( img.Width * _scale ), ( int )( img.Height * _scale ) );
				transform_bmp.Save( m_screen_mem, ImageFormat.Jpeg );

				if( _save_file ) {
					FileStream file = new FileStream( "C:\\test.jpeg", FileMode.Create );
					file.Write( m_screen_mem.ToArray(), 0, ( int )m_screen_mem.Length );
					file.Close();
				}

				

			} catch (Exception ex ) {
				m_screen_mem = null;
				throw ex;
			}
		}

		public void CaptureImage_Graphics( float _scale, bool _save_file = false ) {
			lock( m_lock ) {
				try {
					m_screen_mem = null;

					Bitmap origin_bmp = new Bitmap( Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height );
					Graphics origin_grp = Graphics.FromImage( origin_bmp );
					origin_grp.CopyFromScreen( new Point( 0, 0 ), new Point( 0, 0 ), new Size( Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height ), CopyPixelOperation.Whiteness );

					m_screen_mem = new MemoryStream();
					Bitmap transform_bmp = new Bitmap( origin_bmp, ( int )( Screen.PrimaryScreen.Bounds.Width * _scale ), ( int )( Screen.PrimaryScreen.Bounds.Height * _scale ) );
					transform_bmp.Save( m_screen_mem, ImageFormat.Jpeg );

					origin_grp.Dispose();
					origin_grp = null;

					if( _save_file ) {
						FileStream file = new FileStream( "C:\\test.jpeg", FileMode.Create );
						file.Write( m_screen_mem.ToArray(), 0, ( int )m_screen_mem.Length );
						file.Close();
					}
				} catch( Exception ex ) {
					m_screen_mem = null;
					throw ex;
				}
			}
		}

		public void CaptrueImage_Direct3D9( float _scale, bool _save_file = false ) {

			try {
				m_screen_mem = null;

				IntPtr hWnd = this.Handle;
				Bitmap bitmap = null;
				SlimDX.Direct3D9.Direct3D _direct3D9 = new SlimDX.Direct3D9.Direct3D();
				SlimDX.Direct3D9.AdapterInformation adapterInfo = _direct3D9.Adapters.DefaultAdapter;
				SlimDX.Direct3D9.Device device;

				SlimDX.Direct3D9.PresentParameters parameters = new SlimDX.Direct3D9.PresentParameters();
				parameters.BackBufferFormat = adapterInfo.CurrentDisplayMode.Format;
				Rectangle clientRect = Screen.PrimaryScreen.Bounds;
				parameters.BackBufferHeight = clientRect.Height;
				parameters.BackBufferWidth = clientRect.Width;
				parameters.Multisample = SlimDX.Direct3D9.MultisampleType.None;
				parameters.SwapEffect = SlimDX.Direct3D9.SwapEffect.Discard;
				parameters.DeviceWindowHandle = hWnd;
				parameters.PresentationInterval = SlimDX.Direct3D9.PresentInterval.Default;
				parameters.FullScreenRefreshRateInHertz = 0;

				device = new SlimDX.Direct3D9.Device( _direct3D9, adapterInfo.Adapter, SlimDX.Direct3D9.DeviceType.Hardware, hWnd, SlimDX.Direct3D9.CreateFlags.SoftwareVertexProcessing, parameters );

				// Capture the screen and copy the region into a Bitmap
				using( SlimDX.Direct3D9.Surface surface = SlimDX.Direct3D9.Surface.CreateOffscreenPlain( device, adapterInfo.CurrentDisplayMode.Width, adapterInfo.CurrentDisplayMode.Height, SlimDX.Direct3D9.Format.A8R8G8B8, SlimDX.Direct3D9.Pool.SystemMemory ) ) {
					device.GetFrontBufferData( 0, surface );
					bitmap = new Bitmap( SlimDX.Direct3D9.Surface.ToStream( surface, SlimDX.Direct3D9.ImageFileFormat.Bmp, new Rectangle( clientRect.Left, clientRect.Top, clientRect.Width, clientRect.Height ) ) );
				}

				m_screen_mem = new MemoryStream();
				Bitmap transform_bmp = new Bitmap( bitmap, ( int )( Screen.PrimaryScreen.Bounds.Width * _scale ), ( int )( Screen.PrimaryScreen.Bounds.Height * _scale ) );
				transform_bmp.Save( m_screen_mem, ImageFormat.Jpeg );

			} catch( Exception ex ) {
				m_screen_mem = null;
				throw ex;
			}
		}

		public void CaptrueImage_Direct3D11( float _scale, bool _save_file = false ) {

			try {
				m_screen_mem = null;

				IntPtr hWnd = this.Handle;
				Bitmap bitmap = null;

			} catch( Exception ex ) {
				m_screen_mem = null;
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
			Random rand = new Random();
			int num = rand.Next( 10 );
			string str = "";
			for( int i = 0; i < num; i++ )
				str += " ";
			str += "*";

			listBox1.Items.Add( str );
			listBox1.SelectedIndex = listBox1.Items.Count - 1;
		}
	}
}
