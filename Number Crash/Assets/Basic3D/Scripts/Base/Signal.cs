using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Signal : MonoBehaviour {

    public string signalName;
    public abstract void OnTrigger();
}
