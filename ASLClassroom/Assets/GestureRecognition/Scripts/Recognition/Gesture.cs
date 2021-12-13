using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public void CloseGesture()
    {
        complexPoses[_progress].ExitCPose();
    }

    //manage gesture progress
    //messages gesture recognizer about progress of currently observed gesture
    public GestureStates UpdateProgress(out string[] log, out KeyStates[] logStates)
    {
        GestureStates message = GestureStates.Pass;
        KeyStates complexState = complexPoses[_progress].GetKeysMet(out log, out logStates);
        switch (complexState)
        {
            case KeyStates.Hit:
                FinishPose(_progress);
                if (complexPoses.Count <= _progress + 1)
                {
                    message = GestureStates.Succeeded;
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
    public KeyStates GetKeysMet(out string[] log, out KeyStates[] logStates)
    {
        KeyStates state = KeyStates.Hit;
        bool failed = false;
        bool passed = false;

        log = new string[keys.Count];
        logStates = new KeyStates[keys.Count];
        for (int i = 0; i < keys.Count; i++)
        {
            KeyStates simpleState = keys[i].GetKeyMet();

            log[i] = keys[i].description;
            logStates[i] = simpleState;

            if (simpleState == KeyStates.Fail) failed = true;
            else if (simpleState == KeyStates.None) passed = true;

        }
        if (failed) return KeyStates.Fail;
        if (passed) return KeyStates.None;
        return state;
    }
}