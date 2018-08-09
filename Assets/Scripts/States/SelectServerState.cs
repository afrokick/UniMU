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
    [Inject]
    public OpenLoginScreenSignal OpenLoginScreenSignal { get; private set; }

    private ServerListInfoModel ServerListInfoModel { get; set; }

    public override void Load()
    {
        SelectServerScreen.SelectClicked = ViewOnSelectClicked;

        SelectServerScreen.Show();

        ServerListUpdatedSignal.AddListener(OnServerListUpdated);
        ServerListItemUpdatedSignal.AddListener(OnServerListItemUpdated);

        CSClient.Connect();

        if (CSClient.Connected)
        {
            CSClient.RequestServerList();
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

    private void OnServerListItemUpdated(ServerListItemInfoModel model)
    {
        MainModel.SelectedServerInfo = model;

        Debug.Log($"connect to {model.Ip}:{model.Port}");

        GSClient.Connect(model.Ip, model.Port);

        CSClient.Disconnect();

        OpenLoginScreenSignal.Dispatch();
    }

    private void ViewOnSelectClicked()
    {
        var serverId = ServerListInfoModel.Servers[0].ServerId;

        CSClient.GetServerInfo(serverId);

        Debug.Log("send get info");
    }
}