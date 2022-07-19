public class FriendStateUpdateHandler : IPacketHandler
{
    public byte HandlerCode => 0xC4;

    public void HandlePacket(object sender, Packet packet)
    {
        var friendName = packet.ReadString(10);
        var serverId = packet.ReadByte();
    }
}
