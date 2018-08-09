using System;

//public delegate void ServerPacketReceivedHandler(object sender, Packet packet);

public interface ICSClient
{
    void Connect(string ipAddress = "localhost", int port = 44405);
    void Disconnect();

    bool Connected { get; }

    event EventHandler ClientDisconnected;

    //event ServerPacketReceivedHandler PacketReceived;

    void RequestServerList();
    void GetServerInfo(ushort serverId);
}

public interface IGSClient
{
    void Connect(string ip, int port);
    void Disconnect();
    bool Connected { get; }

    event EventHandler ClientDisconnected;

    //event ServerPacketReceivedHandler PacketReceived;

    void Login(string username, string password);
}
