using System.Collections.Generic;
using System.Linq;
using log4net;
using UniMU.Models;

public class NewPlayersInScopeHandler : IPacketHandler
{
    private readonly ILog log = LogManager.GetLogger(typeof(NewPlayersInScopeHandler));

    public byte HandlerCode => 0x12;

    [Inject]
    public NewPlayersInScopeSignal NewPlayersInScopeSignal { get; private set; }

    public void HandlePacket(object sender, Packet packet)
    {
        //C2 00 2A 12
        //01
        //AF 05
        //CB 26
        //00 05 0E 33 34 3F 15 C7 23 00 00 00 A0 C0 00 00 00 00
        //61 66 72 6F 6B 69 63 6B 00 00
        //CB 26
        //03 01 83

        //C2 01 3E 12
        //08
        //1)
        //2F 9A
        //C8 31
        //40 0F 00 EC CE CF 12 C7 40 00 00 00 80 80 00 00 00 00
        //50 72 6F 64 61 69 6B 61 00 00
        //C8 31
        //13 01 83
        //2)
        //31 7F
        //C5 2D
        //60 1F FF F5 55 5F 0D B6 06 84 04 79 00 FF 11 11 00 00
        //41 6E 74 61 72 61 73 00 00 00
        //C5 2D
        //03 03 81 83 3E
        //3)
        //31 DB
        //CF 2C
        //68 1C 2A F4 44 4C 1B 6C 36 F8 7E 00 00 CF 00 00 30 00
        //41 53 43 45 4E 44 49 4E 47 00
        //CF 2C 53 03 81 83 84 31 EA CA 26 10 0B 06 22 22 2F 12 49 26 FA 06 F9 A0 C0 00 00 00 00 53 61 6E 74 61 00 00 00 00 00 CA 26 13 04 81 83 3E 84 31 EF D6 28 10 08 FF 33 32 3F 16 ED 04 12 84 00 A0 F1 11 01 00 00 53 61 62 61 74 74 00 00 00 00 D6 28 73 02 81 83 32 59 CE 30 18 08 FF 33 33 3D 00 00 06 02 00 FD A4 F1 11 11 00 00 41 4E 45 4D 49 41 00 00 00 00 CE 30 33 04 81 83 84 3E 32 7A CE 2D 18 09 0F 33 33 3F 12 49 06 02 06 F9 A0 C1 11 11 00 00 54 73 65 66 65 79 00 00 00 00 CE 2D 33 04 81 83 3E 84 32 9D D5 2B 30 0C FF 11 11 1F 12 49 04 03 00 FD 00 F0 00 00 00 00 41 67 69 6C 61 79 00 00 00 00 D5 2B 43 04 81 83 84 3E




        //C2 00 29 12
        //01 - count
        //01 02 - id
        //87 78  - x y
        //20 00 FF FF FF F3 00 00 00 F8 00 00 20 FF FF FF 00 00 - (18) appearance
        //74 65 73 74 30 00 00 00 00 00 - (10) name
        //87 78 - x y 
        //00 - rotation + hero status
        //00 - effects count

        var count = packet.ReadByte();

        var list = new List<Character>(count);
        while (count > 0)
        {
            var c = new Character();

            c.Id = packet.ReadShort();

            c.PositionX = packet.ReadByte();
            c.PositionY = packet.ReadByte();

            packet.ReadOffset += 18;//packetList.AddRange(newPlayer.GetAppearanceData(this.appearanceSerializer)); // 9 ... 26
            c.Name = packet.ReadString(10); // 27 ... 36

            c.LastPositionX = packet.ReadByte();
            c.LastPositionY = packet.ReadByte();

            packet.ReadByte();//packetList.Add((byte)(((int)newPlayer.Rotation * 0x10) + GetStateValue(newPlayer.SelectedCharacter.State))); // 39

            //var activeEffects = newPlayer.MagicEffectList.GetVisibleEffects();
            var effectCount = packet.ReadByte();

            while (effectCount > 0)
            {
                var effectId = packet.ReadByte();
                effectCount--;
            }

            list.Add(c);

            count--;
        }

        //if (shopPlayers != null)
        //{
        //    this.player.PlayerView.ShowShopsOfPlayers(shopPlayers);
        //}

        //if (guildPlayers != null)
        //{
        //    this.player.PlayerView.GuildView.AssignPlayersToGuild(guildPlayers, true);
        //}

        log.Info($"[NewPlayersInScopeHandler] chars:\n{string.Join("\n", list.Select(s => s.ToString()))}");

        NewPlayersInScopeSignal.Dispatch(list);
    }
}
