using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OVRPlugin;

[CreateAssetMenu(fileName = "PoseKey", menuName = "Gesture Keys/Pose Key")]
public class PoseKey : Key
{
    public SkeletonType targetHand = SkeletonType.HandLeft;

    //key type specific variables
    public string targetPose;
    public List<string> ignorePoses;
    private string originalPose;
    private string currentPose;
    private KeyStates state = KeyStates.None;
    private PoseRecognizer recognizer;

    //subscribes to detection system
    public override void InitKey()
    {
        recognizer = PoseRecognizer.Instance;
        recognizer.PoseRecognizedEvent += OnPoseRecognized;

        originalPose = recognizer.Recognize(targetHand).name;
        currentPose = originalPose;
    }
    public override void ExitKey()
    {
        recognizer.PoseRecognizedEvent -= OnPoseRecognized;
    }

    private void OnPoseRecognized(string s, SkeletonType handType)
    {
        if(handType == targetHand) currentPose = s;        
    }
    public override KeyStates GetKeyMet()
    {
        if (currentPose == targetPose)
        {
            state = KeyStates.Hit;
        }
        else if (currentPose == originalPose)
        {
            state = KeyStates.None;
        }
        else if (ignorePoses.Contains(currentPose))
        {
            state = KeyStates.None;
        }
        else
        {
            state = KeyStates.Fail;
        }

        return state;
        //if target pose recognized on targetHand: return Hit
        //if no new pose detected: return None
        //if other pose detected: return Fail
    }

}