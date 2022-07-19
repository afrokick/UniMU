public class ServerListItemInfoModel
{
    public string Ip { get; set; }
    public ushort Port { get; set; }

    public override string ToString()
    {
        return $"ip:{Ip} port:{Port}";
    }
}