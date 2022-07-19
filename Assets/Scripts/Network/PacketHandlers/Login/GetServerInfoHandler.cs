using log4net;
using MUnique.OpenMU.Network;

public class GetServerInfoHandler : IPacketHandler, ISubPacketHandler
{
    private readonly ILog log = LogManager.GetLogger(typeof(GetServerListHandler));

    public byte HandlerCode => 0x03;

    [Inject]
    public ServerListItemUpdatedSignal ServerListItemUpdatedSignal { get; set; }

    public void HandlePacket(object sender, Packet packet)
    {
        var ip = packet.ReadString(16);
        var port = packet.Data.MakeWordBigEndian(packet.Size - 2);

        var model = new ServerListItemInfoModel { Ip = ip, Port = port };

        log.Info(model);

        ServerListItemUpdatedSignal.Dispatch(model);
    }
}
