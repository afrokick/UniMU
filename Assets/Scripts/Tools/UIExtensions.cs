/*
 * Created by Alexander Sosnovskiy. May 3, 2016
 */
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class UIExtensions
{
	public static void SetOnClick (this Button go, Action<GameObject> handler)
	{		
		go.onClick.AddListener (() => { 
			if (handler != null)
				handler (go.gameObject);
		});
	}

	public static void SetOnClick (this GameObject go, Action<GameObject> handler)
	{
		var eventHandler = go.GetComponent<Button> ();

		if (eventHandler == null)
			eventHandler = go.AddComponent<Button> ();

		eventHandler.onClick.AddListener (() => {
			if (handler != null)
				handler (go);
		});
	}
}