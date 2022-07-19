public class LoginState : BaseState
{
    [Inject]
    public LoginScreen LoginScreen { get; private set; }
    [Inject]
    public IGSClient GSClient { get; private set; }

    public override void Load()
    {
        LoginScreen.LoginClicked = ViewOnLoginClicked;

        LoginScreen.Show();
    }

    public override void Unload()
    {
        LoginScreen.Hide();

        LoginScreen.LoginClicked = null;
    }

    private async void ViewOnLoginClicked(string username, string password)
    {
        UnityEngine.Debug.Log($"try login with '{username}' '{password}'");
        await GSClient.Login(username, password);
    }
}