using System;
using System.Collections.Generic;
using UniMU.Models;
using UnityEngine;

public class CharactersScreen : BasePopup
{
    [SerializeField]
    private GameObject _btnEnter, _btnDelete, _btnCreate;

    [SerializeField]
    private GameObject _charactersRoot, _characterViewItem;

    public Action EnterClicked;
    public Action CreateClicked;
    public Action DeleteClicked;

    public Action<int> SlotSelected;

    private List<CharacterViewItem> CharacterViewItems { get; } = new List<CharacterViewItem>();

    // Use this for initialization
    void Start()
    {
        _btnEnter.SetOnClick((sender) => EnterClicked?.Invoke());
        _btnCreate.SetOnClick((sender) => CreateClicked?.Invoke());
        _btnDelete.SetOnClick((sender) => DeleteClicked?.Invoke());
    }

    public void UpdateCharactersList(List<Character> characters)
    {
        CharacterViewItems.ForEach((viewItem) => Destroy(viewItem.gameObject));
        CharacterViewItems.Clear();

        characters.ForEach((model) =>
        {
            var newViewItem = Instantiate(_characterViewItem, _charactersRoot.transform).GetComponent<CharacterViewItem>();

            CharacterViewItems.Add(newViewItem);

            newViewItem.gameObject.SetActive(true);

            newViewItem.SetData(model.CharacterSlot, model.Name, "class", model.Level);
            newViewItem.SetSelection(false);

            newViewItem.SlotSelected = (viewItem) => SlotSelected?.Invoke(viewItem.SlotIndex);
        });
    }

    public void SetCreateButtonVisible(bool visible)
    {
        _btnCreate.SetActive(visible);
    }

    public void SetDeleteButtonVisible(bool visible)
    {
        _btnDelete.SetActive(visible);
    }

    public void SetEnterButtonVisible(bool visible)
    {
        _btnEnter.SetActive(visible);
    }

    public void SetSelection(int slot)
    {
        CharacterViewItems.ForEach((viewItem) =>
        {
            viewItem.SetSelection(viewItem.SlotIndex == slot);
        });
    }
}
