using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using log4net;

public class LocalizationLanguage
{
    public SystemLanguage Language { get; set; }
    public string Id { get; set; }
    public string GeneralDictId { get; set; }
    public readonly Dictionary<string, string> Keys = new Dictionary<string, string>();
}

public class LocalizationService
{
    private static readonly ILog log = LogManager.GetLogger(typeof(LocalizationService));

    [Inject]
    public ICoroutineExecuter CoroutineExecuter { get; private set; }

    private readonly IStorageService StorageService;

    private const string LANG_KEY = "currentLang";
    private const string URL = "http://146.185.134.182:23435/api/getLocalizations";
    private const string LOCAL_LANG_FILE = "localizations.dat";

    private static readonly SystemLanguage DefaultLanguage = SystemLanguage.Russian;

    private static Dictionary<SystemLanguage, LocalizationLanguage> _languages = new Dictionary<SystemLanguage, LocalizationLanguage>();

    public static SystemLanguage CurrentLang { get; private set; }
    public SystemLanguage[] Languages { get; private set; }
    public string LangId { get { return _languages[CurrentLang].Id; } }

    public LocalizationService(IStorageService ss)
    {
        StorageService = ss;

        var lang = ss.Get(LANG_KEY);

        if (string.IsNullOrEmpty(lang))
        {
            CurrentLang = DefaultLanguage;
        }
        else
        {
            CurrentLang = (SystemLanguage)Enum.Parse(typeof(SystemLanguage), lang);
        }

        UpdateLocal(null);
    }

    private List<LocalizationLanguage> ParseData(SimpleJSON.JSONNode json)
    {
        var list = new List<LocalizationLanguage>();

        var jsonLangs = json["localizations"].AsArray;

        foreach (SimpleJSON.JSONClass jsonLang in jsonLangs)
        {
            var lang = new LocalizationLanguage();

            var code = jsonLang["code"] + "";

            lang.Language = (SystemLanguage)Enum.Parse(typeof(SystemLanguage), code);

            lang.Id = jsonLang["_id"] + "";
            lang.GeneralDictId = jsonLang["generalDict"] + "";

            var jsonLocalization = jsonLang["localization"].AsArray;

            foreach (SimpleJSON.JSONClass jsonPair in jsonLocalization)
            {
                lang.Keys.Add(jsonPair["key"], jsonPair["val"]);//.ToString().ToUpper());
            }

            list.Add(lang);
        }

        return list;
    }

    public void SetLanguage(SystemLanguage lang)
    {
        if (!_languages.ContainsKey(lang))
            lang = DefaultLanguage;

        CurrentLang = lang;

        StorageService.Set(LANG_KEY, lang.ToString());
        StorageService.Save();
    }

    public string GetLangId(SystemLanguage lang)
    {
        return _languages[lang].Id;
    }

    public static string Get(string key, params object[] pars)
    {
        var lang = _languages[CurrentLang];

        string val = key;

        if (!lang.Keys.ContainsKey(key))
        {
            log.Warn($"No key '{key}' for lang '{CurrentLang}'!");
        }
        else
        {
            val = lang.Keys[key];
        }

        try
        {
            if (pars != null && pars.Length > 0)
                return string.Format(val, pars);
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }

        return val;
    }

    public bool IsInited()
    {
        return _languages.Count > 0;
    }

    public IEnumerable<SystemLanguage> GetNextLang()
    {
        foreach (var language in _languages)
        {
            yield return language.Value.Language;
        }
    }

    public static string Get(SystemLanguage lng, string key)
    {
        var lang = _languages[lng];

        string val = key;

        if (!lang.Keys.ContainsKey(key))
        {
            log.Warn("No key '" + key + "' for lang '" + lng + "'!");
        }
        else
        {
            val = lang.Keys[key];
        }

        return val;
    }

    public void UpdateFromServer(Action<bool> callback)
    {
        CoroutineExecuter.Execute(LoadJSON(URL, (json, raw) =>
        {
            if (json == null)
            {
                UpdateLocal(callback);

                return;
            }

            var status = json["status"].AsInt;

            if (status != 200)
            {
                UpdateLocal(callback);
                return;
            }

            try
            {
                FillLanguages(json);

                DiskStorage.WriteText(LOCAL_LANG_FILE, raw);

                if (callback != null)
                    callback(false);
            }
            catch
            {
                if (callback != null)
                    callback(true);
            }
        }));
    }

    private void UpdateLocal(Action<bool> callback)
    {
        try
        {
            var text = DiskStorage.ReadText(LOCAL_LANG_FILE);

            if (string.IsNullOrEmpty(text))
            {
                text = Resources.Load<TextAsset>("Localization").text;
            }

            var json = SimpleJSON.JSON.Parse(text);

            FillLanguages(json);

            if (callback != null)
                callback(false);
        }
        catch
        {
            if (callback != null)
                callback(true);
        }
    }

    private void FillLanguages(SimpleJSON.JSONNode json)
    {
        _languages.Clear();

        var langs = ParseData(json);

        Languages = new SystemLanguage[langs.Count];

        foreach (var lang in langs)
        {
            _languages.Add(lang.Language, lang);
        }

        _languages.Keys.CopyTo(Languages, 0);
    }

    private IEnumerator LoadJSON(string url, Action<SimpleJSON.JSONNode, string> onFinished)
    {
        WWW www = new WWW(url);

        float elapsedTime = 0.0f;

        while (!www.isDone)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 2)
            {
                if (onFinished != null)
                    onFinished(null, string.Empty);
                yield break;
            }

            yield return null;
        }

        if (!www.isDone || !string.IsNullOrEmpty(www.error) || string.IsNullOrEmpty(www.text))
        {
            if (onFinished != null)
                onFinished(null, string.Empty);
            yield break;
        }

        string response = www.text;

        if (onFinished != null)
            onFinished(SimpleJSON.JSON.Parse(response), response);
    }
}