using UniMU.Models;

public class CreateCharacterState : BaseAdditionalState
{
    [Inject]
    public IGSClient GSClient { get; private set; }

    [Inject]
    public CharacterCreatedSignal CharacterCreatedSignal { get; private set; }

    [Inject]
    public CreateCharacterPopup CreateCharacterPopup { get; private set; }

    public override void Load()
    {
        base.Load();

        CharacterCreatedSignal.AddListener(OnCharacterCreated);

        CreateCharacterPopup.CreateClicked = ViewOnCreateClicked;
        CreateCharacterPopup.CloseClicked = ViewOnCloseClicked;

        CreateCharacterPopup.Show();
    }

    public override void Unload()
    {
        base.Unload();

        CharacterCreatedSignal.RemoveListener(OnCharacterCreated);

        CreateCharacterPopup.Hide();
    }

    private void OnCharacterCreated(bool success, Character character)
    {
        if (success)
        {
            StateMachine.Unload(false);
        }
    }

    private async void ViewOnCreateClicked(string characterName, byte classId)
    {
        await GSClient.CreateCharacter(characterName, classId);
    }

    private void ViewOnCloseClicked()
    {
        StateMachine.Unload(false);
    }
}
