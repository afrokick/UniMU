using log4net;
using MUnique.OpenMU.Network;

public class GetServerInfoHandler : IPacketHandler
{
    private readonly ILog log = LogManager.GetLogger(typeof(GetServerInfoHandler));

    public void HandlePacket(object sender, Packet packet)
    {
        packet.ReadByte();
        var ip = packet.ReadString(16);
        int port = packet.Data.MakeWordBigEndian(packet.Size - 2);

        log.Debug($"server info ip:{ip} port:{port}");
    }
}
