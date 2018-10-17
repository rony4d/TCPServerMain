using System;
using System.Collections.Generic;

namespace TCPGameServer
{
    public class ServerHandleData
    {
		public delegate void Packet_(int connectionId, byte[] data);

		public static Dictionary<int, Packet_> Packets = new Dictionary<int, Packet_>();
        
		public static int PacketLength;

        
        public static void InitPackets()
		{
			Text.WriteLine("Initializing network messages...", TextType.DEBUG);
		}

        public static void HandleData(int connectionId, byte [] data)
		{
			byte[] buffer = (byte [])data.Clone();

			if (ServerTCP.Clients[connectionId].ByteBuffer == null)
			{
				ServerTCP.Clients[connectionId].ByteBuffer = new ByteBuffer();
			}
            
			ServerTCP.Clients[connectionId].ByteBuffer.WriteBytes(buffer);

			if (ServerTCP.Clients[connectionId].ByteBuffer.Count() == 0)
            {
				ServerTCP.Clients[connectionId].ByteBuffer.Clear();
				return;
            }

            if (ServerTCP.Clients[connectionId].ByteBuffer.Count() >= Constants.INT_SIZE)
			{
				PacketLength = ServerTCP.Clients[connectionId].ByteBuffer.ReadInteger(false);
                if (PacketLength <= 0)
				{
					ServerTCP.Clients[connectionId].ByteBuffer.Clear();
					return;
				}
			}

			while (PacketLength > 0 && PacketLength <= ServerTCP.Clients[connectionId].ByteBuffer.Length() - Constants.INT_SIZE) 
			{
				if (PacketLength <= ServerTCP.Clients[connectionId].ByteBuffer.Length() - Constants.INT_SIZE)
				{
					ServerTCP.Clients[connectionId].ByteBuffer.ReadInteger();

					data = ServerTCP.Clients[connectionId].ByteBuffer.ReadBytes(PacketLength);
					HandleDataPackets(connectionId, data);
				}

				PacketLength = 0;
				if (ServerTCP.Clients[connectionId].ByteBuffer.Length() >= Constants.INT_SIZE)
                {
                    ServerTCP.Clients[connectionId].ByteBuffer.ReadInteger();

					PacketLength = ServerTCP.Clients[connectionId].ByteBuffer.ReadInteger(false);

					if (PacketLength <= 0)
                    {
                        ServerTCP.Clients[connectionId].ByteBuffer.Clear();
                        return;
                    }

                }

                if (PacketLength <= 1)
				{
					ServerTCP.Clients[connectionId].ByteBuffer.Clear();
				}
			}

		}

        private static void HandleDataPackets(int connectionId, byte[] data)
		{
			int packetId;
			ByteBuffer byteBuffer = new ByteBuffer();
			byteBuffer.WriteBytes(data);
			packetId = byteBuffer.ReadInteger();
			byteBuffer.Dispose();
            if (Packets.TryGetValue(packetId, out Packet_ packet))
			{
				Text.WriteLine("<Packet>" + Enum.GetName(typeof(ClientPackets), packetId),TextType.DEBUG);
				packet.Invoke(connectionId, data);
			}

		}
	}


}
