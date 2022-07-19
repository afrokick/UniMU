using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterViewItem : MonoBehaviour
{
    [SerializeField]
    private Text _textName, _textLevel, _textClassName;

    [SerializeField]
    private GameObject _btnSelect, _selection;

    public Action<CharacterViewItem> SlotSelected;

    public int SlotIndex { get; private set; }

    private void Start()
    {
        _btnSelect.SetOnClick((sender) => SlotSelected?.Invoke(this));
    }

    public void SetData(int slotIndex, string name, string className, int level)
    {
        SlotIndex = slotIndex;

        _textName.text = name;
        _textLevel.text = $"{level}lvl";
        _textClassName.text = className;
    }

    public void SetSelection(bool enable)
    {
        _selection.SetActive(enable);
    }
}
