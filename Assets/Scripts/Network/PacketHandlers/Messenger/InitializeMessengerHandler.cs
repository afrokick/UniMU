public class InitializeMessengerHandler : IPacketHandler
{
    public byte HandlerCode => 0xC0;

    public void HandlePacket(object sender, Packet packet)
    {
        var letterCount = packet.ReadByte();
        var maxLetters = packet.ReadByte();
        var friendsListCount = packet.ReadByte();

        while (friendsListCount > 0)
        {
            var friend = packet.ReadString(10);
            packet.ReadByte();

            friendsListCount--;
        }

        //foreach (var requesterName in this.friendServer.GetOpenFriendRequests(this.player.SelectedCharacter.Id))
        //{
        //    this.ShowFriendRequest(requesterName);
        //}

        //for (ushort l = 0; l < letters.Count; l++)
        //{
        //    this.AddToLetterList(letters[l], l, false);
        //}
    }
}
