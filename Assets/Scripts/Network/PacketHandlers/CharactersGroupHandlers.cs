using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MUnique.OpenMU.GameLogic.Attributes;
using UniMU.Models;

public class CharactersGroupHandlers : IPacketHandler
{
    private const byte LevelMask = 0x78;

    public byte HandlerCode => 0xF3;

    private readonly ILog log = LogManager.GetLogger(typeof(CharactersGroupHandlers));

    private Dictionary<byte, IPacketHandler> Handlers { get; } = new Dictionary<byte, IPacketHandler>();

    [Inject]
    public MainModel MainModel { get; private set; }

    [Inject]
    public CharactersListUpdatedSignal CharactersListUpdatedSignal { get; private set; }
    [Inject]
    public CharacterFocusedSignal CharacterFocusedSignal { get; private set; }
    [Inject]
    public CharacterCreatedSignal CharacterCreatedSignal { get; private set; }
    [Inject]
    public CharacterDeletedSignal CharacterDeletedSignal { get; private set; }
    [Inject]
    public CharacterSelectedSignal CharacterSelectedSignal { get; private set; }

    [PostConstruct]
    public void Initialize()
    {

    }

    public void HandlePacket(object sender, Packet packet)
    {
        var subCode = packet.SubCode;
        packet.ReadByte();

        switch (subCode)
        {
            case 0x00:
                HandleCharactersList(packet);
                break;
            case 0x01:
                HandleCreateCharacter(packet);
                break;
            case 0x02:
                HandleDeleteCharacter(packet);
                break;
            case 0x15:
                HandleFocusCharacter(packet);
                break;
            case 0x03:
                HandleSelectCharacter(packet);
                break;
            //case 0x06:
            //    this.ReadIncreaseStats(player, packet);
            //    break;
            case 0x10:
                HandleUpdateInventoryList(packet);
                break;
            case 0x11:
                HandleUpdateSkillList(packet);
                break;
            //case 0x12: ////Data Loaded by Client
            //    player.ClientReadyAfterMapChange();
            //    break;
            //case 0x30: ////GCSkillKeyRecv
            //    this.saveKeyConfigurationAction.SaveKeyConfiguration(player, packet.Skip(4).ToArray());
            //    break;
            //case 0x52:
            //this.AddMasterSkillPoint(player, packet);
            //break;
            default:
                log.Error($"unhandled subCode {subCode}");
                break;
        }
    }

    private void HandleCharactersList(Packet packet)
    {
        //C1 4B F3 00
        //03 - unlocked classes
        //00 - move cnt
        //02 - characters count
        //00 | 61 66 72 6F 6B 69 63 6B 00 00 | 00 88 | 00 00 | 00 05 0E 33 34 3F 15 C7 23 00 00 00 A0 C0 00 00 00 00 | FF
        //01 | 47 65 6E 61 00 00 00 00 00 00 | 00 79 | 00 00 | 20 06 FF 58 19 9F 00 41 00 00 00 00 40 F0 00 00 00 00 | FF
        var list = new List<Character>();

        const int characterSize = 34;

        byte unlockedCharacterClassesFlags = packet.ReadByte();
        byte moveCnt = packet.ReadByte();
        byte charactersCount = packet.ReadByte();

        //packet.ReadByte(); // packet[7] ??? new in season 6 - probably vault extension

        while (charactersCount > 0)
        {
            byte characterSlot = packet.ReadByte();
            string characterName = packet.ReadString(10);

            packet.ReadByte();//?

            var level = packet.ReadShort(false);

            var characterState = packet.ReadByte(); // status & item block?

            //18 length

            //var classAndPose = packet.ReadByte();//1
            //var leftHandId = packet.ReadByte();//2
            //var rightHandId = packet.ReadByte();//3

            //var preview = this.appearanceSerializer.GetAppearanceData(new CharacterAppearanceDataAdapter(character));
            //Buffer.BlockCopy(preview, 0, packet, offset + 15, preview.Length);

            packet.ReadOffset += 18;

            //TODO: not implemented on server
            var guildStatus = packet.ReadByte();

            charactersCount--;

            list.Add(new Character
            {
                CharacterSlot = characterSlot,
                Name = characterName,
                State = (HeroState)characterState,
                Level = level,
            });
        }

        log.Info($"characters:\n{string.Join("\n", list.Select(s => s.ToString()))}");

        CharactersListUpdatedSignal.Dispatch(list);
    }

