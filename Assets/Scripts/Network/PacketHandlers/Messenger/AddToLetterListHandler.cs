public class AddToLetterListHandler : IPacketHandler
{
    public byte HandlerCode => 0xC6;

    public void HandlePacket(object sender, Packet packet)
    {
        var id = packet.ReadShort();
        var senderName = packet.ReadString(10);
        var date = packet.ReadString(30);
        var subject = packet.ReadString(32);
        var flag = packet.ReadByte();

        switch (flag)
        {
            case 0:
                //not read
                break;
            case 1:
                //read, no new
                break;
            case 2:
                //read, new
                break;

            default: throw new System.Exception("flag not implemened");
        }
    }
}
