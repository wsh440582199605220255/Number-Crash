using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ctrl_bar : MonoBehaviour
{
    public GameObject bar;
    public bool setLinearTarget = false;
    private UpdateBar barClass;

    private void Start() {
        if (bar == null) {
            Q.WarningPrint(transform, this.GetType().ToString());
            return;
        }

        barClass = bar.GetComponent<UpdateBar>();
        SetBar();
    }

    private void SetBar() {
        float value = transform.GetComponent<Slider>().value;
        if (setLinearTarget) {
            barClass.LinearTarget = value * barClass.MaxSlashValue;
        }
        else {
            barClass.PercentValue = value;
        }
    }
}
