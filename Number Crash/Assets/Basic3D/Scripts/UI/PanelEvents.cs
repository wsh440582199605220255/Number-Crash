using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelEvents : MonoBehaviour {

    public GameObject Panel;

    private void Awake() {
        if (Panel == null) {
            Q.WarningPrint(transform, this.GetType().ToString());
            return;
        }
    }

    public void closePanel() {
        Panel.SetActive(false);
    }
    
    public void openPanel() {
        Panel.SetActive(true);
    }
}
