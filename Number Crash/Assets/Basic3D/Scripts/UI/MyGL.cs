using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGL : MonoBehaviour
{

    public Material material;
    public Transform largeCircle;
    public Transform smallCircle;

    private Vector2 vL, vR;

    private void Awake() {
        vL = (Vector2)largeCircle.position - new Vector2(180f, 0);
        vR = (Vector2)largeCircle.position + new Vector2(180f, 0);
    }

    private void OnPostRender() {
        GL.PushMatrix();
        material.SetPass(0);
        GL.LoadPixelMatrix();
        GL.Begin(GL.LINES);
        GL.Color(Color.black);

        GL.Vertex3(vL.x, vL.y, 0);
        GL.Vertex3(vR.x, vR.y, 0);

        GL.Vertex3(vL.x, vL.y, 0);
        GL.Vertex3(smallCircle.position.x, smallCircle.position.y, 0);

        GL.Vertex3(smallCircle.position.x, smallCircle.position.y, 0);
        GL.Vertex3(vR.x, vR.y, 0);

        GL.End();
        GL.PopMatrix();
    }
}
