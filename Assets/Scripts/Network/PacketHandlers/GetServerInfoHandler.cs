using MUnique.OpenMU.Network;

public class GetServerInfoHandler : IPacketHandler
{
    [Inject]
    public ServerListItemUpdatedSignal ServerListItemUpdatedSignal { get; set; }

    public void HandlePacket(object sender, Packet packet)
    {
        packet.ReadByte();
        var ip = packet.ReadString(16);
        var port = packet.Data.MakeWordBigEndian(packet.Size - 2);

        var model = new ServerListItemInfoModel { Ip = ip, Port = port };

        ServerListItemUpdatedSignal.Dispatch(model);
    }
}
