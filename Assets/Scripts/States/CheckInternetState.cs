//public class CheckInternetState : BaseAdditionalState
//{
//    [Inject]
//    public IInternetService InternetService { get; private set; }

//    [Inject]
//    public CheckInternetPopup CheckInternetPopup { get; private set; }

//    [Inject]
//    public InternetStateChangedSignal InternetStateChangedSignal { get; private set; }

//    public override void Load()
//    {
//        base.Load();

//        CheckInternetPopup.CloseClicked += OnCloseClicked;

//        InternetStateChangedSignal.AddListener(OnInternetStateChanged);

//        CheckInternetPopup.Show();
//    }

//    public override void Unload()
//    {
//        base.Unload();

//        CheckInternetPopup.CloseClicked -= OnCloseClicked;

//        InternetStateChangedSignal.RemoveListener(OnInternetStateChanged);

//        CheckInternetPopup.Hide();
//    }

//    private void OnCloseClicked()
//    {
//        if (StateMachine.LastState == this)
//            StateMachine.Unload(false);
//    }

//    private void OnInternetStateChanged(bool connected)
//    {
//        if (connected && StateMachine.LastState == this)
//            StateMachine.Unload(false);
//    }
//}