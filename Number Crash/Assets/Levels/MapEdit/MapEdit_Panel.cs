using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapEdit_Panel : MonoBehaviour, IPointerClickHandler
{
    public NType nodeType = NType.basic;
    public static MapEdit_Panel CurrPanel { get; private set; }
    public static GameObject pointer2;
    public Sprite mySprite;

    public void OnPointerClick(PointerEventData eventData)
    {
        CurrPanel = this;
        pointer2.transform.position = this.transform.position;
    }

    private void Awake()
    {
        if (nodeType == NType.basic)
        {
            pointer2 = new GameObject("pointer2");
            pointer2.transform.localScale *= 0.32f;
            pointer2.transform.parent = this.transform.parent;
            pointer2.AddComponent<Image>().color = new Color(1, 0.5f, 0.5f);
            CurrPanel = this;
            pointer2.transform.position = this.transform.position;
        }
        mySprite = GetComponent<Image>().sprite;
    }


}
