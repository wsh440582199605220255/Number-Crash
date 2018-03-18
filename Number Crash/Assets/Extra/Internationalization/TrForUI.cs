using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Help translating static UI,for dynamic you'd better using the Lang.Tr("")
/// </summary>
[RequireComponent(typeof(Text))]
public class TrForUI : MonoBehaviour
{

    [SerializeField]private string english = "";
    [SerializeField]private string chinese = "";
    [SerializeField]private string other = "";

    private Text myText;

    private void Awake()
    {
        myText = GetComponent<Text>();

        UpdateLanguage();
    }

    public void UpdateLanguage()
    {
        switch (Lang.GlobalLanguage)
        {
            case Language.English: if (english != "") myText.text = english; break;
            case Language.Chinese: if (chinese != "") myText.text = chinese; break;
            case Language.Other: if (other != "") myText.text = other; break;
        }
    }

}
