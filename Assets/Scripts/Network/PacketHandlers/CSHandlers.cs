using System.Collections;
using System.Collections.Generic;
using log4net;

public class CSHandlers : IPacketHandler
{
    private readonly ILog log = LogManager.GetLogger(typeof(CSHandlers));

    private Dictionary<byte, IPacketHandler> Handlers { get; } = new Dictionary<byte, IPacketHandler>();

    [Inject]
    public GetServerListHandler GetServerListHandler { get; private set; }
    [Inject]
    public GetServerInfoHandler GetServerInfoHandler { get; private set; }

    [PostConstruct]
    public void Initialize()
    {
        Handlers.Add(0x06, GetServerListHandler);
        Handlers.Add(0x03, GetServerInfoHandler);
    }

    public void HandlePacket(object sender, Packet packet)
    {
        var subCode = packet.SubCode;

        if (Handlers.TryGetValue(subCode, out IPacketHandler packetHandler))
        {
            packetHandler?.HandlePacket(sender, packet);
        }
        else
        {
            log.Error($"handler not found for sub code of packet {packet}");
        }
    }
}
