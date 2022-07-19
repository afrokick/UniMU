using log4net;

public class MagicEffectHandler : IPacketHandler
{
    private readonly ILog log = LogManager.GetLogger(typeof(MagicEffectHandler));

    public byte HandlerCode => 0x07;

    public void HandlePacket(object sender, Packet packet)
    {
        var isActive = packet.ReadBool();
        var characterId = packet.ReadShort();
        var effectId = packet.ReadByte();

        log.Info($"MagicEffectHandler: charId:{characterId} effect:{effectId} isActive:{isActive}");
    }
}

public class PingHandler : IPacketHandler
{
    [Inject]
    public IGSClient GSClient { get; private set; }

    private readonly ILog log = LogManager.GetLogger(typeof(PingHandler));

    public byte HandlerCode => 0xFC;

    //int count = 0;

    public async void HandlePacket(object sender, Packet packet)
    {
        log.Info($"Ping: handle ping");

        //count++;

        //if (count % 5 == 0)
        //{
        //    await GSClient.UnknownCheck();
        //}
    }
}

public class SpawnHandler : IPacketHandler
{
    private readonly ILog log = LogManager.GetLogger(typeof(SpawnHandler));

    public byte HandlerCode => 0xB8;

    public void HandlePacket(object sender, Packet packet)
    {
        //var isActive = packet.ReadBool();
        //var characterId = packet.ReadShort();
        //var effectId = packet.ReadByte();

        //log.Info($"MagicEffectHandler: charId:{characterId} effect:{effectId} isActive:{isActive}");
    }
}

public class ServerMessageHandler : IPacketHandler
{
    public enum MessageType
    {
        /// <summary>
        /// The message is shown as centered golden message in the client.
        /// </summary>
        GoldenCenter = 0,

        /// <summary>
        /// The message is shown as a blue system message.
        /// </summary>
        BlueNormal = 1,

        /// <summary>
        /// The message is a guild notice, centered in green.
        /// </summary>
        GuildNotice = 2,
    }

    private readonly ILog log = LogManager.GetLogger(typeof(ServerMessageHandler));

    public byte HandlerCode => 0x0D;

    public void HandlePacket(object sender, Packet packet)
    {
        var messageType = (MessageType)packet.ReadByte();

        packet.ReadOffset += 9;

        var message = packet.ReadString(packet.Size - packet.ReadOffset - 4);

        packet.ReadOffset += 4;

        log.Info($"ServerMessage:[{messageType}]${message}");
    }
}

public class PlayerShopsHandler : IPacketHandler
{
    private readonly ILog log = LogManager.GetLogger(typeof(PlayerShopsHandler));

    public byte HandlerCode => 0x3F;

    public void HandlePacket(object sender, Packet packet)
    {
        //C2 00 2C 3F
        //00 - subCode
        //01 - count
        //31 EF - char id
        //45 58 53 5F 5F 5F 5F 4A 45 57 4C 53 00 00 00 00 00 00
        //00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00

        var subCode = packet.SubCode;
        packet.ReadByte();

        switch (subCode)
        {
            case 0x00:
                {
                    var count = packet.ReadByte();

                    while (count > 0)
                    {
                        var charId = packet.ReadShort();
                        var shopName = packet.ReadString(36);
                        count--;

                        log.Info($"SHOP: id:{charId} name:{shopName}");
                    }
                    break;
                }
        }
    }
}