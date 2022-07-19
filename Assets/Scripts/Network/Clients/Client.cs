using System;
using System.Threading.Tasks;

public interface ICSClient
{
    Task Connect(string ipAddress, int port);
    void Disconnect();

    bool Connected { get; }

    event EventHandler ClientDisconnected;

    Task RequestServerList();
    Task GetServerInfo(ushort serverId);
}

public interface IGSClient
{
    Task Connect(string ip, int port);
    void Disconnect();
    bool Connected { get; }

    event EventHandler ClientDisconnected;

    Task Pong();
    Task UnknownCheck();
    Task Login(string username, string password);
    Task GetCharactersList();
    Task CreateCharacter(string characterName, byte classId);
    Task DeleteCharacter(string characterName, string securityCode);
    Task FocusCharacter(string characterName);
    Task SelectCharacter(string characterName);
}
