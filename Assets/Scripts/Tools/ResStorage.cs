using System.Collections.Generic;
using UnityEngine;


static class ResStorage
{
	private static readonly Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();

    public static Sprite GetSprite(string path)
    {
        if (Sprites.ContainsKey(path)) return Sprites[path];
        var s = Resources.Load<Sprite>(path);
        if (s == null) Debug.LogError("Invalid path for icon: [" + path + "]");
        Sprites.Add(path, s);
        return s;
    }

}

