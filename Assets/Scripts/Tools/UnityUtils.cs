using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public static class UnityUtils
{
	static public T GetChild<T>(GameObject obj, string name) where T : Component
    {
        var ar = obj.GetComponentsInChildren<T>();
        return ar.FirstOrDefault(item => item.name == name);
    }

    static public T GetChildLike<T>(GameObject obj, string name) where T : Component
    {
        var ar = obj.GetComponentsInChildren<T>();
        return ar.FirstOrDefault(item => item.name.StartsWith(name));
    }

    static public T RootChildLike<T>(string name) where T : Component
    {
        var objs = Object.FindObjectsOfType(typeof(T));
        foreach (var o in objs)
        {
            if (o.name.StartsWith(name) && o is T)
            {
                return o as T;
            }
        }
        return null;
    }

    public static GameObject GetRootGameObject(string name)
    {
        var objs = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var o in objs)
        {
            if (o.name == name)
            {
                return o;
            }
        }
        return null;
    }

    public static T GetRootObject<T>(string name) where T : Component
    {
        var objs = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var o in objs)
        {
            if (o.name.StartsWith(name))
            {
                var cmp = o.GetComponent<T>();
                if (cmp != null)
                {
                    return cmp;
                }
            }
        }
        return null;
    }

    static public T GetRootObject<T>() where T : Component
    {
        var objs = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var o in objs)
        {
            var cmp = o.GetComponent<T>();
            if (cmp != null)
            {
                return cmp;
            }
        }
        return null;
    }


    static public T GetChild<T>(Transform parent, string name) where T : Component
    {
        var ar = parent.gameObject.GetComponentsInChildren<T>();
        return ar.FirstOrDefault(item => item.name == name);
    }

    static public T Init<T>(GameObject obj) where T : Component
    {
        var c = obj.GetComponent<T>();
        return c;
    }

    public static GameObject Clone(GameObject go)
    {
        var clone = Object.Instantiate(go);
        if (clone != null)
        {
            clone.transform.SetParent(go.transform.parent, false);
            clone.transform.localScale = go.transform.localScale;
            clone.transform.position = go.transform.position;
        }
        return clone;
    }

    public static T Clone<T>(GameObject go, Transform parent = null) where T : Component
    {
        if (parent == null)
            parent = go.transform.parent;
        var clone = Object.Instantiate(go);
        if (clone != null)
        {
            clone.transform.localScale = go.transform.localScale;
            clone.transform.SetParent(parent, false);
            clone.transform.localPosition = go.transform.localPosition;
        }
        return (clone != null) ? clone.GetComponent<T>() : null;
    }

    public static GameObject Clone(GameObject go, Transform parent)
    {
        var clone = Object.Instantiate(go);
        if (clone != null)
        {
            clone.transform.SetParent(parent, false);
        }
        return clone;
    }

    public static int Parse(string s)
    {
        int res = 0;
        int k = 1;
        for (int i = s.Length - 1; i >= 0; --i)
        {
            char ch = s[i];
            if (ch >= '0' && ch <= '9')
            {
                res += (s[i] - '0') * k;
            }
            else if (ch == '-')
            {
                res = -res;
                break;
            }
            else
                break;
            k *= 10;
        }
        return res;
    }

    public static string Signed(int val)
    {
        if (val > 0) return "+" + val;
        return val.ToString();
    }

    public static bool IsEqual(float v0, float v1, float epsilon = 0.001f)
    {
        return Mathf.Abs(v0 - v1) < epsilon;
    }

    public static Color ColorFromInt(int color)
    {
        float r = (color >> 16) % 256 / 255.0f;
        float g = (color >> 8) % 256 / 255.0f;
        float b = color % 256 / 255.0f; ;
        return new Color(r, g, b);
    }

    public static Vector2 ResizeScrollerContext(GameObject lastItem, bool horisontal)
    {
        if (lastItem == null)
            return Vector2.zero;
        var rect = lastItem.GetComponent<RectTransform>();
        var content = lastItem.transform.parent.gameObject.GetComponent<RectTransform>();
        var sz = content.sizeDelta;
        if (horisontal)
        {
            sz.x = rect.anchoredPosition.x + rect.sizeDelta.x * (1.0f - rect.pivot.x);
        }
        else
        {
            sz.y = Math.Abs(rect.anchoredPosition.y) + rect.sizeDelta.y * rect.pivot.y;
        }
        content.sizeDelta = sz;
        return content.sizeDelta;
    }

    private static int _syncRnd = UnityEngine.Random.Range(0, int.MaxValue);

    private static int GetRandom()
    {
        _syncRnd = (1103515245 * _syncRnd + 12345);
        return Math.Abs(_syncRnd);
    }

    public static void SetRandomize(int r) { _syncRnd = r; }

    public static int SyncRand(int min, int max = 0)
    {
        if (max == 0)
        {
            return GetRandom() % min;
        }

        return GetRandom() % (max - min) + min;
    }

    public static float SyncRand(float min, float max)
    {
        if (IsEqual(min, max))
            return min;
        int dist = (int)(10000.0f * (max - min));
        return (GetRandom() % dist) / 10000.0f + min;
    }

    static public uint TimeStamp
    {
        get
        {
            var ServerDelta = 0;
            var unixTimestamp = ServerDelta + (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return (uint)unixTimestamp;
        }
    }

    static public string GetDottedTimeSpan(int uts)
    {
        if (uts < TimeStamp)
            uts = (int)TimeStamp;
        var value = new TimeSpan(0, 0, (int)(uts - TimeStamp));
        string duration = (value.Hours + value.Days * 24).ToString("00") + ":" + value.Minutes.ToString("00") + ":" + value.Seconds.ToString("00");
        return duration;
    }

    static public string GetDottedTime(int uts, bool needHours = true)
    {
        var value = new TimeSpan(0, 0, uts);
        string duration = "";
        if (value.Hours > 0 || needHours)
            duration += (value.Hours + value.Days * 24).ToString("00") + ":";
        duration += value.Minutes.ToString("00") + ":" + value.Seconds.ToString("00");
        return duration;
    }

    static public string GetShortInt(int val)
    {
        if (val >= 1000000)
        {
            return val / 1000000 + "M";
        }
        if (val >= 1000)
        {
            return val / 1000 + "k";
        }

        return val.ToString();
    }

    static public void ShareScreen(string msg)
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
            screenTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
            screenTexture.Apply();
            byte[] dataToSave = screenTexture.EncodeToJPG();

            string destination = Path.Combine(Application.persistentDataPath, "tpc-" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".jpg");

            File.WriteAllBytes(destination, dataToSave);

            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), msg);
            //intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "SUBJECT");
            intentObject.Call<AndroidJavaObject>("setType", "image/jpg");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            // option one WITHOUT chooser:
            //currentActivity.Call("startActivity", intentObject);

            // option two WITH chooser:
            AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share image using:");
            currentActivity.Call("startActivity", jChooser);
        }
