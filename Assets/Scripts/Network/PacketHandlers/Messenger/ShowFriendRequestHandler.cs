public class ShowFriendRequestHandler : IPacketHandler
{
    public byte HandlerCode => 0xC2;

    public void HandlePacket(object sender, Packet packet)
    {
        var friendName = packet.ReadString(10);
    }
}
