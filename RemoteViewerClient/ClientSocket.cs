using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteViewerClient {
	class ClientSocket {

		string m_ip = null;
		int m_port = 0;
		private Thread m_conn_thread = null;
		private Thread m_recv_thread = null;
		private TcpClient m_tc = null;
		public delegate void recv_callback( byte[] _data, int _size );
		private recv_callback m_recv_callback;
		private bool m_thread_stop_flag = false;

		public ClientSocket( recv_callback _recv_callback = null ) {
			m_recv_callback = _recv_callback;
		}


		public void connect( string _ip, int _port ) {
			try {
				disconnect();

				m_ip = _ip;
				m_port = _port;

				m_conn_thread = new Thread( new ParameterizedThreadStart( thread_proc_connect ) );
				m_conn_thread.Start( this );
			}
			catch( Exception ex ) {
				Console.WriteLine( ex.Message );
				throw ex;
			}			
		}

		private static void thread_proc_connect( object _obj ) {
			try {
				ClientSocket cs = ( ClientSocket )_obj;
				
				cs.m_tc = new TcpClient();
				IAsyncResult ar = cs.m_tc.BeginConnect( cs.m_ip, cs.m_port, fnAsyncCallback, _obj );
				cs.m_tc.EndConnect( ar );
			} catch( Exception ex ) {
				Console.WriteLine( ex.Message );
			}
		}

		static void fnAsyncCallback( IAsyncResult ar ) {
			try {
				ClientSocket cs = ( ClientSocket )ar.AsyncState;

				if( cs.m_tc.Connected == true ) {
					cs.m_thread_stop_flag = false;
					cs.m_recv_thread = new Thread( new ParameterizedThreadStart( thread_proc_on_recv ) );
					cs.m_recv_thread.Name = string.Format( "ClientSocket_RecvThread_{0}", ( ( RemoteViewerClient.RemoteScreen )cs.m_recv_callback.Target )._IDX );
					cs.m_recv_thread.Start( cs );
				}
			} catch( Exception ) {

			}
		}

		public void disconnect() {
			m_thread_stop_flag = true;
			
			if( m_tc != null ) {
				if( m_tc.Connected == true )
					m_tc.Client.Disconnect( false );
				
				m_tc.Close();
				m_tc.Dispose();
			}
			m_recv_thread?.Join();
			m_recv_thread?.Abort();
			
			m_conn_thread?.Join();
			m_conn_thread?.Abort();
		}

		public bool is_connected() {
			if( m_tc == null )
				return false;

			return m_tc.Connected;
		}

		private static void thread_proc_on_recv( object _obj ) {
			ClientSocket cs = ( ClientSocket )_obj;
			NetworkStream stream = null;
			while( true ) {
				try {
					Thread.Sleep( 10 );

					if( cs.m_thread_stop_flag == true )
						break;

					stream = cs.m_tc.GetStream();

					if( stream.CanRead && cs.m_tc.Connected ) {
						byte[] bytes = new byte[ 102500 ];
						int read_cnt = stream.Read( bytes, 0, bytes.Length );
						if( read_cnt > 0)
							cs.m_recv_callback?.Invoke( bytes, bytes.Length );
					} else {
						cs.m_tc.Close();
						stream.Close();
					}
					
				} catch( Exception ex ) {
					cs.m_tc.Close();
					stream.Close();
					Console.WriteLine( ex.Message );
					break;
				}
			}
		}
		public void send( string _msg ) {
			try {
				m_tc.GetStream().WriteAsync( Encoding.ASCII.GetBytes( _msg ), 0, _msg.Length );
			} catch( Exception ex ) {
				Console.WriteLine( ex.Message );
				throw ex;
			}			
		}

	}
}
