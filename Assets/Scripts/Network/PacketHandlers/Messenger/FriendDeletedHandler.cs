public class FriendDeletedHandler : IPacketHandler
{
    public byte HandlerCode => 0xC3;

    public void HandlePacket(object sender, Packet packet)
    {
        var friendName = packet.ReadString(10);
    }
}
