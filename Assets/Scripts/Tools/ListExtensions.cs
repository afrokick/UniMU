/*
 * Created by Alexander Sosnovskiy. May 3, 2016
 */
using System;
using System.Collections.Generic;

public static class ListExtensions
{
	public static List<T> Clone<T>(this List<T> list)
	{
		return new List<T>(list);
	}

	public static void Reshuffle<T>(this List<T> list)
	{
		for (int t = 0; t < list.Count; t++ )
		{
			var tmp = list[t];

			int r = UnityEngine.Random.Range(t, list.Count);

			list[t] = list[r];

			list[r] = tmp;
		}
	}

	public static T[] ForEach<T>(this T[] list, Action<T> callback)
	{
		if(callback != null)
			for(int i=0;i< list.Length;i++){
				callback(list[i]);
			}

		return list;
	}
}