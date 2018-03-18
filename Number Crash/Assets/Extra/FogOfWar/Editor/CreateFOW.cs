using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateFOW : MonoBehaviour
{

    [MenuItem("FogOfWar/CreateFOWSystem")]
    private static void NewFOWSystem() {
        GameObject root = new GameObject("FOWRenderRoot");
        root.AddComponent<FOWSystem>();
        root.transform.position = Vector3.zero;
    }


    [MenuItem("FogOfWar/CreateRender")]
    private static void CreateRender() {
        Transform parent = GameObject.Find("FOWRenderRoot").transform;
        if (parent == null) return;
        // TODO：实际项目中，从这里的资源管理类加载预设
        // 为了简单，这里直接从Resource加载
        Object prefabs = Resources.Load("Prefabs/projector");
        if (prefabs != null) {
            GameObject projector = Instantiate(prefabs) as GameObject;
            if (projector != null) {
                projector.transform.parent = parent;
                projector.transform.position = parent.position + (Vector3.up * 64);
                FOWRender render = projector.gameObject.AddComponent<FOWRender>();
                render.GetComponent<Projector>().orthographicSize = FOWSystem.Instance.worldSize * 0.5f;
                render.gameObject.SetActive(false);
            }
        }
    }
}
