using System;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharacterPopup : BasePopup
{
    [SerializeField]
    private InputField _inputName;

    [SerializeField]
    private GameObject _btnCreate, _btnClose;

    public Action<string, byte> CreateClicked { get; set; }
    public Action CloseClicked { get; set; }

    // Use this for initialization
    void Start()
    {
        _btnCreate.SetOnClick(sender =>
        {
            var newName = _inputName.text;
            byte classId = 4; // (byte)CharacterClassNumber.DarkKnight;
            if (newName.Length < 4 || newName.Length > 10)
            {
                Debug.LogError("name length must be from 4 to 10 symbols");
                return;
            }

            CreateClicked?.Invoke(newName, classId);
        });

        _btnClose.SetOnClick(sender => CloseClicked?.Invoke());
    }
}
