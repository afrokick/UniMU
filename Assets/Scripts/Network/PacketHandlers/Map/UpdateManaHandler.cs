public class UpdateManaHandler : IPacketHandler
{
    public byte HandlerCode => 0x27;

    public void HandlePacket(object sender, Packet packet)
    {
        var updateType = (UpdateType)packet.ReadByte();
        var mana = packet.ReadShort(false);
        var ag = packet.ReadShort(false);

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