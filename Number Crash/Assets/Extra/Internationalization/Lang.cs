using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public enum Language
{
    English = 0,
    Chinese,
    Other,
    OriginText,
}

public static class Lang
{
    private class Sentence
    {
        public string originText = null;
        public string[] trs = null;
    }

    public static void LoadLanguageAsset(string trText)
    {
        if (dics.Count > 0) return;
        string[] ss = trText.Split('\n');

        Sentence s;
        for (int i = 0; i < ss.Length; i++)
        {
            s = JsonConvert.DeserializeObject<Sentence>(ss[i]);
            if (s != null) dics.Add(s.originText, s.trs);
        }
    }

    private static Dictionary<string, string[]> dics = new Dictionary<string, string[]>();
    private static Language lang = Language.English;

    public static Language GlobalLanguage {
        get { return lang; }
        set {
            lang = value;
            TrForUI[] tfus = UnityEngine.Object.FindObjectsOfType<TrForUI>();
            for (int i = 0; i < tfus.Length; i++) tfus[i].UpdateLanguage();
        }
    }

    public static string Tr(string originText)
    {
#if UNITY_EDITOR
        if (dics.Count == 0)
        {
            Debug.Log("The language file isn't loaded.");
            return originText;
        }
#endif
        if (dics.ContainsKey(originText))
        {
            string result = dics[originText][(int)lang];
            if (result != "") return result;
        }

        return originText;
    }
}

/// <summary>
/// Tag a string which can be translate
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class Internationable : Attribute
{
    public Internationable()
    {

    }
}