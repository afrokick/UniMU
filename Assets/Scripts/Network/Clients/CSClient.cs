using System;
using System.Net.Sockets;
using log4net;
using MUnique.OpenMU.Network;

public class CSClient : ICSClient
{
    private readonly ILog log = LogManager.GetLogger(typeof(CSClient));

    private IConnection Connection { get; set; }

    public bool Connected => Connection != null && Connection.Connected;

    public event EventHandler ClientDisconnected;

    //public event ServerPacketReceivedHandler PacketReceived;

    [Inject]
    public PacketHandler PacketHandler { get; private set; }

    public void Connect(string ipAddress = "localhost", int port = 44405)
    {
        try
        {
            var serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Connect(ipAddress, port);

            Connection = new Connection(serverSocket, null, null);

            Connection.PacketReceived += OnPacketReceived;
            Connection.Disconnected += OnConnectionDisconnected;

            Connection.BeginReceive();
        }
        catch (SocketException socketException)
        {
            log.Error("Socket exception: " + socketException);
        }
    }

    public void Disconnect()
    {
        Connection?.Disconnect();
    }

    private void OnPacketReceived(object sender, byte[] rawPacket)
    {
        var packet = new Packet(rawPacket, false);

        log.Debug(packet.ToString());

        PacketHandler?.HandlePacket(sender, packet);
    }

    private void OnConnectionDisconnected(object sender, EventArgs e)
    {
        ClientDisconnected?.Invoke(sender, e);

        Connection.PacketReceived -= OnPacketReceived;
        Connection.Disconnected -= OnConnectionDisconnected;

        Connection = null;
    }

    public void RequestServerList()
    {
        byte[] packet = { 0xC1, 0x04, 0xF4, 0x06 };

        SendPacket(packet);
    }

    public void GetServerInfo(ushort serverId)
    {
        byte[] packet = { 0xC1, 0x06, 0xF4, 0x03, 0x00, 0x00 };

        SendPacket(packet);
    }

    private void SendPacket(byte[] packet)
    {
        System.Threading.ThreadPool.QueueUserWorkItem((object state) =>
        {
            Connection.Send(packet);
        });
    }
}
