using UnityEngine;
using UnityEngine.EventSystems;

public class _AndroidInput : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public static _AndroidInput PUBLIC;
    public static bool wantFly = false;
    public static bool wantReset = false;
    //public static bool wantSkill0 = false;
    //public static bool wantSkill1 = false;

    [SerializeField] private float radius = 140f;
    private Vector2 startPos;
    private Vector2 currVector;
    private CanvasGroup canvasGroup;
    private float startAlpha;


    private void Awake() {
        PUBLIC = this;
        startPos = transform.position;
        canvasGroup = GetComponentInParent<CanvasGroup>();
        startAlpha = canvasGroup.alpha;
    }

    public Vector2 InputAxis { get { return currVector; } }

    public Vector2 GradientAxis { get { return currVector / radius; } }

    public Vector2 NormalAxis { get { return currVector.normalized; } }

    public Vector2 AbsAxis {
        get {
            Vector2 result = Vector2.zero;
            if (currVector.x > 0) result.x = 1;
            if (currVector.x < 0) result.x = -1;
            if (currVector.y > 0) result.y = 1;
            if (currVector.y < 0) result.y = -1;
            return result;
        }
    }

    

    public void OnPointerUp(PointerEventData eventData) {
        canvasGroup.alpha = startAlpha;
        transform.position = startPos;
        currVector = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData) {
        currVector = Vector2.ClampMagnitude(eventData.position - startPos, radius);
        transform.position = currVector + startPos;
    }

    public void OnPointerDown(PointerEventData eventData) {
        canvasGroup.alpha = 1;
        currVector = Vector2.ClampMagnitude(eventData.position - startPos, radius);
        transform.position = currVector + startPos;
    }
}
