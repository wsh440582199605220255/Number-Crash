using DG.Tweening;
using UnityEngine;

/// <summary>
/// 主面板UI控制程序
/// </summary>
public class MainPanelUI : MonoBehaviour
{

    //private WaterWave_Single ws;
    private CanvasGroup colorMask;
    private GaussianBlur blur;
    private float transDuration = 2f;
    //private GameObject circleEffect;
    public static MainPanelUI PUBLIC;

    // Use this for initialization
    void Awake() {

        PUBLIC = this;
        //ws = GetComponent<WaterWave_Single>();
        blur = GetComponent<GaussianBlur>();
        colorMask = GameObject.Find("Canvas/colorMask").GetComponent<CanvasGroup>();
        //circleEffect = GameObject.Find("Canvas/circleEffect");
    }


    public void TransEffect() {
        //ws.CreateWave(Input.mousePosition);
        colorMask.DOFade(0.9f, transDuration).SetEase<Tween>(Ease.InQuad);
        Blur();
    }

    private void Blur() {
        blur.enabled = true;            //开启高斯模糊      
        DOTween.To(() => blur.blurSpread, x => blur.blurSpread = x, 8f, transDuration).SetEase<Tween>(Ease.InQuad);
        DOTween.To(() => blur.downSample, x => blur.downSample = x, 8f, transDuration).SetEase<Tween>(Ease.InQuad);

    }
}
