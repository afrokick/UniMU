using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectServerScreen : BasePopup
{
    [SerializeField]
    private GameObject _btnConnect;

    // Use this for initialization
    void Start()
    {

    }

    public void UpdateServerList(ServerListInfoModel model)
    {
        _btnConnect.SetActive(model.Servers.Count > 0);
    }

    public void OnSelectClicked()
    {

    }
}
