using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OVRPlugin;
using static RelationRecognizer;

/// <summary>
/// The Key for the position of the player's hands relative to other markers. Are they next to the head? The chest?
/// Asks the relative position recognizer in the scene to calculate the distance between the specified objects to see if they're close enough.
/// </summary>
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
    }

    public override KeyStates GetKeyMet()
    {
        bool met = recognizer.GetDistanceTo(targetHand, relativeObject) == targetDistance;
        if (met) return KeyStates.Hit;
        else return KeyStates.None;
    }


}
