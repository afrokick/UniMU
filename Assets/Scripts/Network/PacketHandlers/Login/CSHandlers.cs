using System.Collections;
using System.Collections.Generic;
using log4net;
using strange.extensions.injector.api;

public class CSHandlers : IPacketHandler
{
    private readonly ILog log = LogManager.GetLogger(typeof(CSHandlers));

    private Dictionary<byte, IPacketHandler> Handlers { get; } = new Dictionary<byte, IPacketHandler>();

    [Inject]
    public IInjectionBinder InjectionBinder { get; private set; }

    public byte HandlerCode => 0xF4;

    [PostConstruct]
    public void Initialize()
    {
        RegisterHandler<GetServerListHandler>();
        RegisterHandler<GetServerInfoHandler>();
    }

    private void RegisterHandler<T>() where T : IPacketHandler
    {
        var handler = InjectionBinder.GetInstance<T>();

        Handlers.Add(handler.HandlerCode, handler);
    }

    public void HandlePacket(object sender, Packet packet)
    {
        var subCode = packet.SubCode;

        if (Handlers.TryGetValue(subCode, out IPacketHandler packetHandler))
        {
            packet.ReadByte();//read subcode
            packetHandler?.HandlePacket(sender, packet);
        }
        else
        {
            log.Error($"[CSHandlers] handler not found for sub code of packet {packet}");
        }
    }
}
