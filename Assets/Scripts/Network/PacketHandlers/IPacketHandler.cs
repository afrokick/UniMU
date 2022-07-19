public interface IPacketHandler
{
    byte HandlerCode { get; }

    void HandlePacket(object sender, Packet packet);
}

public interface ISubPacketHandler
{
}