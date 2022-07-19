using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ServerListInfoModel
{
    public IReadOnlyList<ServerListInfoItemModel> Servers { get; }

    public ServerListInfoModel(IEnumerable<ServerListInfoItemModel> servers)
    {
        Servers = new List<ServerListInfoItemModel>(servers);
    }

    public override string ToString()
    {
        return $"Servers:\n{string.Join("\n", Servers.Select(server => server.ToString()))}";
    }
}

public class ServerListInfoItemModel
{
    public ushort ServerId { get; set; }
    public byte Usage { get; set; }

    public override string ToString()
    {
        return $"ServerListInfoItemModel: {ServerId} - {Usage}";
    }
}
