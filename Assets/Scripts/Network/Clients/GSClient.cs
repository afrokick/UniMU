using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using log4net;
using MUnique.OpenMU.Network;
using MUnique.OpenMU.Network.SimpleModulus;
using MUnique.OpenMU.Network.Xor;

public class GSClient : IGSClient
{
    private readonly ILog log = LogManager.GetLogger(typeof(GSClient));

    private IConnection Connection { get; set; }

    public bool Connected => Connection != null && Connection.Connected;

    public event EventHandler ClientDisconnected;

    [Inject]
    public PacketHandler PacketHandler { get; private set; }

    [Inject]
    public AppQuitSignal AppQuitSignal { get; private set; }

    [Inject]
    public MainModel MainModel { get; private set; }

    public static byte[] ConnId { get; set; }

    private Timer pingTimer { get; } = new Timer();

    public async Task Connect(string ipAddress, int port)
    {
        var serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            serverSocket.Connect(ipAddress, port);

            Connection = new Connection(serverSocket, new Encryptor(), new Decryptor(SimpleModulusDecryptor.DefaultClientKey));

            Connection.PacketReceived += OnPacketReceived;
            Connection.Disconnected += OnConnectionDisconnected;

            AppQuitSignal.AddOnce(() =>
            {
                Disconnect();
            });

            Connection.BeginReceive();
        }
        catch (SocketException socketException)
        {
            log.Error("Socket exception: " + socketException);
        }

        log.Debug("connected to GS");

