using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PreloaderState : BaseState
{
    //[Inject]
    //public MainConfig MainConfig { get; private set; }
    [Inject]
    public PreloaderScreen PreloaderScreen { get; private set; }
    //[Inject]
    //public ICoroutineExecuter CoroutineExecuter { get; private set; }
    //[Inject]
    //public IStorageService StorageService { get; private set; }
    //[Inject]
    //public MainModel MainModel { get; private set; }
    //[Inject]
    //public LocalizationService LocalizationService { get; private set; }

    private Action _onFinished;

    public void SetFinishCallback(Action callback)
    {
        _onFinished = callback;
    }

    public override void Load()
    {
        PreloaderScreen.SetStatusText("Loading...");

        PreloaderScreen.Show();

        //UpdateConfig();
    }

    public override void Unload()
    {
        PreloaderScreen.Hide();
    }

    //private void UpdateConfig()
    //{
    //    NLog.Log("Start update config...");

    //    MainConfig.UpdateFromServer(error =>
    //    {
    //        if (error)
    //        {
    //            PreloaderScreen.SetStatusText(LocalizationService.Get("checkConnection"));

    //            UpdateConfig();

    //            return;
    //        }

    //        NLog.Log("Config updated");

    //        UpdateLocalization();
    //    });
    //}

    //private void UpdateLocalization()
    //{
    //    NLog.Log("Start update localization...");

    //    LocalizationService.UpdateFromServer(error =>
    //    {
    //        if (error)
    //        {
    //            PreloaderScreen.SetStatusText(LocalizationService.Get("checkConnection"));

    //            UpdateLocalization();

    //            return;
    //        }

    //        NLog.Log("Localization updated!");

    //        var queue = new Queue<SystemLanguage>(MainConfig.SupportedLanguages.Length);

    //        foreach (var lang in MainConfig.SupportedLanguages)
    //        {
    //            queue.Enqueue(lang);
    //        }

    //        UpdateDictionaries(queue);
    //    });
    //}

    //private void UpdateDictionaries(Queue<SystemLanguage> langsForLoading)
    //{
    //    var lang = langsForLoading.Peek();

    //    WordsDictionary.UpdateFromServer(LocalizationService.GetLangId(lang), (error) =>
    //    {
    //        NLog.Log("[WordsDictionary] Updated. With error? " + error);

    //        if (error)
    //        {
    //            PreloaderScreen.SetStatusText(LocalizationService.Get("checkConnection"));

    //            UpdateDictionaries(langsForLoading);

    //            return;
    //        }

    //        PreloaderScreen.SetStatusText("");

    //        langsForLoading.Dequeue();

    //        if (langsForLoading.Count > 0)
    //        {
    //            UpdateDictionaries(langsForLoading);
    //        }
    //        else
    //        {
    //            UpdateLevels();
    //        }
    //    });
    //}

    //private void UpdateLevels()
    //{
    //    NLog.Log("Start update levels...");

    //    LevelService.UpdateFromServer(error =>
    //    {

    //        NLog.Log("[LevelService] Updated. With error? " + error);

    //        if (error)
    //        {
    //            PreloaderScreen.SetStatusText(LocalizationService.Get("checkConnection"));

    //            UpdateLevels();

    //            return;
    //        }

    //        PreloaderScreen.SetStatusText("");

    //        if (_onFinished != null)
    //            _onFinished();
    //    });
    //}
}