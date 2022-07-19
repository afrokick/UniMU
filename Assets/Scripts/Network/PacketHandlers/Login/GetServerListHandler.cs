using System.Collections.Generic;
using log4net;

public class GetServerListHandler : IPacketHandler, ISubPacketHandler
{
    private readonly ILog log = LogManager.GetLogger(typeof(GetServerListHandler));

    public byte HandlerCode => 0x06;

    [Inject]
    public ServerListUpdatedSignal ServerListUpdatedSignal { get; set; }

    public void HandlePacket(object sender, Packet packet)
    {
        var count = packet.ReadShort();

        var list = new List<ServerListInfoItemModel>();

        while (count-- > 0)
        {
            var serverId = packet.ReadShort();
            var load = packet.ReadByte();
            var unknown = packet.ReadByte();

            list.Add(new ServerListInfoItemModel
            {
                ServerId = serverId,
                Usage = load
            });
        }


        var serverList = new ServerListInfoModel(list);

        log.Info(serverList);

        ServerListUpdatedSignal.Dispatch(serverList);
    }
}
