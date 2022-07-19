public class UpdateHealthHandler : IPacketHandler
{
    public byte HandlerCode => 0x26;

    public void HandlePacket(object sender, Packet packet)
    {
        var updateType = (UpdateType)packet.ReadByte();
        var hp = packet.ReadShort(false);
        packet.ReadByte();
        var sd = packet.ReadShort(false);

        switch (updateType)
        {
            case UpdateType.Maximum:

                break;
            case UpdateType.Current:

                break;
            default:
                break;
        }
    }
}
