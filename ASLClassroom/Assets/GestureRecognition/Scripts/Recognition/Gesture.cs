using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

/// <summary>
/// A Gesture describes a word from ASL, where a sequence of Complex Poses (made up of movements or hand poses for example) conveys a certain meaning.
/// </summary>
[CreateAssetMenu(fileName = "Gesture", menuName = "Gesture Keys/Gesture")]
public class Gesture : ScriptableObject
{
    public enum GestureStates { Advance, Failed, Succeeded, Pass }
    public string description;
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
    public GestureStates UpdateProgress(out string[] log, out KeyStates[] logStates, out Sprite[] keySprites)
    {
        GestureStates message = GestureStates.Pass;
        KeyStates complexState = complexPoses[_progress].GetKeysMet(out log, out logStates, out keySprites);
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

/// <summary>
/// A Complex Pose is a collection of simple keys like for position or pose, used like keyframes that make up a gesture. 
/// A Complex Pose might be an open palm that faces forward, like in the YOURS gesture. Also referred to as CPose.
/// </summary>
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
    public KeyStates GetKeysMet(out string[] log, out KeyStates[] logStates, out Sprite[] keySprites)
    {
        KeyStates state = KeyStates.Hit;
        bool failed = false;
        bool passed = false;

        log = new string[keys.Count];
        logStates = new KeyStates[keys.Count];
        keySprites = new Sprite[keys.Count];
        for (int i = 0; i < keys.Count; i++)
        {
            KeyStates simpleState = keys[i].GetKeyMet();

            log[i] = keys[i].description;
            logStates[i] = simpleState;
            keySprites[i] = keys[i].sprite;

            if (simpleState == KeyStates.Fail) failed = true;
            else if (simpleState == KeyStates.None) passed = true;

        }
        if (failed) return KeyStates.Fail;
        if (passed) return KeyStates.None;
        return state;
    }
}