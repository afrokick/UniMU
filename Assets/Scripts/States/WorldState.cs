public class WorldState : BaseState
{
    [Inject]
    public MainModel MainModel { get; private set; }
    [Inject]
    public IGSClient GSClient { get; private set; }

    //[Inject]
    //public LoadingCharacterScreen LoadingCharacterScreen { get; private set; }

    public async override void Load()
    {
        //LoadingCharacterScreen.Show();

        //UnityEngine.SceneManagement.SceneManager.LoadScene("MuWorld", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public override void Unload()
    {
        //LoadingCharacterScreen.Hide();
    }
}