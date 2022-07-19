using System;
using UnityEngine;

public class SelectServerScreen : BasePopup
{
    [SerializeField]
    private GameObject _btnConnect;

    public Action SelectClicked;

    // Use this for initialization
    void Start()
    {
        _btnConnect.SetOnClick(OnSelectClicked);
    }

    public void UpdateServerList(ServerListInfoModel model)
    {
        _btnConnect.SetActive(model.Servers.Count > 0);
    }

    public void OnSelectClicked(GameObject sender)
    {
        SelectClicked?.Invoke();
    }
}
