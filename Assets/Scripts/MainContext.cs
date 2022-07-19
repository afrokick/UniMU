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

        injectionBinder.Bind<LocalizationService>().ToSingleton();

        //injectionBinder.Bind<GameManager>().ToSingleton();
        injectionBinder.Bind<IStorageService>().To<StorageService>().ToSingleton();
        injectionBinder.Bind<ICoroutineExecuter>().To<CoroutineExecuter>().ToSingleton();
        injectionBinder.Bind<IStateMachine>().To<StateMachine>().ToSingleton();

        BindHandlers();
        BindStates();
        BindViews();

        //signals and command
        commandBinder.Bind<AppQuitSignal>();
        commandBinder.Bind<AppStartSignal>()
                     .InSequence()
                     .To<AppStartCommand>()
                     .To<LoadStateCommand<PreloaderState>>()
                     .To<LoadStateCommand<SelectServerState>>()
                     .Once();

        commandBinder.Bind<HardwareBackPressSignal>();

        commandBinder.Bind<OpenAlertSignal>().To<OpenAlertCommand>();

        commandBinder.Bind<OpenSelectServerScreenSignal>().To<LoadStateCommand<SelectServerState>>();
        commandBinder.Bind<OpenLoginScreenSignal>().To<LoadStateCommand<LoginState>>();
        commandBinder.Bind<OpenCharactersScreenSignal>().To<LoadStateCommand<CharactersState>>();
        commandBinder.Bind<OpenCreateCharacterPopupSignal>().To<LoadAdditionalStateCommand<CreateCharacterState>>();
        commandBinder.Bind<OpenLoadingCharacterScreenSignal>().To<LoadStateCommand<LoadingCharacterState>>();

        commandBinder.Bind<ServerListUpdatedSignal>();
        commandBinder.Bind<ServerListItemUpdatedSignal>();
        commandBinder.Bind<CharactersListUpdatedSignal>();
        commandBinder.Bind<CharacterFocusedSignal>();
        commandBinder.Bind<CharacterCreatedSignal>();
        commandBinder.Bind<CharacterDeletedSignal>();
        commandBinder.Bind<CharacterSelectedSignal>();
        commandBinder.Bind<NewPlayersInScopeSignal>();

        commandBinder.Bind<OpenWorldSignal>().To<LoadStateCommand<WorldState>>();

        commandBinder.Bind<LoggedInSignal>().InSequence().To<LoadStateCommand<CharactersState>>();
    }

    private void BindHandlers()
    {
        //NETWORK
        injectionBinder.Bind<PacketHandler>().ToSingleton();

        var packetHandlerInterfaceType = typeof(IPacketHandler);
        var handlers = this.GetType().Assembly.GetTypes().Where(type => type.IsClass && type.GetInterfaces().Contains(packetHandlerInterfaceType));

        foreach (var handler in handlers)
        {
            injectionBinder.Bind(handler).ToSingleton();
        }
    }

    private void BindStates()
    {
        var baseStateType = typeof(BaseState);
        var baseAdditionalStateType = typeof(BaseAdditionalState);

        var states = this.GetType().Assembly.GetTypes().Where(type => type.IsSubclassOf(baseStateType) || type.IsSubclassOf(baseAdditionalStateType));

        foreach (var stateType in states)
        {
            injectionBinder.Bind(stateType).To(stateType);
        }
    }

    private void BindViews()
    {
        foreach (var child in _childs.Values)
        {
            child.gameObject.SetActive(false);

            var t = child.GetType();

            injectionBinder.Bind(t).ToValue(child);
        }
    }
}
