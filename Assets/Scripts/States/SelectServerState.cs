using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SelectServerState : BaseState
{
    [Inject]
    public SelectServerScreen SelectServerScreen { get; private set; }
    [Inject]
    public ICSClient CSClient { get; private set; }
    [Inject]
    public ServerListUpdatedSignal ServerListUpdatedSignal { get; private set; }

    public override void Load()
    {
        SelectServerScreen.Show();

        ServerListUpdatedSignal.AddListener(OnServerListUpdated);

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

        ServerListUpdatedSignal.RemoveListener(OnServerListUpdated);
    }


    private void OnServerListUpdated(ServerListInfoModel model)
    {
        SelectServerScreen.UpdateServerList(model);
    }

}