    private void HandleCreateCharacter(Packet packet)
    {
        var statusCode = packet.ReadByte();

        var success = statusCode == 1;

        Character character = null;

        if (success)
        {
            character = new Character
            {
                Name = packet.ReadString(10),
                CharacterSlot = packet.ReadByte(),
                Level = packet.ReadShort(false),
            };
        }


        CharacterCreatedSignal.Dispatch(success, character);
    }

    private void HandleDeleteCharacter(Packet packet)
    {
        var statusCode = packet.ReadByte();

        var success = statusCode == 1;

        CharacterDeletedSignal.Dispatch(success);
    }

    private void HandleFocusCharacter(Packet packet)
    {
        var characterName = packet.ReadString(10);

        CharacterFocusedSignal.Dispatch(characterName);
    }

    private void HandleSelectCharacter(Packet packet)
    {
        //C3 68 F3 03
        //C8 2F - x y
        //02 00 - map id
        //00 00 00 00 01 95 BA CC - exp
        //00 00 00 00 01 99 3A 80 - next level exp
        //12 00 - points
        //E6 00 - str
        //4E 00 - agi
        //0F 00 - vit
        //F4 01 - ene
        //00 00 - cur hp
        //00 00 - max hp
        //00 00 - cur mana
        //00 00 - max mana
        //00 00 - cur shield
        //00 00 - max shield
        //00 00 - cur abi
        //00 00 - max abi
        //00 00 - ?
        //77 5D 78 00 - money
        //03 - hero state
        //00 - status
        //55 00 1D 00 00 00 00 00 1D 00 BB 01 00 00 BB 01 00 00 CF 06 00 00 CF 06 00 00 F6 04 00 00 F6 04 00 00 5A 00 00 00 B5 00 00 00 12 00 00 00
        var character = MainModel.SelectedCharacter;

        character.PositionX = packet.ReadByte();
        character.PositionY = packet.ReadByte();

        var mapNumber = packet.ReadShort(false);
        MainModel.SelectedMap = (mapNumber + 1);

        character.Experience = packet.ReadLong();
        character.NextLevelExperience = packet.ReadLong();

        character.LevelUpPoints = packet.ReadShort(false);

        character.Attributes[Stats.BaseStrength] = packet.ReadShort(false);
        character.Attributes[Stats.BaseAgility] = packet.ReadShort(false);
        character.Attributes[Stats.BaseVitality] = packet.ReadShort(false);
        character.Attributes[Stats.BaseEnergy] = packet.ReadShort(false);

        character.Attributes[Stats.CurrentHealth] = packet.ReadShort(false);
        character.Attributes[Stats.MaximumHealth] = packet.ReadShort(false);
        character.Attributes[Stats.CurrentMana] = packet.ReadShort(false);
        character.Attributes[Stats.MaximumMana] = packet.ReadShort(false);
        character.Attributes[Stats.CurrentShield] = packet.ReadShort(false);
        character.Attributes[Stats.MaximumShield] = packet.ReadShort(false);
        character.Attributes[Stats.CurrentAbility] = packet.ReadShort(false);
        character.Attributes[Stats.MaximumAbility] = packet.ReadShort(false);

        packet.ReadOffset += 2;// 2 missing bytes here are padding

        character.Money = (int)packet.ReadInt(false);

        character.PlayerKillCount = packet.ReadByte();
        character.State = (HeroState)packet.ReadByte();

        character.UsedFruitPoints = packet.ReadShort(false);
        packet.ReadShort(false);// TODO: MaxFruits, calculate the right value
        character.Attributes[Stats.BaseLeadership] = packet.ReadShort(false);
        character.UsedNegFruitPoints = packet.ReadShort(false);
        packet.ReadShort(false); // TODO: MaxNegFruits, calculate the right value
        //packet[68] = this.player.Account.IsVaultExtended ? (byte)1 : (byte)0;
        packet.ReadByte();

        CharacterSelectedSignal.Dispatch();
    }

