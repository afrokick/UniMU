public class LoadingCharacterState : BaseState
{
    [Inject]
    public MainModel MainModel { get; private set; }
    [Inject]
    public IGSClient GSClient { get; private set; }

    [Inject]
    public CharacterSelectedSignal CharacterSelectedSignal { get; private set; }

    [Inject]
    public OpenWorldSignal OpenWorldSignal { get; private set; }

    [Inject]
    public LoadingCharacterScreen LoadingCharacterScreen { get; private set; }

    public async override void Load()
    {
        LoadingCharacterScreen.Show();

        CharacterSelectedSignal.AddListener(OnCharacterSelected);

        await GSClient.SelectCharacter(MainModel.SelectedCharacter.Name);
    }

    public override void Unload()
    {
        LoadingCharacterScreen.Hide();

        CharacterSelectedSignal.RemoveListener(OnCharacterSelected);
    }

    private void OnCharacterSelected()
    {
        UnityEngine.Debug.Log($"player:\n{MainModel.SelectedCharacter}");
        UnityEngine.Debug.Log($"go to map {MainModel.SelectedMap}");

        OpenWorldSignal.Dispatch();
    }
}