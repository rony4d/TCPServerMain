using System;
using System.Threading;

namespace TCPGameServer
{
    class Program
    {
		static Thread mainThread = new Thread(MainThread);
        
        static void Main(string[] args)
        {
			mainThread.Name = "main thread";
			Text.WriteLine("INITIALIZING {0}", TextType.DEBUG, mainThread.Name);
			mainThread.Start();
        }

		static void MainThread()
		{
			General.InitServer();
			while(true)
			{
				
			}
		}
    }
}
