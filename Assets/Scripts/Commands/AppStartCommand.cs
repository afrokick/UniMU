using System;
using System.Linq;
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
    [Inject]
    public IStateMachine StateMachine { get; private set; }
    //[Inject]
    //public IStorageService StorageService { get; private set; }

    private byte[] FromHexToPacket(string hex)
    {
        return hex.Split('-', ' ').Select(s => Convert.ToByte(s, 16)).ToArray();
    }

    uint GetIntegerBigEndian(byte[] data)
    {
        return unchecked((uint)(data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24)));
    }

    public override void Execute()
    {
        Application.targetFrameRate = 30;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Logger.ConfigureAllLogging();
    }
}