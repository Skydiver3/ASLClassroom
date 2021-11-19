using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PoseRecognizer))]
public class PoseRecognizerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        PoseRecognizer myTarget = (PoseRecognizer)target;

        if(GUILayout.Button("Add Pose Left"))
        {
            myTarget.SaveLeft();
        }if(GUILayout.Button("Add Pose Right"))
        {
            myTarget.SaveRight();
        }

    }
    
}
