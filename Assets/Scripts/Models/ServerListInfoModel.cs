using System.Collections;
using System.Collections.Generic;

public class ServerListInfoModel
{
    public IReadOnlyList<ServerListInfoItemModel> Servers { get; }

    public ServerListInfoModel(IEnumerable<ServerListInfoItemModel> servers)
    {
        Servers = new List<ServerListInfoItemModel>(servers);
    }
}

public class ServerListInfoItemModel
{
    public ushort ServerId { get; set; }
    public byte Usage { get; set; }
}
