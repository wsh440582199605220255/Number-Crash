using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StoryMask : MonoBehaviour
{
    const float SCALE_1920_1080 = 0.3739583f;


    public float changeTime = 1f;

    public float verticalDistance = -1;

    private Transform up, down;
    private Vector2 upEndPos, upStartPos;
    private Vector2 downEndPos, downStartPos;
    private CanvasGroup UI_cg;
    private bool isMask = false;

    void Start() {
        up = transform.Find("up").GetComponent<RectTransform>();
        down = transform.Find("down").GetComponent<RectTransform>();
        UI_cg = transform.parent.Find("UI").GetComponent<CanvasGroup>();

        if (up == null || down == null || UI_cg == null || verticalDistance < 0f) Q.WarningPrint(transform, this.GetType().ToString());

        upStartPos = up.position;
        downStartPos = down.position;

        upEndPos = upStartPos - new Vector2(0, verticalDistance * SCALE_1920_1080);
        downEndPos = downStartPos + new Vector2(0, verticalDistance * SCALE_1920_1080);

    }



    public void MaskChange() {
        if (!isMask) {
            up.DOMove(upEndPos, changeTime);
            down.DOMove(downEndPos, changeTime);
            UI_cg.alpha = 0f;
            isMask = true;
        }
        else {
            up.DOMove(upStartPos, changeTime);
            down.DOMove(downStartPos, changeTime);
            UI_cg.DOFade(1f, changeTime);
            isMask = false;
        }
    }
}
