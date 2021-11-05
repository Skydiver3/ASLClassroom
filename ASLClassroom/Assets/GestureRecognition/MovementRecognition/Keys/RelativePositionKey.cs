using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OVRPlugin;
using static RelationRecognizer;

[CreateAssetMenu(fileName = "RelativePositionKey", menuName = "Gesture Keys/Relative Position Key")]
public class RelativePositionKey : Key
{
    //reference to hand
    public RelativeObjects targetHand;
    //reference to other object
    public RelativeObjects relativeObject;
    //target distance
    public DistanceTypes targetDistance;
    private RelationRecognizer recognizer;


    public override void InitKey()
    {
        recognizer = RelationRecognizer.Instance;
    }
    public override void ExitKey()
    {
        Debug.Log("Exit RelativePos Key");
    }

    public override KeyStates GetKeyMet()
    {
        bool met = recognizer.GetDistanceTo(targetHand, relativeObject) == targetDistance;
        if (met) return KeyStates.Hit;
        else return KeyStates.None;
    }


}
