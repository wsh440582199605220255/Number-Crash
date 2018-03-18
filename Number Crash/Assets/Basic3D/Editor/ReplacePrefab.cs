using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ReplacePrefab : ScriptableWizard
{
    public GameObject prefab;
    public string targetName = "Default";

    [MenuItem("Tools/Batch Replace Prefab")]
    static void CreateWizerd()
    {
        DisplayWizard("Batch Replace Prefab", typeof(ReplacePrefab), "Go!");
    }

    private void OnEnable()
    {
        helpString = "The targetGameObject must be active.";
    }

    private void OnWizardCreate()
    {
        if (prefab == null)
        {
            errorString = "The prefab can't be empty!";
            return;
        }

        if (targetName == "Default") targetName = prefab.name;

        GameObject[] Gb = FindObjectsOfType<GameObject>();

        foreach (var item in Gb)
        {

            if (item != null && item.name == targetName) PrefabUtility.ConnectGameObjectToPrefab(item, prefab);


        }
    }
}