#endif
    }


    public static T Load<T>(string path) where T : Component
    {
        var prefub = Resources.Load<GameObject>(path);
        return Clone<T>(prefub);
    }

    public static void ResizePool<T>(IList<T> pool, T sample, int count, UnityAction<T> onCreate, Transform parent) where T : MonoBehaviour
    {
        for (int i = 0; i < count; ++i)
        {
            if (i >= pool.Count)
            {
                var cloned = sample.Clone<T>();
                cloned.transform.SafeSetParent(parent);

                pool.Add(cloned);

                if (onCreate != null)
                {
                    onCreate(pool[i]);
                }
            }

            pool[i].gameObject.SetActive(true);
        }

        for (int i = count; i < pool.Count; ++i)
        {
            pool[i].gameObject.SetActive(false);
        }
    }

    public static T GetChild<T>(this MonoBehaviour gameObject, string name) where T : Component
    {
        var ar = gameObject.GetComponentsInChildren<T>();

        return ar.FirstOrDefault(x => x.name == name);
    }

    //public static GameObject Clone(this MonoBehaviour gameObject)
    //{
    //    var oldVis = Visible;
    //    if (!Visible)
    //        Visible = true;
    //    var go = Utils.Clone(gameObject);
    //    Visible = oldVis;
    //    return go;
    //}

    public static T Clone<T>(this MonoBehaviour script) where T : Component
    {
        if (script.GetComponent<T>() != null)
        {
            var go = Clone(script.gameObject);
            return go.GetComponent<T>();
        }
        return null;
    }
}

