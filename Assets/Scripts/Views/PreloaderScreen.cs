using UnityEngine;
using UnityEngine.UI;

public class PreloaderScreen : BasePopup
{
    [SerializeField]
    private Text _stateText;

    void Start()
    {

    }

    public void SetStatusText(string text)
    {
        _stateText.text = text;
    }
}