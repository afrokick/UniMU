using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using log4net;
using MUnique.OpenMU.Network;

public class CSClient : ICSClient
{
    private readonly ILog log = LogManager.GetLogger(typeof(CSClient));

    private IConnection Connection { get; set; }

    public bool Connected => Connection != null && Connection.Connected;

    public event EventHandler ClientDisconnected;

    [Inject]
    public PacketHandler PacketHandler { get; private set; }

    [Inject]
    public AppQuitSignal AppQuitSignal { get; private set; }

    public async Task Connect(string ipAddress, int port)
    {
        var serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            serverSocket.Connect(ipAddress, port);

            Connection = new Connection(serverSocket, null, null);

            Connection.PacketReceived += OnPacketReceived;
            Connection.Disconnected += OnConnectionDisconnected;

            AppQuitSignal.AddOnce(() => Disconnect());

            Connection.BeginReceive();
        }
        catch (SocketException socketException)
        {
            log.Error("Socket exception: " + socketException);
        }
    }

    public void Disconnect()
    {
        log.Info("Disconnect");
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
        log.Info("OnConnectionDisconnected");
        ClientDisconnected?.Invoke(sender, e);

        Connection.PacketReceived -= OnPacketReceived;
        Connection.Disconnected -= OnConnectionDisconnected;

        Connection = null;
    }

    public async Task RequestServerList()
    {
        byte[] packet = { 0xC1, 0x04, 0xF4, 0x06 };

        await SendPacket(packet);
    }

    public async Task GetServerInfo(ushort serverId)
    {
        byte[] packet = { 0xC1, 0x06, 0xF4, 0x03, 0x00, 0x00 };

        await SendPacket(packet);
    }

    private async Task SendPacket(byte[] packet)
    {
        log.Info("SendPacket");
        await Task.Run(() => Connection.Send(packet));
    }
}
