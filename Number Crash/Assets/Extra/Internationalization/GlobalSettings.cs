using UnityEngine;

[DefaultExecutionOrder(-2000)]
public class GlobalSettings : MonoBehaviour
{
    public enum Difficulty
    {
        easy, hard,
    }

    public Difficulty globalDifficulty = Difficulty.easy;
    public TextAsset trAsset;
    public Language globalLanguage = Language.English;

    public static GlobalSettings PUBLIC = null;
    private void Awake()
    {
        if (PUBLIC)
        {         
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this);
        PUBLIC = this;

        if (trAsset != null) Lang.LoadLanguageAsset(trAsset.text);
        //else Debug.Log("The language file is missing!");
        
        Lang.GlobalLanguage = globalLanguage;
    }
}
