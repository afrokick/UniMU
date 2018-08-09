using System.Collections.Generic;
using log4net;

public class GetServerListHandler : IPacketHandler
{
    private readonly ILog log = LogManager.GetLogger(typeof(GetServerListHandler));

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

        ServerListUpdatedSignal.Dispatch(new ServerListInfoModel(list));
    }
}
