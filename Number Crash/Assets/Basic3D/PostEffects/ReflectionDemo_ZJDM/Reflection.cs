using UnityEngine;
using System.Collections;

public class Reflection : MonoBehaviour
{

    public Transform Panel;
    public Camera RefCamera;
    public Material RefMat;
    // Use this for initialization
    void Start() {
        if (null == RefCamera) {
            GameObject go = new GameObject {
                name = "refCamera"
            };
            RefCamera = go.AddComponent<Camera>();
            RefCamera.CopyFrom(Camera.main);
            RefCamera.enabled = false;
            RefCamera.cullingMask = ~(1 << LayerMask.NameToLayer("Water"));
        }
        if (null == RefMat) {
            RefMat = this.GetComponent<Renderer>().sharedMaterial;
        }
        RenderTexture refTexture = new RenderTexture(Mathf.FloorToInt(Camera.main.pixelWidth),
                                             Mathf.FloorToInt(Camera.main.pixelHeight), 24) {
            hideFlags = HideFlags.DontSave
        };
        RefCamera.targetTexture = refTexture;
    }

    public void OnWillRenderObject() {
        RenderRefection();
    }

    void RenderRefection() {
        Vector3 normal = Panel.up;
        float d = -Vector3.Dot(normal, Panel.position);
        Matrix4x4 refMatrix = new Matrix4x4 {
            m00 = 1 - 2 * normal.x * normal.x,
            m01 = -2 * normal.x * normal.y,
            m02 = -2 * normal.x * normal.z,
            m03 = -2 * d * normal.x,

            m10 = -2 * normal.x * normal.y,
            m11 = 1 - 2 * normal.y * normal.y,
            m12 = -2 * normal.y * normal.z,
            m13 = -2 * d * normal.y,

            m20 = -2 * normal.x * normal.z,
            m21 = -2 * normal.y * normal.z,
            m22 = 1 - 2 * normal.z * normal.z,
            m23 = -2 * d * normal.z,

            m30 = 0,
            m31 = 0,
            m32 = 0,
            m33 = 1
        };

        RefCamera.worldToCameraMatrix = Camera.main.worldToCameraMatrix * refMatrix;
        RefCamera.transform.position = refMatrix.MultiplyPoint(Camera.main.transform.position);

        Vector3 forward = Camera.main.transform.forward;
        Vector3 up = Camera.main.transform.up;
        forward = refMatrix.MultiplyPoint(forward);
        //up = refMatrix.MultiplyPoint (up);
        //Quaternion refQ = Quaternion.LookRotation (forward, up);
        //RefCamera.transform.rotation = refQ;
        RefCamera.transform.forward = forward;

        GL.invertCulling = true;
        RefCamera.Render();
        GL.invertCulling = false;

        RefCamera.targetTexture.wrapMode = TextureWrapMode.Repeat;
        RefMat.SetTexture("_RefTexture", RefCamera.targetTexture);
    }
}
