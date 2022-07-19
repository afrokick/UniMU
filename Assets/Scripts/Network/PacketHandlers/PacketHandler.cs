using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using strange.extensions.injector.api;

public class PacketHandler
{
    private static readonly Type SubPacketHandlerInterfaceType = typeof(ISubPacketHandler);

    private readonly ILog log = LogManager.GetLogger(typeof(PacketHandler));

    private Dictionary<byte, IPacketHandler> Handlers { get; } = new Dictionary<byte, IPacketHandler>();

    [Inject]
    public ICoroutineExecuter CoroutineExecuter { get; private set; }

    [Inject]
    public IInjectionBinder InjectionBinder { get; private set; }

    [PostConstruct]
    public void Initialize()
    {
        var packetHandlerInterfaceType = typeof(IPacketHandler);
        var handlers = this.GetType().Assembly.GetTypes().Where(type => type.IsClass
                                                                && type.GetInterfaces().Contains(packetHandlerInterfaceType)
                                                                && !type.GetInterfaces().Contains(SubPacketHandlerInterfaceType));

        foreach (var handlerType in handlers)
        {
            var handler = InjectionBinder.GetInstance(handlerType) as IPacketHandler;

            Handlers.Add(handler.HandlerCode, handler);
        }
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
            log.Error($"[PacketHandler]Can't find packet handler for packet:{packet}");
        }
    }
}
