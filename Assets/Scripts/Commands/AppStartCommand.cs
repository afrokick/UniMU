using strange.extensions.command.impl;
using UnityEngine;

public class AppStartCommand : Command
{
    [Inject]
    public MainModel MainModel { get; private set; }
    //[Inject]
    //public ICoroutineExecuter CoroutineExecuter { get; private set; }
    //[Inject]
    //public NotificationService NotificationService { get; private set; }
    //[Inject]
    //public IInternetService InternetService { get; private set; }
    //[Inject]
    //public IStateMachine StateMachine { get; private set; }
    //[Inject]
    //public IStorageService StorageService { get; private set; }

    public override void Execute()
    {
        Application.targetFrameRate = 30;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Logger.ConfigureAllLogging();
    }
}