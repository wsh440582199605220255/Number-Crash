using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 适用于任何Unity项目的静态方法，包括一个通用字典和若干常量
/// </summary>
public static class Q
{
    public const float INF = 0xFFFFFF;
    public const float UNITY2D_BETWEEN_COLLIDERS = 0.0149f;
    public static string[] PREVENT_LAYERS = { "Default", "Platform", "Defender" };

    /// <summary>
    /// 一个通用的字典，用于存储和获取GameObject
    /// </summary>
    public static Dictionary<string, GameObject> Objs = new Dictionary<string, GameObject>();

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawRay(Vector2 start, Vector2 dir, Color color) {
        Debug.DrawRay(start, dir, color);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawLine(Vector2 start, Vector2 end, Color color) {
        Debug.DrawLine(start, end, color);
    }

    public static void Swap<T>(ref T a, ref T b) {
        T temp = a;
        a = b;
        b = temp;
    }


    /// <summary>
    /// 立即销毁parent的所有子物体
    /// </summary>
    /// <param name="parent"></param>
    public static void DestroyAllChildren(GameObject parent) {
        while (parent.transform.childCount > 0) Object.DestroyImmediate(parent.transform.GetChild(0).gameObject);
    }

    /// <summary>
    /// 从Resources文件夹里加载资源到Objs[]
    /// </summary>
    /// <param name="path">路径，后面需要带'/'</param>
    /// <param name="name">名字，保存在Objs[]中</param>
    public static void LoadForObjs(string path, string name) {
        if (!Objs.ContainsKey(name)) {
            Objs[name] = (GameObject)Resources.Load(path + name);
        }
    }



    /// <summary>
    /// 输出警告信息，强制退出游戏
    /// </summary>
    /// <param name="from"></param>
    /// <param name="className"></param>
    /// <param name="text"></param>
    public static void WarningPrint(Transform from, string className, string text = "Execute disabled,some necessary properties are lost.") {

        Debug.Log("GameObject [" + GetFullPath(from) + "] =>" + className + ':' + text);

        from.gameObject.SetActive(false);
        //DestroyImmediate(from.gameObject);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #region Editor Functions
    /// <summary>
    /// 获取一个物体Transform组件的完整路径，displayRoot决定是否显示根路径
    /// </summary>
    /// <param name="t"></param>
    public static string GetFullPath(Transform t, bool displayRoot = true) {
        if (!displayRoot && t.parent == null) return "";
        string path = t.name;

        while (t.parent != null) {
            t = t.parent;
            if (!displayRoot && t.parent == null) break;
            path = path.Insert(0, t.name + '/');
        }
        return path;
    }



    /// <summary>
    /// 加载编辑器扩展相关的资源[Draw Axis]
    /// </summary>
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LoadForEditor() {
        LoadForObjs("editor/", "redPoint");
        LoadForObjs("editor/", "bluePoint");
        LoadForObjs("editor/", "whiteLine");
    }
    #endregion

}
