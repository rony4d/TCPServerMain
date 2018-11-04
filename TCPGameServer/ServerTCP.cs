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

            Text.WriteLine("Client connection received", TextType.INFO);
            for (int i = 1; i < Constants.MAX_PLAYERS; i++)
			{
                if (Clients[i].Socket == null)
				{
					Clients[i] = new Client(tcpClient, i);
                    if (i == 1)
                    {
                        Text.WriteLine("Client with connection Id " + Clients[i].ConnectionID + " socket connected" + Clients[i].Socket.Connected, TextType.INFO);

                    }

					General.JoinMap(i);
					return;
				}
			}
		}

        public static void SendDataTo(int connectionID, byte[] data)
		{
            try
            {
                ByteBuffer byteBuffer = new ByteBuffer();
                byteBuffer.WriteInteger(data.GetUpperBound(0) - data.GetLowerBound(0) + 1);
                byteBuffer.WriteBytes(data);

                if (Clients[connectionID].ClientNetworkStream != null)
                {
                    Text.WriteLine("Client with connection Id" + connectionID + " has a network stream", TextType.DEBUG);

                    Clients[connectionID].ClientNetworkStream.BeginWrite(byteBuffer.ToArray(), Constants.NETWORK_STREAM_OFFSET, byteBuffer.ToArray().Length, null, null);

                }
                else
                {
                    Text.WriteLine("Client with connection Id" + connectionID + " has null network stream", TextType.ERROR);
                }
                //byteBuffer.Dispose();
            }
            catch (ObjectDisposedException ex)
            {
                Text.WriteLine("SendDataTo Object disposed exception: "+ ex.Message, TextType.ERROR);

            }

		}

		public static void SendDataToAll(byte[] data)
		{
            try
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
            catch (ObjectDisposedException ex)
            {
                Text.WriteLine("SendDataToAll Object disposed exception: " + ex.Message, TextType.ERROR);

            }
        
		}

		public static void SendInGame(int connectionId)
		{
            try
            {
                Text.WriteLine("connectionID " + connectionId + " sendInGame Called", TextType.DEBUG);

                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteInteger((int)ServerPackets.SIngame);
                buffer.WriteInteger(connectionId);
                SendDataTo(connectionId, buffer.ToArray());
                //buffer.Dispose();
            }
            catch (ObjectDisposedException ex)
            {
                Text.WriteLine("SendInGame Object disposed exception: " + ex.Message, TextType.ERROR);

            }
 
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
            try
            {
                Text.WriteLine("connectionID " + connectionId + " sendPlayerData called", TextType.DEBUG);

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
            catch (ObjectDisposedException ex)
            {
                Text.WriteLine("SendPlayerData Object disposed exception: " + ex.Message, TextType.ERROR);

            }

		}

		public static void SendPlayerDisconnect(int connectionId)
        {
            try
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteInteger((int)ServerPackets.SIngame);
                buffer.WriteInteger(connectionId);
                SendDataToAllBut(connectionId, buffer.ToArray());
                //buffer.Dispose();
            }
            catch (ObjectDisposedException ex)
            {
                Text.WriteLine("SendPlayerDisconnect Object disposed exception: " + ex.Message, TextType.ERROR);

            }
  
        }

		private static void SendDataToAllBut(int connectionId, byte[] data)
		{
            try
            {
                for (int i = 1; i < Constants.MAX_PLAYERS; i++)
                {
                    if (i != connectionId)
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
            }
            catch (ObjectDisposedException ex)
            {
                Text.WriteLine("SendDataToAllBut Object disposed exception: " + ex.Message, TextType.ERROR);

            }
        
		}
	}
}
