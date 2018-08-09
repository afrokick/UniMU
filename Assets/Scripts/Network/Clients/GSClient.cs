using System;
using System.Net.Sockets;
using System.Text;
using log4net;
using MUnique.OpenMU.Network;
using MUnique.OpenMU.Network.Xor;

public class GSClient : IGSClient
{
    private readonly ILog log = LogManager.GetLogger(typeof(GSClient));

    private IConnection Connection { get; set; }

    public bool Connected => Connection != null && Connection.Connected;

    public event EventHandler ClientDisconnected;

    //public event ServerPacketReceivedHandler PacketReceived;

    [Inject]
    public PacketHandler PacketHandler { get; private set; }

    public void Connect(string ipAddress, int port)
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

    public void Login(string username = "test0", string password = "test0")
    {
        var maxPasswordLength = 20;

        var encryptor = new Xor3Encryptor(0);

        var packet = new byte[43];

        packet[0] = 0xC3;
        packet[1] = (byte)packet.Length;
        packet[2] = 0xF1;
        packet[3] = 0x01;

        byte[] userbytes = new byte[10];
        byte[] pass = new byte[maxPasswordLength];

        Encoding.UTF8.GetBytes(username, 0, username.Length, userbytes, 0);
        Encoding.UTF8.GetBytes(password, 0, password.Length, pass, 0);

        encryptor.Encrypt(userbytes);
        encryptor.Encrypt(pass);

        Array.Copy(userbytes, 0, packet, 4, userbytes.Length);
        Array.Copy(pass, 0, packet, 14, pass.Length);

        int startTickCountIndex = 34;

        var versionStartIndex = startTickCountIndex + 4;

        byte[] version = { 0x31, 0x30, 0x34, 0x30, 0x34 };

        Array.Copy(version, 0, packet, versionStartIndex, 5);

        Connection.Send(packet);
    }
}
