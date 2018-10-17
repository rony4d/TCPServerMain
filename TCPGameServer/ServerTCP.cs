using System;
using System.Net;
using System.Net.Sockets;

namespace TCPGameServer
{
    public class ServerTCP
    {
		private static TcpListener serverSocket = new TcpListener(IPAddress.Any, Constants.PORT);
		public static Client[] Clients = new Client[Constants.MAX_PLAYERS];

        public static void InitServer()
		{
			Text.WriteLine("Initializing server socket...", TextType.DEBUG);
			serverSocket.Start();
			serverSocket.BeginAcceptTcpClient(new AsyncCallback(ClientConnectCallback), null);// creates a new thread for the TCP server
		}

		private static void ClientConnectCallback(IAsyncResult result)
		{
			TcpClient tcpClient = serverSocket.EndAcceptTcpClient(result);
			serverSocket.BeginAcceptTcpClient(new AsyncCallback(ClientConnectCallback), null);// creates a new thread for the TCP server
            
            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
			{
                if (Clients[i].Socket == null)
				{
					Clients[i] = new Client(tcpClient, i);
					return;
				}
			}
		}

        public static void SendDataTo(int connectionID, byte[] data)
		{
			ByteBuffer byteBuffer = new ByteBuffer();
			byteBuffer.WriteInteger(data.GetUpperBound(0) - data.GetLowerBound(0) + 1);
			byteBuffer.WriteBytes(data);
			Clients[connectionID].ClientNetworkStream.BeginWrite(byteBuffer.ToArray(), Constants.NETWORK_STREAM_OFFSET, byteBuffer.ToArray().Length, null,null);
			byteBuffer.Dispose();
		}

		public static void SendDataToAll(byte[] data)
		{
			////
		}
	
    }
}
