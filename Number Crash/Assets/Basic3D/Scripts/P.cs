using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

/// <summary>
/// 全局单例类，包括静态全局变量和静态函数。
/// </summary>
[DefaultExecutionOrder(-200)]
public class P : MonoBehaviour
{
    public enum FramePerSecond
    {
        fantastic = 60,
        good = 45,
        based = 30,
    }


    public static P PUBLIC;

    public bool debugMode = true;
    public Text debugText;
    public Text debugText2;

    [SerializeField] private bool openStartEffect = true;
    public float startEffectDuration = 2.5f;
    private CanvasGroup transMaskCg;
    private WaterWave wwr;
    public GameObject colorMask;
    private string currentScene;
    public FramePerSecond fps = FramePerSecond.good;

    // Use this for initialization
    void Awake() {
        PUBLIC = this;

        Application.targetFrameRate = (int)fps;
        currentScene = SceneManager.GetActiveScene().name;

        if (debugMode) {
            debugText = GameObject.Find("Canvas/DebugText").GetComponent<Text>();
            StartCoroutine(DisPlayFPS());
        }

        colorMask = GameObject.Find("Canvas/colorMask");
        transMaskCg = colorMask.GetComponent<CanvasGroup>();
        wwr = Camera.main.GetComponent<WaterWave>();

        if (openStartEffect) {

            transMaskCg.alpha = 0.9f;
            transMaskCg.DOFade(0, startEffectDuration).SetEase<Tween>(Ease.OutQuad);

            if (wwr) StartCoroutine(Wr());       //开启水波特效
        }
    }

    IEnumerator Wr() {
        wwr.enabled = true;
        yield return new WaitForSecondsRealtime(startEffectDuration);
        wwr.enabled = false;
    }



    public GameObject MakeEffect(GameObject effect, Vector3 pos, float destroyDelay = 1f) {
        GameObject newEffect = Instantiate(effect);
        newEffect.transform.position = pos;
        Destroy(newEffect, destroyDelay);
        return newEffect;
    }


    IEnumerator DisPlayFPS() {
        while (true) {
            float dt = 0;
            for (int i = 0; i < 10; i++) {
                dt += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            debugText.text = "FPS:" + (10.0f / dt).ToString();// + '\n' + Input.acceleration.ToString();
        }
    }


    private IEnumerator _LoadSceneWithEffect(string name, float dur) {
        AsyncOperation async = SceneManager.LoadSceneAsync(name);
        async.allowSceneActivation = false;

        yield return new WaitForSecondsRealtime(dur);
        async.allowSceneActivation = true;

        //yield return new WaitForSecondsRealtime(dur);
        //SceneManager.LoadSceneAsync(name);
    }



    /// <summary>
    /// 异步加载一个新场景，并使用特效
    /// </summary>
    /// <param name="name"></param>
    public void LoadSceneWithEffect(string name, float dur = 1.5f) {
        StartCoroutine(_LoadSceneWithEffect(name, dur));
    }

    /// <summary>
    /// 异步重载当前场景
    /// </summary>
    public void ReLoadCurrentScene(float dur = 1.5f) {
        Invoke("_ReLoadCurrentScene", dur);
        colorMask.GetComponent<Image>().color = Color.black;
        transMaskCg.DOFade(1, dur);
    }

    private void _ReLoadCurrentScene() {
        SceneManager.LoadSceneAsync(currentScene);
    }

}
