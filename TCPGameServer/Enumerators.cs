using System;
namespace TCPGameServer
{
	public enum ServerPackets
    {
        SIngame = 1,
        SPlayerData,
		SDisconnect
    }

    public enum ClientPackets
    {
        ClientLogin,
        ClientNewAccount
    }
}
