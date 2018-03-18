using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    protected float restTime = 0f;
    protected float coldDownTime = 5f;

    public float ColdDownTime {
        get { return coldDownTime; }
        set {
            coldDownTime = value;
            if (restTime > coldDownTime) restTime = coldDownTime;
        }
    }

    public bool isCd = false;

    public event Action<GameObject> TimeOut;

    protected virtual void _AfterUpdate() { }

    // Update is called once per frame
    void Update() {
        if (!isCd) return;

        if (restTime > 0f) {
            restTime -= Time.deltaTime;
            isCd = true;
        }
        else {
            isCd = false;
            if (TimeOut != null) TimeOut(this.gameObject);
        }
        _AfterUpdate();
    }


    /// <summary>
    /// 重置冷却时间
    /// </summary>
    public void ResetCd() {
        isCd = true;
        restTime = coldDownTime;
    }

}
