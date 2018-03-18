using MainLogic.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapEdit_Panel : MonoBehaviour, IPointerClickHandler
{
    public NType nodeType = NType.basic;
    private Sprite sprite;

    public void OnPointerClick(PointerEventData eventData)
    {
        ChessNode node = MapEdit_UI.PUBLIC.CurrentNode;
        if (node == null) return;
        node.GetComponent<SpriteRenderer>().sprite = sprite;
        node.type = nodeType;
    }

    private void Awake()
    {
        sprite = GetComponent<UnityEngine.UI.Image>().sprite;
    }


}
