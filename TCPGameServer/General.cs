using System;
namespace TCPGameServer
{
    public class General
    {
        public static int GetTickCount()
		{
			return Environment.TickCount;
		}

        public static void InitServer()
		{
			Text.WriteLine("Loading server ...", TextType.DEBUG);

			int start = GetTickCount();

			InitClients();
			ServerHandleData.InitPackets();
			ServerTCP.InitServer();

			int end = GetTickCount();

			Text.WriteLine("Server loaded in {0} ms", TextType.DEBUG, end - start);
            //Start loop here in a new thread

		}

        private static void InitClients()
		{
			for (int i = 1; i < Constants.MAX_PLAYERS; i++)
			{
				ServerTCP.Clients[i] = new Client(null, 0);
			}
		}
    }
}
