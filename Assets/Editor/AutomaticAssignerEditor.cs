using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AutomaticAssignerScript))]
public class AutomaticAssignerEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        AutomaticAssignerScript myTarget = target as AutomaticAssignerScript;
        if (GUILayout.Button("Assign Files")) {
            myTarget.AssignAudioFiles();
        }
    }

}