        pingTimer.Interval = 10000;
        pingTimer.Elapsed += PingTimer_Elapsed;
        pingTimer.AutoReset = true;
        pingTimer.Enabled = true;
    }

    private async void PingTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
        //await Pong();
    }

    public void Disconnect()
    {
        log.Debug("disconnected from GS");
        Connection?.Disconnect();

        if (pingTimer != null)
        {
            pingTimer.Enabled = false;
        }
    }

    private void OnPacketReceived(object sender, byte[] rawPacket)
    {
        var packet = new Packet(rawPacket, false);

        log.Debug(packet.ToString());

        PacketHandler?.HandlePacket(sender, packet);
    }

    private void OnConnectionDisconnected(object sender, EventArgs e)
    {
        log.Debug("GS: OnConnectionDisconnected");
        ClientDisconnected?.Invoke(sender, e);

        Connection.PacketReceived -= OnPacketReceived;
        Connection.Disconnected -= OnConnectionDisconnected;

        Connection = null;
    }

    bool notReady = false;

    public async Task Pong()
    {
        //var ticks = new byte[4];
        //var nowMs = (DateTime.Now.Ticks % TimeSpan.TicksPerDay) / TimeSpan.TicksPerMillisecond;
        //SetIntegerBigEndian(ticks, (uint)nowMs);

        //var packet = new byte[] { 0xC3, 12, 0x0E, 0, ticks[0], ticks[1], ticks[2], ticks[3], 0, 0, 0, 0, };

        //await SendPacket(packet);
    }

    private byte[] FromHexToPacket(string hex)
    {
        return hex.Split('-', ' ').Select(s => Convert.ToByte(s, 16)).ToArray();
    }

    public async Task UnknownCheck()
    {
        var str = "C1-58-FB-76-FD-0E-D3-90-F6-69-A3-23-2B-40-4F-2A-96-6E-3E-F5-2F-10-0B-10-E0-13-BC-7B-61-75-A4-C5-1A-4E-42-F8-75-FF-59-68-7A-E1-29-AF-E6-BB-82-DF-55-9E-FD-70-9A-E7-CC-93-50-E1-08-8B-A2-F3-1B-4D-AA-C7-FD-74-22-C0-5F-2C-B2-27-29-7E-49-56-60-3D-41-A4-C8-45-05-9D-A8-F7";

        var packet = FromHexToPacket(str);

        //var dec = new ServerDecryptor(SimpleModulusDecryptor.DefaultServerKey);

        //dec.Decrypt(ref packet);


        log.Info($"send unknown check:{packet.AsString()}");

        await SendPacket(packet);
    }

    public void SetIntegerBigEndian(byte[] data, uint value)
    {
        data[0] = (byte)(value & 0xFF);
        data[1] = (byte)((value >> 8) & 0xFF);
        data[2] = (byte)((value >> 16) & 0xFF);
        data[3] = (byte)((value >> 24) & 0xFF);
    }

    public async Task Login(string username = "test0", string password = "test0")
    {
        log.Info("Login");
        var maxLoginLength = 10;
        var maxPasswordLength = 10;

        var encryptor = new Xor3Encryptor(0);

        var packet = new byte[49];

        packet[0] = 0xC3;
        packet[1] = (byte)packet.Length;
        packet[2] = 0xF1;
        packet[3] = 0x01;

        var headerSize = 4;

        byte[] userbytes = new byte[maxLoginLength];
        byte[] pass = new byte[maxPasswordLength];

        Encoding.UTF8.GetBytes(username, 0, username.Length, userbytes, 0);
        Encoding.UTF8.GetBytes(password, 0, password.Length, pass, 0);

        encryptor.Encrypt(userbytes);
        encryptor.Encrypt(pass);

        Array.Copy(userbytes, 0, packet, headerSize, userbytes.Length);
        Array.Copy(pass, 0, packet, headerSize + maxLoginLength, pass.Length);

        int startTickCountIndex = headerSize + maxLoginLength + maxPasswordLength;

        //var oldTick = new byte[] { 0x96, 0xEE };

        var tick = new byte[4];
        var nowMs = (DateTime.Now.Ticks % TimeSpan.TicksPerDay) / TimeSpan.TicksPerMillisecond;
        SetIntegerBigEndian(tick, (uint)nowMs);
        log.Info($"set ticks for login:{nowMs}");
        Array.Copy(tick, 0, packet, startTickCountIndex, tick.Length);

        var versionStartIndex = startTickCountIndex + tick.Length;

        var verAndSum = new byte[] { 0x31, 0x30, 0x34, 0x31, 0x30, 0x43, 0x79, 0x74, 0x6F, 0x49, 0x53, 0x65, 0x72, 0x70, 0x61, 0x6E, 0x74, 0x4C, 0x6F, 0x78, 0x69 };

        Array.Copy(verAndSum, 0, packet, versionStartIndex, verAndSum.Length);

        await SendPacket(packet);
    }

    public async Task GetCharactersList()
    {
        log.Info("GetCharactersList");
        var packet = new byte[] { 0xC1, 0x04, 0xF3, 0x00 };

        await SendPacket(packet);
    }

    public async Task CreateCharacter(string characterName, byte classId)
    {
        var packet = new byte[15];
        packet[0] = 0xC1;
        packet[2] = 0xF3;
        packet[3] = 0x01;

        Encoding.UTF8.GetBytes(characterName, 0, characterName.Length, packet, 4);

        packet[14] = (byte)(classId << 2);

        packet.SetPacketSize();

        await SendPacket(packet);
    }

    public async Task DeleteCharacter(string characterName, string securityCode)
    {
        var packet = new byte[21];
        packet[0] = 0xC1;
        packet[2] = 0xF3;
        packet[3] = 0x02;

        Encoding.UTF8.GetBytes(characterName, 0, characterName.Length, packet, 4);
        Encoding.UTF8.GetBytes(securityCode, 0, securityCode.Length, packet, 14);

        packet.SetPacketSize();

        await SendPacket(packet);
    }

    public async Task FocusCharacter(string characterName)
    {
        var packet = new byte[14];
        packet[0] = 0xC1;
        packet[2] = 0xF3;
        packet[3] = 0x15;

        Encoding.UTF8.GetBytes(characterName, 0, characterName.Length, packet, 4);

        packet.SetPacketSize();

        await SendPacket(packet);
    }

    public async Task SelectCharacter(string characterName)
    {
        var packet = new byte[] { 0xC1, 0x04, 0xF3, 0x03, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        Encoding.UTF8.GetBytes(characterName, 0, characterName.Length, packet, 4);

        packet.SetPacketSize();

        await SendPacket(packet);
    }

    //public async Task Walk(byte x, byte y)
    //{
    //    log.Info($"Send walk: {x} {y}");
    //    var packet = new byte[] { 0xC1, 6, 0xD4, x, y, 0 };

    //    packet.SetPacketSize();

    //    await SendPacket(packet);
    //}

    private async Task SendPacket(byte[] packet)
    {
        log.Info("SendPacket");
        await Connection?.Send(packet);
    }
}
