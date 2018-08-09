using UnityEngine;
using UnityEngine.UI;
using System;

public enum AlertResult
{
    Ok, Cancel
}

public class AlertPopup : BasePopup
{
    [SerializeField]
    private Text _text;

    [SerializeField]
    private GameObject _okBtn, _cancelBtn;

    public event Action OkClicked, CancelClicked;

    void Awake()
    {
        _okBtn.SetOnClick(OnOkButtonClick);
        _cancelBtn.SetOnClick(OnCancelButtonClick);
    }

    public void SetText(string text)
    {
        _text.text = text;
    }

    private void OnOkButtonClick(GameObject sender)
    {
        if (OkClicked != null)
            OkClicked();
    }

    private void OnCancelButtonClick(GameObject sender)
    {
        if (CancelClicked != null)
            CancelClicked();
    }
}