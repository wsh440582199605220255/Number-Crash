using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPong : MonoBehaviour
{
    private Material material;
    private float factor;
    //private Color color;

    // Use this for initialization
    void Start() {
        material = GetComponent<SpriteRenderer>().material;
        //color.a = 1f;
    }

    // Update is called once per frame
    void Update() {
        //Debug.Log(factor);
        factor = Mathf.PingPong(Time.time, 2f);

        material.SetFloat("_Factor", factor);
        //material.SetColor("_Color", color);
    }
}
