public class LoginScreenHandler : IPacketHandler
{
    public byte HandlerCode => 0xF1;

    [Inject]
    public LoggedInSignal LoggedInSignal { get; private set; }
    [Inject]
    public OpenLoginScreenSignal OpenLoginScreenSignal { get; private set; }

    public void HandlePacket(object sender, Packet packet)
    {
        var subCode = packet.SubCode;
        packet.ReadByte();//read subcode

        switch (subCode)
        {
            case 0x00:
                packet.ReadByte();//read success
                OpenLoginScreenSignal.Dispatch();
                GSClient.ConnId = new byte[] { packet.ReadByte(), packet.ReadByte() };
                break;
            case 0x01:
                //login result

                var result = packet.ReadByte();

                if (result == 1)
                {
                    LoggedInSignal.Dispatch();
                }

                break;
        }
    }
}
