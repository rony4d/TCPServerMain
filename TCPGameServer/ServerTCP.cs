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
			serverSocket.BeginAcceptTcpClient(new AsyncCallback(ClientConnectCallback), null);// creates a new thread for the TCP server to start listening for incoming connections/packets
		}

		private static void ClientConnectCallback(IAsyncResult result)
		{
            //debugger can't get this guy since he is running on a separate thread. 
			TcpClient tcpClient = serverSocket.EndAcceptTcpClient(result);
			serverSocket.BeginAcceptTcpClient(new AsyncCallback(ClientConnectCallback), null);// creates a new thread for the TCP server to start listening for incoming connections/packets
            
            for (int i = 1; i < Constants.MAX_PLAYERS; i++)
			{
                if (Clients[i].Socket == null)
				{
					Clients[i] = new Client(tcpClient, i);
					General.JoinMap(i);
					return;
				}
			}
		}

        public static void SendDataTo(int connectionID, byte[] data)
		{
			ByteBuffer byteBuffer = new ByteBuffer();
			byteBuffer.WriteInteger(data.GetUpperBound(0) - data.GetLowerBound(0) + 1);
			byteBuffer.WriteBytes(data);
			if (Clients[connectionID].ClientNetworkStream != null)
			{
				Clients[connectionID].ClientNetworkStream.BeginWrite(byteBuffer.ToArray(), Constants.NETWORK_STREAM_OFFSET, byteBuffer.ToArray().Length, null, null);

			}else{
				Text.WriteLine("Client with connection Id" + connectionID +" has null network stream", TextType.ERROR);
			}
			byteBuffer.Dispose();
		}

		public static void SendDataToAll(byte[] data)
		{
			for (int i = 1; i < Constants.MAX_PLAYERS; i++)
			{
                if (Clients[i].Socket != null)
				{
                    if (Types.TempPlayerRecs[i].isPlaying)
					{
						SendDataTo(i, data); 
					}
				}
			}
		}

		public static void SendInGame(int connectionId)
		{
			ByteBuffer buffer = new ByteBuffer();
			buffer.WriteInteger((int)ServerPackets.SIngame);
			buffer.WriteInteger(connectionId);
			SendDataTo(connectionId, buffer.ToArray());
			buffer.Dispose();
		}

 

		public static byte[] PlayerData(int connectionId)
		{
			ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInteger((int)ServerPackets.SPlayerData);
            buffer.WriteInteger(connectionId);
			return buffer.ToArray();
		}

		public static void SendPlayerData(int connectionId)
        {
	
			for (int i = 1; i < Constants.MAX_PLAYERS; i++)
			{
				if (Clients[i] != null)
                {
                    if (Types.TempPlayerRecs[i].isPlaying)
					{
                        if (connectionId != i) 
						{
							SendDataTo(connectionId, PlayerData(i));
						}
					}
				}
			}

			SendDataToAll(PlayerData(connectionId));
		}
	
    }
}
