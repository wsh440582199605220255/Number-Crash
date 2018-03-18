using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Animations;

public class PrefabsToAnim : ScriptableWizard
{

    public Transform obj1, obj2;
    public string AnimationPath = "Assets/Animations";
    public Command cmd;

    public enum Command
    {
        Recover_obj1 = 0,
        Create_Animation,
        Custom,
    }


    [MenuItem("Tools/Prefabs To Anim")]
    static void CreateWizerd()
    {
        DisplayWizard("Prefabs To Anim", typeof(PrefabsToAnim), "Close", "RunCommand");
    }


    private void OnWizardCreate()
    {
    }


    private void OnWizardOtherButton()
    {
        switch (cmd)
        {
            case Command.Recover_obj1:
                Recover(); break;
            case Command.Create_Animation:
                CreateAnimation(); break;
            case Command.Custom:
                CustomFunc(); break;
        }
    }

    //Write your custom code here
    private void CustomFunc()
    {

    }

    //用obj2的Transform恢复obj1
    private void Recover()
    {
        errorString = "";
        if (CheckObjs())
        {
            DFS_recover(obj1, obj2);
        }
    }

    private void DFS_recover(Transform rt1, Transform rt2)
    {
        for (int i = 0; i < rt1.childCount; i++)
        {
            if (rt1.GetChild(i).name != rt2.GetChild(i).name)
            {
                errorString = "Invaild transform!";
                return;
            }
            rt1.GetChild(i).localPosition = rt2.GetChild(i).localPosition;
            rt1.GetChild(i).eulerAngles = rt2.GetChild(i).eulerAngles;
            rt1.GetChild(i).localScale = rt2.GetChild(i).localScale;

            DFS_recover(rt1.GetChild(i), rt2.GetChild(i));
        }
    }


    AnimationClip clip;
    EditorCurveBinding binding;
    AnimationCurve curve;
    Keyframe[][] keys;
    private void CreateAnimation()
    {
        errorString = "";
        if (CheckObjs())
        {
            clip = new AnimationClip();
            clip.frameRate = 24;

            //AnimationClipSettings clipSettings = new AnimationClipSettings();
            //clipSettings.loopTime = true;
            //AnimationUtility.SetAnimationClipSettings(clip, clipSettings);

            keys = new Keyframe[3][];
            for (int i = 0; i < keys.Length; i++) keys[i] = new Keyframe[2];
            binding = new EditorCurveBinding();
            curve = new AnimationCurve();

            DFS_Anim(obj1, obj2);

            //生成Assets
            AssetDatabase.CreateAsset(clip, Path.Combine(AnimationPath, obj1.name + ".anim"));
            AssetDatabase.SaveAssets();
        }
    }


    private void DFS_Anim(Transform rt1, Transform rt2)
    {
        for (int i = 0; i < rt1.childCount; i++)
        {
            Transform rt1Child = rt1.GetChild(i);
            Transform rt2Child = rt2.GetChild(i);

            if (rt1Child.name != rt2Child.name)
            {
                errorString = "Invaild transform!";
                return;
            }

            if (rt1Child.localPosition != rt2Child.localPosition)
            {
                AddProperty(rt1Child, rt1Child.localPosition, rt2Child.localPosition, "m_LocalPosition");
            }

            if (rt1Child.localScale != rt2Child.localScale)
            {
                AddProperty(rt1Child, rt1Child.localScale, rt2Child.localScale, "m_LocalScale");
            }

            if (rt1Child.localRotation != rt2Child.localRotation)
            {
                AddProperty(rt1Child, rt1Child.localEulerAngles, rt2Child.localEulerAngles, "m_LocalRotation");
            }

            DFS_Anim(rt1Child, rt2Child);
        }
    }

    private readonly string[] V3str = { ".x", ".y", ".z" };
    private void AddProperty(Transform curr, Vector3 v1, Vector3 v2, string propertyName)
    {

        keys[0][0].value = v1.x;
        keys[0][1].value = v2.x;
        keys[1][0].value = v1.y;
        keys[1][1].value = v2.y;
        keys[2][0].value = v1.z;
        keys[2][1].value = v2.z;

        for (int j = 0; j < 3; j++)
        {
            keys[j][0].time = 0f;
            keys[j][1].time = 1f;


            binding.path = Q.GetFullPath(curr, false);
            binding.type = typeof(Transform);
            binding.propertyName = propertyName + V3str[j];

            curve.keys = keys[j];

            //设置Property
            AnimationUtility.SetEditorCurve(clip, binding, curve);
        }
    }




    private bool CheckObjs()
    {
        if (obj1 == null || obj2 == null)
        {
            errorString = "Both obj can't be null.";
            return false;
        }
        return true;
    }
}
