public class PreloaderState : BaseState
{
    [Inject]
    public PreloaderScreen PreloaderScreen { get; private set; }

    public override void Load()
    {
        PreloaderScreen.SetStatusText("Loading...");

        PreloaderScreen.Show();
    }

    public override void Unload()
    {
        PreloaderScreen.Hide();
    }
}