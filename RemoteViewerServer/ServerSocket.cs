using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteViewerServer {
	class ServerSocket {

		static int m_port = 0;
		Thread m_thread = null;
		static List<TcpClient> m_client_list = new List<TcpClient>();

		static TcpListener m_listener = null;
		public delegate void recv_callback( byte[] _data, int _size );
		static recv_callback m_recv_callback;

		public ServerSocket( recv_callback _recv_callback = null ) {
			m_recv_callback = _recv_callback;
		}

		public void start( int _port ) {
			stop();

			m_port = _port;


			m_listener = new TcpListener( IPAddress.Any, m_port );
			m_listener.Start();

			ThreadStart ts = new ThreadStart( thread_proc_server_run );
			m_thread = new Thread( ts );
			m_thread.Start();
		}

		private static void thread_proc_server_run() {
			while( true ) {
				Thread.Sleep( 1 );
				try {
					Accept();
				} catch( Exception ) {
					
				}
			}
		}

		private static void Accept() {
			TcpClient tc = m_listener.AcceptTcpClient();
			m_client_list.Add( tc );
			
			Thread recv_thread = new Thread( new ParameterizedThreadStart( thread_proc_receive ) );
			recv_thread.Start( tc );
		}

		private static void thread_proc_receive( object _obj ) {
			TcpClient tc = ( TcpClient )_obj;
			NetworkStream stream = null;
			while( true ) {
				try {
					stream = tc.GetStream();

					var buff = new byte[ 1024 ];
					//stream.ReadTimeout = 3000;
					var nbytes = stream.Read( buff, 0, buff.Length );
					if( nbytes > 0 ) {
						m_recv_callback?.Invoke( buff, nbytes );
					} else {
						stream.Close();
						tc.Close();
						m_client_list.Remove( tc );
						break;
					}
				} catch( Exception ex ) {
					Console.Write( ex.Message );
					stream.Close();
					tc.Close();
					m_client_list.Remove( tc );
					break;
				}
			}
		}

		public void stop() {
			foreach( var client in m_client_list ) {
				client.Close();
			}
			m_client_list.Clear();

			m_listener?.Stop();
			m_thread?.Abort();
		}

		public void send( byte[] _data ) {
			try {
				foreach( var client in m_client_list ) {
					client.GetStream().Write( _data, 0, _data.Length );
				}
			} catch( Exception ex ) {
				Console.WriteLine( ex.Message );
			}
		}

		async static Task async_accept() {
			m_listener = new TcpListener( IPAddress.Any, m_port );
			m_listener.Start();
			while( true ) {

				// 비동기 Accept                
				TcpClient tc = await m_listener.AcceptTcpClientAsync().ConfigureAwait( false );
				m_client_list.Add( tc );

				// 새 쓰레드에서 처리
				await Task.Factory.StartNew( thread_proc_on_recv, tc );
			}
		}

		async private static void thread_proc_on_recv( object _obj ) {
			TcpClient tc = ( TcpClient )_obj;
			NetworkStream stream = null;
			while( true ) {
				try {
					if( tc.Connected == false ) {
						Thread.Sleep( 100 );
						continue;
					}

					int MAX_SIZE = 102400;
					stream = tc.GetStream();

					// 비동기 수신
					var buff = new byte[ MAX_SIZE ];
					var nbytes = await stream.ReadAsync( buff, 0, buff.Length ).ConfigureAwait( false );
					if( nbytes > 0 ) {
						m_recv_callback?.Invoke( buff, nbytes );
					}
				} catch( Exception ex ) {
					stream.Close();
					tc.Close();
					m_client_list.Remove( tc );
					break;
				}
			}
		}
	}
}