    private void HandleUpdateInventoryList(Packet packet)
    {
        var itemSize = 12;
        //C4 02 9D F3 10
        //33 - items count
        //00 | 05 38 4C 00 00 50 00 FF FF FF FF FF
        //01 | 0E 50 43 00 00 60 00 FF FF FF FF FF 02 03 48 35 00 00 70 00 FF FF FF FF FF 03 03 45 33 00 00 80 00 FF FF FF FF FF 04 03 50 3A 00 00 90 00 FF FF FF FF FF 05 04 44 26 00 00 A0 00 FF FF FF FF FF 06 03 59 3D 00 00 B0 00 FF FF FF FF FF 09 0C 48 40 00 00 D0 00 FF FF FF FF FF 0A 14 00 00 00 00 D0 00 FF FF FF FF FF 0C 02 00 32 00 00 E0 00 FF FF FF FF FF 0D 02 00 32 00 00 E0 00 FF FF FF FF FF 0E 02 00 0D 00 00 E0 00 FF FF FF FF FF 14 05 00 32 00 00 E0 00 FF FF FF FF FF 15 05 00 32 00 00 E0 00 FF FF FF FF FF 16 05 00 32 00 00 E0 00 FF FF FF FF FF 17 05 00 32 00 00 E0 00 FF FF FF FF FF 18 05 00 32 00 00 E0 00 FF FF FF FF FF 19 05 00 32 00 00 E0 00 FF FF FF FF FF 1A 05 00 16 00 00 E0 00 FF FF FF FF FF 1C 05 00 32 00 00 E0 00 FF FF FF FF FF 1D 05 00 32 00 00 E0 00 FF FF FF FF FF 1E 05 00 32 00 00 E0 00 FF FF FF FF FF 1F 05 00 32 00 00 E0 00 FF FF FF FF FF 20 05 00 32 00 00 E0 00 FF FF FF FF FF 21 05 00 32 00 00 E0 00 FF FF FF FF FF 24 05 00 32 00 00 E0 00 FF FF FF FF FF 25 05 00 32 00 00 E0 00 FF FF FF FF FF 26 05 00 32 00 00 E0 00 FF FF FF FF FF 27 05 00 32 00 00 E0 00 FF FF FF FF FF 28 05 00 32 00 00 E0 00 FF FF FF FF FF 29 05 00 32 00 00 E0 00 FF FF FF FF FF 2C 05 00 32 00 00 E0 00 FF FF FF FF FF 2D 05 00 32 00 00 E0 00 FF FF FF FF FF 2E 05 00 32 00 00 E0 00 FF FF FF FF FF 34 05 00 32 00 00 E0 00 FF FF FF FF FF 35 05 00 32 00 00 E0 00 FF FF FF FF FF 36 05 00 32 00 00 E0 00 FF FF FF FF FF 3C 05 00 32 00 00 E0 00 FF FF FF FF FF 3D 05 00 32 00 00 E0 00 FF FF FF FF FF 3E 05 00 32 00 00 E0 00 FF FF FF FF FF 44 05 00 32 00 00 E0 00 FF FF FF FF FF 45 05 00 32 00 00 E0 00 FF FF FF FF FF 46 05 00 32 00 00 E0 00 FF FF FF FF FF 4C 08 00 00 00 00 F0 00 FF FF FF FF FF 4D 08 00 00 00 00 F0 00 FF FF FF FF FF 4E 08 00 00 00 00 F0 00 FF FF FF FF FF 4F 08 00 00 00 00 F0 00 FF FF FF FF FF 50 08 00 00 00 00 F0 00 FF FF FF FF FF 51 08 00 00 00 00 F0 00 FF FF FF FF FF 5C 09 00 00 00 00 F0 00 FF FF FF FF FF 6B 07 00 00 00 00 C0 00 FF FF FF FF FF

        var itemCount = packet.ReadByte();

        string str = "";

        while (itemCount > 0)
        {
            var itemSlot = packet.ReadByte();
            packet.ReadOffset += itemSize;

            var array = new byte[itemSize];
            Array.Copy(packet.Data, packet.ReadOffset - itemSize, array, 0, itemSize);

            var itemNumber = array[0] + ((array[0] & 0x80) << 1);
            var itemGroup = (array[5] & 0xF0) >> 4;
            //var definition = DataInitialization.Instance.gameConfiguration.Items.FirstOrDefault(def => def.Number == itemNumber && def.Group == itemGroup);

            //if (definition == null)
            //{
            //    log.Warn($"Couldn't find the item definition for the given byte array. Extracted item number and group: {itemNumber}, {itemGroup}");
            //}

            //var item = persistenceContext.CreateNew<Item>();
            //item.Definition = definition;
            var level = (byte)((array[1] & LevelMask) >> 3);
            var durability = array[2];

            var name = $"{itemNumber}-{itemGroup}";

            itemCount--;

            str += $"{itemSlot}) {name} +{level}({durability})\n";
        }

        log.Info($"UpdateInventoryList:\n{str}");
    }

    public void HandleUpdateSkillList(Packet packet)
    {
        var skillsCount = packet.ReadByte();

        packet.ReadByte();

        while (skillsCount > 0)
        {
            var index = packet.ReadByte();
            var id = packet.ReadShort();
            var level = packet.ReadByte();

            skillsCount--;
        }
    }
}
