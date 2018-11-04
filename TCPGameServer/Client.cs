using System;
using System.Net.Sockets;

namespace TCPGameServer
{
    public class Client
    {
		public TcpClient Socket;
		public NetworkStream ClientNetworkStream;

		public int ConnectionID;
		byte[] _clientReceiveBuffer;

		public ByteBuffer ByteBuffer;

		public Client(TcpClient socket, int connectionID)
		{
			if (socket == null)
			{
				return;
			}
			Socket = socket;
			ConnectionID = connectionID;
            
			Socket.SendBufferSize = Constants.MAX_BUFFER_SIZE;
			Socket.ReceiveBufferSize = Constants.MAX_BUFFER_SIZE;
			ClientNetworkStream = Socket.GetStream();
            Text.WriteLine("nextwork stream initialized for client: " + connectionID, TextType.INFO);
			_clientReceiveBuffer = new byte[Constants.MAX_BUFFER_SIZE];
			ClientNetworkStream.BeginRead(_clientReceiveBuffer, Constants.NETWORK_STREAM_OFFSET,Socket.ReceiveBufferSize, ReceiveBufferCallback, null);

			Text.WriteLine("Incoming connection from {0}", TextType.INFO, Socket.Client.RemoteEndPoint.ToString());

			General.JoinMap(connectionID);
		}

		private void ReceiveBufferCallback(IAsyncResult result)
		{
			try
			{
				int readBytes = ClientNetworkStream.EndRead(result);

                if (readBytes <= 0)
                {
					CloseConnection();
                    return;
                }

                byte[] newBytesRead = new byte[readBytes];
                Buffer.BlockCopy(_clientReceiveBuffer, Constants.NETWORK_STREAM_OFFSET, newBytesRead, Constants.NETWORK_STREAM_OFFSET, readBytes);

				//Handle the Data Here
                Text.WriteLine("Server is now handling data from " + Socket.Client.RemoteEndPoint, TextType.INFO);

				ServerHandleData.HandleData(ConnectionID, newBytesRead); 

				ClientNetworkStream.BeginRead(_clientReceiveBuffer, Constants.NETWORK_STREAM_OFFSET, Socket.ReceiveBufferSize, ReceiveBufferCallback, null);

			}
			catch 
			{
				CloseConnection();
				return;
			}
		
		}

		private void CloseConnection()
		{
			Text.WriteLine("Connection from {0} has been terminated", TextType.INFO, Socket.Client.RemoteEndPoint.ToString());
			ServerTCP.SendPlayerDisconnect(ConnectionID);
			Socket.Close();
			Socket = null;
		}
	}
}
