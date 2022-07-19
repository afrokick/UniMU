using log4net;
using MUnique.OpenMU.DataModel.Configuration;

public class NewNpcsInScopeHandler : IPacketHandler
{
    private readonly ILog log = LogManager.GetLogger(typeof(NewNpcsInScopeHandler));

    public byte HandlerCode => 0x13;

    public void HandlePacket(object sender, Packet packet)
    {
        //C2 00 19 13
        //02 - count
        //0B 46 00 F1 D7 2D D7 2D 10 00
        //0B 4B 00 E5 C6 2F C6 2F 20 00

        const int NpcDataSize = 10;

        //var newObjectList = newObjects.ToList();
        //var packet = new byte[(newObjectList.Count * NpcDataSize) + 5];
        //packet[0] = 0xC2;
        //packet[1] = (byte)(packet.Length >> 8 & 0xFF);
        //packet[2] = (byte)(packet.Length & 0xFF);
        //packet[3] = 0x13; ////Packet Id
        //packet[4] = (byte)newObjectList.Count;

        var count = packet.ReadByte();

        while (count > 0)
        {
            var mobId = packet.ReadShort(false);
            var mobType = packet.ReadShort(false);

            ////Mob Type:
            //var npcStats = npc.Definition;
            //if (npcStats != null)
            //{
            //    packet[7 + monsterOffset] = (byte)((npcStats.Number >> 8) & 0xFF);
            //    packet[8 + monsterOffset] = (byte)(npcStats.Number & 0xFF);
            //}


            //Coords:

            var x = packet.ReadByte();
            var y = packet.ReadByte();

            var targetX = packet.ReadByte();
            var targetY = packet.ReadByte();

            var rotation = (Direction)(packet.ReadByte() >> 4);//packet[13 + monsterOffset] = (byte)((int)npc.Rotation << 4);
            var effectsCount = packet.ReadByte();//14 = offset byte for magic effects - currently we don't show them for NPCs

            count--;

            log.Info($"NPC here: {mobType} on {x},{y}");
        }
    }
}
