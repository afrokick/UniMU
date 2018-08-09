using System.Collections.Generic;
using System.Text;
using log4net;
using MUnique.OpenMU.Network;

public class PacketHandler
{
    private readonly ILog log = LogManager.GetLogger(typeof(PacketHandler));

    private Dictionary<byte, IPacketHandler> Handlers { get; } = new Dictionary<byte, IPacketHandler>();

    [Inject]
    public ICoroutineExecuter CoroutineExecuter { get; private set; }
    [Inject]
    public SayHelloHandler SayHelloHandler { get; private set; }
    [Inject]
    public CSHandlers CSHandlers { get; private set; }

    [PostConstruct]
    public void Initialize()
    {
        Handlers.Add(0x00, SayHelloHandler);
        Handlers.Add(0xF4, CSHandlers);
    }

    public void HandlePacket(object sender, Packet packet)
    {
        var packetType = packet.Code;

        if (Handlers.TryGetValue(packetType, out IPacketHandler packetHandler))
        {
            CoroutineExecuter.ExecuteOnUpdate(() =>
            {
                log.Debug($"handle packet with {packetHandler.GetType().Name}");

                packetHandler?.HandlePacket(sender, packet);
            });
        }
        else
        {
            log.Error($"Can't find packet handler for packet:{packet}");
        }
    }
}
