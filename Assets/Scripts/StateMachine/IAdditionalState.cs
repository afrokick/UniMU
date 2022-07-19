/// <summary>
/// Additional state.
/// StateMachine can load any numbers of IAdditionalState.
/// BUT! If StateMachine change IState, all IAdditionalState will be unloaded.
/// </summary>
public interface IAdditionalState : IState
{

}