public class LoginScreenFBHandler : IPacketHandler
{
    public byte HandlerCode => 0xFB;

    [Inject]
    public LoggedInSignal LoggedInSignal { get; private set; }
    [Inject]
    public OpenLoginScreenSignal OpenLoginScreenSignal { get; private set; }

    public void HandlePacket(object sender, Packet packet)
    {
        var subCode = packet.SubCode;
        packet.ReadByte();//read subcode;
    }
}
