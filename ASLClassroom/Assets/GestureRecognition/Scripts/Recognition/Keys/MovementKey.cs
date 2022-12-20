using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static MovementRecognizer;

[CreateAssetMenu(fileName = "MovementKey", menuName = "Gesture Keys/Movement Key")]
public class MovementKey : Key
{
    //Include option to ask for both hands!

    private MovementRecognizer recognizer;

    public enum HandTypes { LeftHand, RightHand, BothHands }
    public HandTypes handType;

    [HideInInspector] public Directions lineDirectionL;
    [HideInInspector] public Directions lineDirectionR;

    public override void ExitKey()
    {
        //i hope you have a nice day
    }

    public override KeyStates GetKeyMet()
    {
        KeyStates stateL = KeyStates.Hit;
        KeyStates stateR = KeyStates.Hit;
        if (handType == HandTypes.LeftHand || handType == HandTypes.BothHands)
        {
            //check for left hand
            Directions directionL = recognizer.currentLineL;
            bool rightDirection = directionL == lineDirectionL;

            if (rightDirection) stateL = KeyStates.Hit;
            else if (directionL == Directions.Still || directionL == Directions.Invalid) stateL = KeyStates.None;
            else stateL = KeyStates.Fail;
        }
        if (handType == HandTypes.RightHand || handType == HandTypes.BothHands)
        {
            //check for right hand
            Directions directionR = recognizer.currentLineR;
            bool rightDirection = directionR == lineDirectionR;

            if (rightDirection) stateR = KeyStates.Hit;
            else if (directionR == Directions.Still || directionR == Directions.Invalid) stateR = KeyStates.None;
            else stateR = KeyStates.Fail;
        }

        //fail if either fails, success if both succeed, nothing otherwise
        if (stateL == KeyStates.Fail || stateR == KeyStates.Fail) return KeyStates.Fail;
        if (stateL == KeyStates.Hit && stateR == KeyStates.Hit) return KeyStates.Hit;
        return KeyStates.None;
    }

    public override void InitKey()
    {
        recognizer = MovementRecognizer.Instance;
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(MovementKey))]
    public class MovementKeyEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var script = (MovementKey)target;

            DrawDefaultInspector();



            if (script.handType == HandTypes.LeftHand || script.handType == HandTypes.BothHands)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("lineDirectionL"));
            }
            if (script.handType == HandTypes.RightHand || script.handType == HandTypes.BothHands)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("lineDirectionR"));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
