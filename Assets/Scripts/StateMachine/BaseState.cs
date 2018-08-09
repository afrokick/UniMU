public abstract class BaseState : IState
{
    [Inject]
    public IStateMachine StateMachine { get; set; }

    [Inject]
    public HardwareBackPressSignal HardwareBackPressSignal { get; set; }

    public virtual void Load()
    {
        HardwareBackPressSignal.AddListener(OnHardwareBackPress);
    }

    public virtual void Unload()
    {
        HardwareBackPressSignal.RemoveListener(OnHardwareBackPress);
    }

    protected virtual void OnHardwareBackPress()
    {
        if (StateMachine.LastState != null)
            return;

        StateMachine.Unload(false);
    }
}