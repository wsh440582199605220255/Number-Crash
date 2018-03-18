using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Newtonsoft.Json;

public struct Vector3Value
{
    public float x, y, z;
    public Vector3Value(UnityEngine.Vector3 v) {
        this.x = v.x;
        this.y = v.y;
        this.z = v.z;
    }
    public Vector3 ToUnityVector3() {
        return new Vector3(x, y, z);
    }
}

public class TransformValue
{
    public string name;
    public Vector3Value localPosition, roration, scale;

    public TransformValue(Transform t) {
        if (t == null) return;
        this.name = t.name;
        this.localPosition = new Vector3Value(t.localPosition);
        this.roration = new Vector3Value(t.eulerAngles);
        this.scale = new Vector3Value(t.localScale);
    }
}


public class ScenesToFile : ScriptableWizard
{
    public Transform root = null;
    public string savePath = "Assets/Animations";

    public string commandLine = "";

    const string cf = "Create file", df = "Delete file";
    const string cAnim = "Create Animation", check = "Check file", lf = "Load file";

    [MenuItem("Tools/Scenes To File")]
    static void CreateWizerd() {
        DisplayWizard("Scenes To File", typeof(ScenesToFile), "Close", "RunCommand");
    }

    private void OnEnable() {
        helpString = "Commands:\n" + cf + '\n' + df + '\n' + lf + '\n' + check + '\n' + cAnim;
    }

    private void OnWizardCreate() {
    }

    private void OnWizardOtherButton() {
        //执行命令行之前检查
        if (savePath == "" || root == null) {
            errorString = "Please input the SavePath and RootScene first.";
            return;
        }
        switch (commandLine) {
            case cf:
                CreatFile(); break;
            case df:
                DeleteFile(); break;
            case check:
                CheckFile(); break;
            case cAnim: {
                    if (CheckFile()) {
                        CreateAnimation();
                    }
                    break;
                }
            case lf:
                LoadFile(); break;
            default: errorString = "Invaild command!"; break;
        }
    }

    private bool CheckFile() {
        if (File.Exists(Path.Combine(savePath, root.name + ".txt"))) {
            errorString = "The file is ready!";
            return true;
        }
        else {
            errorString = "The file doesn't exist.";
            return false;
        }
    }

    List<TransformValue> list = new List<TransformValue>();

    private void CreatFile() {
        list.Clear();
        DFS_Create(root);

        string result = JsonConvert.SerializeObject(list);

        File.WriteAllText(Path.Combine(savePath, root.name + ".txt"), result);
        errorString = "";
    }

    //root的子节点将按照深度优先搜索的顺序加入list
    private void DFS_Create(Transform rt) {
        for (int i = 0; i < rt.childCount; i++) {
            list.Add(new TransformValue(rt.GetChild(i)));
            DFS_Create(rt.GetChild(i));
        }
    }

    private int p = 0;
    private void DFS_Load(Transform rt, List<TransformValue> L) {
        for (int i = 0; i < rt.childCount; i++) {
            if (rt.GetChild(i).name != L[p].name) {
                errorString = "Invaild transform!";
                return;
            }
            rt.GetChild(i).localPosition = L[p].localPosition.ToUnityVector3();
            rt.GetChild(i).localScale = L[p].scale.ToUnityVector3();
            rt.GetChild(i).eulerAngles = L[p].roration.ToUnityVector3();

            p++;
            DFS_Load(rt.GetChild(i), L);
        }
    }

    private void DeleteFile() {
        string path = Path.Combine(savePath, root.name + ".txt");
        if (File.Exists(path)) {
            File.Delete(path);
        }
        errorString = "Delete compeleted.";
    }

    //从文件中读取场景，恢复所有物体的Transfrom
    private void LoadFile() {
        errorString = "";
        string path = Path.Combine(savePath, root.name + ".txt");
        if (!File.Exists(path)) {
            errorString = "The file doesn't exist.";
            return;
        }
        List<TransformValue> L = JsonConvert.DeserializeObject<List<TransformValue>>(File.ReadAllText(path));

        p = 0;
        DFS_Load(root, L);
    }

    //从文件向当前场景创建动画
    private void CreateAnimation() {
        errorString = "功能还没有实现！";
        return;
        //string path = Path.Combine(savePath, root.name + ".txt");
        //List<TransformValue> L= JsonConvert.DeserializeObject<List<TransformValue>>(File.ReadAllText(path));      
    }
}
