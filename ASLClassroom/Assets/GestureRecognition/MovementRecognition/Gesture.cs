using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "Gesture", menuName = "Gesture Keys/Gesture")]
public class Gesture : ScriptableObject
{
    public enum GestureStates { Advance, Failed, Succeeded, Pass }
    public List<ComplexPose> complexPoses;

    private int _progress = 0;
    public int Progress { get { return _progress; } }

    public void InitGesure()
    {
        complexPoses[0].InitCPose();
    }

    //manage gesture progress
    //messages gesture recognizer about progress of currently observed gesture
    public GestureStates UpdateProgress()
    {
        GestureStates message = GestureStates.Pass;
        KeyStates complexState = complexPoses[_progress].GetKeysMet();
        switch (complexState)
        {
            case KeyStates.Hit:
                FinishPose(_progress);
                if (complexPoses.Count <= _progress + 1)
                {
                    message = GestureStates.Succeeded;
                    ResetProgress();
                    break;
                }
                AdvanceProgress();
                message = GestureStates.Advance;
                break;
            case KeyStates.Fail:
                ResetProgress();
                message = GestureStates.Failed;
                break;
        }
        return message;
    }
    private void AdvanceProgress()
    {
        _progress++;
        complexPoses[_progress].InitCPose();
    }
    private void ResetProgress()
    {
        _progress = 0;
    }
    private void FinishPose(int index)
    {
        complexPoses[index].ExitCPose();
    }

}

[System.Serializable]
public class ComplexPose
{
    public List<Key> keys;
    public void InitCPose()
    {
        foreach (Key key in keys)
        {
            key.InitKey();
        }
    }
    public void ExitCPose()
    {
        foreach (Key key in keys)
        {
            key.ExitKey();
        }
    }

    //analyze key state:
    //if any simple key fails, pose automatically fails
    //if any simple key is not yet reached, pose does not succeed yet
    public KeyStates GetKeysMet()
    {
        KeyStates state = KeyStates.Hit;

        foreach (Key key in keys)
        {
            Debug.Log(key.name + " " + key.GetKeyMet().ToString());
            KeyStates simpleState = key.GetKeyMet();

            if (simpleState == KeyStates.Fail) return simpleState;
            else if (simpleState == KeyStates.None) state = KeyStates.None;
        }
        return state;
    }
}