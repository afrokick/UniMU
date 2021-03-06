using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainContext : SignalContext
{
    private readonly Transform _uiRoot;

    private readonly Dictionary<string, BasePopup> _childs;

    public MainContext(Transform uiRoot, MonoBehaviour view)
        : base(view)
    {
        _uiRoot = uiRoot;

        var count = _uiRoot.childCount;

        _childs = new Dictionary<string, BasePopup>(count);

        if (count == 0) return;

        Transform child = null;

        for (int i = 0; i < _uiRoot.childCount; i++)
        {
            child = _uiRoot.GetChild(i);

            _childs.Add(child.name, child.GetComponent<BasePopup>());
        }
    }

    protected override void mapBindings()
    {
        base.mapBindings();

        injectionBinder.Bind<MainConfig>().ToSingleton();
        injectionBinder.Bind<MainModel>().ToSingleton();

        injectionBinder.Bind<ICSClient>().To<CSClient>().ToSingleton();
        injectionBinder.Bind<IGSClient>().To<GSClient>().ToSingleton();

        //NETWORK
        injectionBinder.Bind<PacketHandler>().ToSingleton();

        injectionBinder.Bind<SayHelloHandler>().ToSingleton();
        injectionBinder.Bind<CSHandlers>().ToSingleton();
        injectionBinder.Bind<GetServerListHandler>().ToSingleton();
        injectionBinder.Bind<GetServerInfoHandler>().ToSingleton();

        //injectionBinder.Bind<IInternetService>().To<InternetService>().ToSingleton();
        injectionBinder.Bind<LocalizationService>().ToSingleton();

        //injectionBinder.Bind<GameManager>().ToSingleton();
        injectionBinder.Bind<IStorageService>().To<StorageService>().ToSingleton();
        injectionBinder.Bind<ICoroutineExecuter>().To<CoroutineExecuter>().ToSingleton();
        injectionBinder.Bind<IStateMachine>().To<StateMachine>().ToSingleton();
        //injectionBinder.Bind<SoundService>().ToValue(GameObject.FindObjectOfType<SoundService>());



        BindStates();
        BindViews();

        //signals and command
        commandBinder.Bind<AppQuitSignal>();
        commandBinder.Bind<AppStartSignal>()
            .InSequence()
            .To<AppStartCommand>()
            .To<OpenPreloaderCommand>()
                     .To<OpenSelectServerCommand>()
            .Once();

        commandBinder.Bind<HardwareBackPressSignal>();

        commandBinder.Bind<OpenSelectServerScreenSignal>()
            .InSequence()
                     .To<OpenSelectServerCommand>();


        //commandBinder.Bind<OpenLevelsMenuSignal>()
        //.InSequence()
        //.To<OpenLevelsMenuCommand>();

        commandBinder.Bind<ServerListUpdatedSignal>();

        //commandBinder.Bind<InternetStateChangedSignal>();
        //commandBinder.Bind<OpenCheckInternetSignal>().To<OpenCheckInternetCommand>();

        commandBinder.Bind<OpenAlertSignal>().To<OpenAlertCommand>();
    }

    private void BindStates()
    {
        BindAsSelf<PreloaderState>();
        BindAsSelf<SelectServerState>();
        BindAsSelf<GameState>();
        BindAsSelf<AlertState>();
    }

    private void BindAsSelf<T>()
    {
        injectionBinder.Bind<T>().To<T>();
    }

    private void BindViews()
    {
        InjectScreen<PreloaderScreen>("preloader");
        InjectScreen<SelectServerScreen>("selectServer");
        InjectScreen<AlertPopup>("alert");
        InjectScreen<LoaderPopup>("loader");
        //InjectScreen<CheckInternetPopup>("check_connect");
    }

    private strange.extensions.injector.api.IInjectionBinding InjectScreen<T>(string name) where T : BasePopup
    {
        return injectionBinder.Bind<T>().ToValue(CreateScreen<T>(name));
    }

    private T CreateScreen<T>(string name) where T : BasePopup
    {
        T s = null;

        if (_childs.ContainsKey(name))
            s = _childs[name] as T;
        else
        {
            var prefub = Resources.Load<GameObject>("screens/" + name);

            var obj = UnityUtils.Clone(prefub, _uiRoot);

            s = obj.GetComponent<T>();

            _childs.Add(name, s);
        }

        s.name = name;

        s.gameObject.SetActive(false);

        return s;
    }
}
