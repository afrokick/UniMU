public class FriendAddedHandler : IPacketHandler
{
    public byte HandlerCode => 0xC1;

    public void HandlePacket(object sender, Packet packet)
    {
        var friendName = packet.ReadString(10);
        var serverId = packet.ReadByte();
    }
}
