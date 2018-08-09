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

    private void ViewOnLoginClicked(string username, string password)
    {
        GSClient.Login(username, password);
    }
}