using UnityEngine;
using System;

public class MainConfig
{
    public readonly static SystemLanguage[] SupportedLanguages = { SystemLanguage.English, SystemLanguage.Russian };

    public static string GetRandomGreating()
    {
        var greatings = LocalizationService.Get("greatings").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        return greatings[UnityEngine.Random.Range(0, greatings.Length)];
    }
}