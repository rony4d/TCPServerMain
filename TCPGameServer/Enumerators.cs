using System;
namespace TCPGameServer
{
	public enum ServerPackets
    {
        SIngame = 1,
        SPlayerData
    }

    public enum ClientPackets
    {
        ClientLogin,
        ClientNewAccount
    }
}
