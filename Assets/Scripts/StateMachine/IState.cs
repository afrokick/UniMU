/// <summary>
/// Main interface for all state.
/// Only one IState may stay in StateMachine's stack
/// </summary>
public interface IState
{
	void Load();
	void Unload();
}