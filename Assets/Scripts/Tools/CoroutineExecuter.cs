/*
 * Created by Alexander Sosnovskiy. May 3, 2016
 */
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Executer contains StartCoroutine method. GameObject doesn't.
/// </summary>
public class ExecuterGo : MonoBehaviour { }

public interface ICoroutineExecuter
{
	/// <summary>
	/// Execute any action from any thread
	/// </summary>
	/// <param name="action">Action.</param>
	void ExecuteOnUpdate(Action action);
	/// <summary>
	/// Execute any action from any thread
	/// </summary>
	/// <param name="action">Action.</param>
	/// <param name="delay">Delay in seconds.</param>
	void ExecuteOnUpdate(Action action, float delay);

	/// <summary>
	/// Execute coroutine from UI thread
	/// </summary>
	/// <param name="coroutine">Coroutine.</param>
	Coroutine Execute(IEnumerator coroutine);

	void RemoveCoroutine(Coroutine coroutine);
}

public class CoroutineExecuter : ICoroutineExecuter
{
	private static ExecuterGo _instance;

	/// <summary>
	/// Create singleton for Executer.
	/// </summary>
	/// <value>The instance.</value>
	private ExecuterGo Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new GameObject("_CoroutineExecuter").AddComponent<ExecuterGo>();
				GameObject.DontDestroyOnLoad(_instance.gameObject);
			}

			return _instance;
		}
	}

	private readonly List<Action> _tasks = new List<Action>();

	public CoroutineExecuter()
	{
		Instance.StartCoroutine(Update()); //It must be run on UI thread.
	}

	private IEnumerator Update()
	{
		while (true)
		{
			if (_tasks.Count > 0)
			{
				lock (_tasks)
					while (_tasks.Count > 0)
					{
						var temp = _tasks[0];
						_tasks.RemoveAt(0);

						if (temp != null)
						{
							temp();
						}
					}
			}

			yield return null;
		}
	}

	public void ExecuteOnUpdate(Action action)
	{
		lock (_tasks)
		{
			_tasks.Add(action);
		}
	}

	public void ExecuteOnUpdate(Action action, float wait)
	{
		lock (_tasks)
		{
			_tasks.Add(()=>{
				Execute(WaitAndExecute(action, wait));
			});
		}
	}

	public Coroutine Execute(IEnumerator coroutine)
	{
		return Instance.StartCoroutine(coroutine);
	}

	public void RemoveCoroutine(Coroutine coroutine)
	{
		if (coroutine != null)
			Instance.StopCoroutine(coroutine);
	}

	private IEnumerator WaitAndExecute(Action action, float wait)
	{
		yield return new WaitForSeconds(wait);

		if(action != null)
			action();
	}
}