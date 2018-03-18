using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BatchRename : ScriptableWizard {
    public string BaseName = "Obj_";
    public int StartIndex = 0;
    public int Increment = 1;

    [MenuItem("Tools/Batch Rename...")]
    static void CreateWizerd() {
        DisplayWizard("Batch Rename", typeof(BatchRename), "Rename");
    }

    private void OnEnable() {
        UpdateSeletionNumber();
    }

    private void OnSelectionChange() {
        UpdateSeletionNumber();
    }

    private void UpdateSeletionNumber() {
        helpString = "";
        if (Selection.objects != null) {
            helpString = "Number of objects selected:" + Selection.objects.Length;
        }
    }

    private void OnWizardCreate() {
        if (Selection.objects == null) return;

        int i = StartIndex;
        
        foreach (Object obj in Selection.objects) {
            obj.name = BaseName + i.ToString();
            i += Increment;
        }
    }
}
