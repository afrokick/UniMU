using UnityEngine;

public class SelectServerState : BaseState
{
    [Inject]
    public MainModel MainModel { get; private set; }
    [Inject]
    public SelectServerScreen SelectServerScreen { get; private set; }
    [Inject]
    public ICSClient CSClient { get; private set; }
    [Inject]
    public IGSClient GSClient { get; private set; }
    [Inject]
    public ServerListUpdatedSignal ServerListUpdatedSignal { get; private set; }
    [Inject]
    public ServerListItemUpdatedSignal ServerListItemUpdatedSignal { get; private set; }

    private ServerListInfoModel ServerListInfoModel { get; set; }

    public override async void Load()
    {
        SelectServerScreen.SelectClicked = ViewOnSelectClicked;

        SelectServerScreen.Show();

        ServerListUpdatedSignal.AddListener(OnServerListUpdated);
        ServerListItemUpdatedSignal.AddListener(OnServerListItemUpdated);

        await CSClient.Connect("127.0.0.1", 44405);

        if (CSClient.Connected)
        {
            await CSClient.RequestServerList();
        }
        else
        {
            Debug.LogError("cs seems disconnected");
        }
    }

    public override void Unload()
    {
        SelectServerScreen.Hide();

        SelectServerScreen.SelectClicked = null;

        ServerListUpdatedSignal.RemoveListener(OnServerListUpdated);
        ServerListItemUpdatedSignal.RemoveListener(OnServerListItemUpdated);
    }

    private void OnServerListUpdated(ServerListInfoModel model)
    {
        ServerListInfoModel = model;

        SelectServerScreen.UpdateServerList(model);
    }

    private async void OnServerListItemUpdated(ServerListItemInfoModel model)
    {
        Debug.Log($"connect to {model.Ip}:{model.Port}");

        MainModel.SelectedServerInfo = model;

        await GSClient.Connect(model.Ip, model.Port);

        CSClient.Disconnect();
    }

    private async void ViewOnSelectClicked()
    {
        var serverId = ServerListInfoModel.Servers[0].ServerId;

        await CSClient.GetServerInfo(serverId);
    }
}