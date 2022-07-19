using System.Collections.Generic;

public interface IStateMachine
{
	IState LastState { get; }
	IState CurrentState { get; }

	/// <summary>
	/// Load new state. All states in stack will be unloaded.
	/// </summary>
	/// <param name="state">State.</param>
	void Load(IState state);
	void Load(IAdditionalState state, bool unloadPrev);
	void Unload(bool loadPrev);
}

public class StateMachine : IStateMachine
{
    private readonly Stack<IState> Stack = new Stack<IState>();

    private IState _currentState;

    public IState LastState
    {
        get
        {
            if (Stack.Count == 0)
                return null;

            return Stack.Peek();
        }
    }

	public IState CurrentState { get { return _currentState; } }

    public void Load(IState state)
    {
        while (Stack.Count > 0)
        {
            var item = Stack.Pop();
            item.Unload();
        }

        _currentState = state;

        Stack.Push(_currentState);
        _currentState.Load();
    }

    public void Load(IAdditionalState state, bool unloadPrev)
    {
        if (unloadPrev)
            LastState.Unload();

        Stack.Push(state);

        state.Load();
    }

    public void Unload(bool loadPrev)
    {
        var state = Stack.Pop();
        state.Unload();

        if (_currentState == state)
        {
            _currentState = null;
        }

        if (loadPrev && LastState != null)
            LastState.Load();
    }
}