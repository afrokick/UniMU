using System.Collections.Generic;
using System.Linq;
using UniMU.Models;
using UnityEngine;

public class CharactersState : BaseState
{
    [Inject]
    public MainModel MainModel { get; private set; }
    [Inject]
    public CharactersScreen CharactersScreen { get; private set; }
    [Inject]
    public IGSClient GSClient { get; private set; }

    [Inject]
    public CharactersListUpdatedSignal CharactersListUpdatedSignal { get; private set; }
    [Inject]
    public CharacterFocusedSignal CharacterFocusedSignal { get; private set; }
    [Inject]
    public CharacterCreatedSignal CharacterCreatedSignal { get; private set; }
    [Inject]
    public CharacterDeletedSignal CharacterDeletedSignal { get; private set; }

    [Inject]
    public OpenCreateCharacterPopupSignal OpenCreateCharacterPopupSignal { get; private set; }
    [Inject]
    public OpenLoadingCharacterScreenSignal OpenLoadingCharacterScreenSignal { get; private set; }

    private List<Character> Characters { get; set; }

    public override async void Load()
    {
        CharactersScreen.EnterClicked = ViewOnEnterClicked;
        CharactersScreen.CreateClicked = ViewOnCreateClicked;
        CharactersScreen.DeleteClicked = ViewOnDeleteClicked;
        CharactersScreen.SlotSelected = ViewOnSlotSelected;

        CharactersScreen.Show();

        CharactersListUpdatedSignal.AddListener(OnCharactersListUpdated);
        CharacterFocusedSignal.AddListener(OnCharacterFocused);
        CharacterCreatedSignal.AddListener(OnCharacterCreated);
        CharacterDeletedSignal.AddListener(OnCharacterDeleted);

        await GSClient.GetCharactersList();
    }

    public override void Unload()
    {
        CharactersScreen.Hide();

        CharactersListUpdatedSignal.RemoveListener(OnCharactersListUpdated);
        CharacterFocusedSignal.RemoveListener(OnCharacterFocused);
        CharacterCreatedSignal.RemoveListener(OnCharacterCreated);
        CharacterDeletedSignal.RemoveListener(OnCharacterDeleted);

        CharactersScreen.EnterClicked = null;
    }

    private void OnCharactersListUpdated(List<Character> characters)
    {
        Characters = characters;

        CharactersScreen.SetSelection(-1);
        CharactersScreen.SetEnterButtonVisible(false);
        CharactersScreen.SetDeleteButtonVisible(false);

        CharactersScreen.UpdateCharactersList(characters);
        CharactersScreen.SetCreateButtonVisible(Characters.Count < 5);
    }

    private void ViewOnCreateClicked()
    {
        if (Characters.Count >= 5) return;

        OpenCreateCharacterPopupSignal.Dispatch();
    }

    private async void ViewOnDeleteClicked()
    {
        if (MainModel.SelectedCharacter == null) return;

        await GSClient.DeleteCharacter(MainModel.SelectedCharacter.Name, "nocode");
    }

    private async void ViewOnSlotSelected(int slot)
    {
        var character = Characters.First(c => c.CharacterSlot == slot);

        MainModel.SelectedCharacter = character;

        await GSClient.FocusCharacter(character.Name);
    }

    private void ViewOnEnterClicked()
    {
        OpenLoadingCharacterScreenSignal.Dispatch();
    }

    private void OnCharacterFocused(string characterName)
    {
        if (MainModel.SelectedCharacter.Name != characterName) return;

        CharactersScreen.SetSelection(MainModel.SelectedCharacter.CharacterSlot);
        CharactersScreen.SetEnterButtonVisible(true);
        CharactersScreen.SetDeleteButtonVisible(true);
    }

    private async void OnCharacterCreated(bool success, Character character)
    {
        if (!success) return;

        Characters.Add(character);

        OnCharactersListUpdated(Characters);
    }

    private async void OnCharacterDeleted(bool success)
    {
        if (!success) return;

        Characters.Remove(MainModel.SelectedCharacter);

        OnCharactersListUpdated(Characters);
    }

}