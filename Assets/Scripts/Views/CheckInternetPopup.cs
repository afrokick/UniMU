#pragma warning disable 0649

using UnityEngine;
using System;
using UnityEngine.UI;

public class CheckInternetPopup : BasePopup
{
    [SerializeField]
    private Button _closeBtn;

    public event Action CloseClicked = delegate { };

    void Awake()
    {
        _closeBtn.SetOnClick(OnCloseButtonClick);
    }

    private void OnCloseButtonClick(GameObject sender)
    {
        CloseClicked();
    }
}