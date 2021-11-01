using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OVRPlugin;

[CreateAssetMenu(fileName = "OrientationKey", menuName = "Gesture Keys/Orientation Key")]
public class OrientationKey : Key
{
    public SkeletonType targetHand = SkeletonType.HandLeft;
    public enum OrientationType { Pointing, Facing }
    public OrientationType orientationType = OrientationType.Facing;
    public Transform hand = null;
    //key type specific variables
    //target orientation (pointing)
    //target orientation (facing)
    public OrientationRecognizer.Directions targetDirection;
    private OrientationRecognizer.Directions originalDirection;


    public override void InitKey()
    {
        hand = null;
        GetHand();

        if (orientationType == OrientationType.Facing)
        {
            originalDirection = OrientationRecognizer.Instance.GetFacingDirection(hand);
        }
        else
        {
            originalDirection = OrientationRecognizer.Instance.GetForwardDirection(hand);
        }
    }
    public override void ExitKey()
    {
        hand = null;
    }

    public override KeyStates GetKeyMet()
    {
        if (hand==null)
        {
            InitKey();
            if (hand ==null) return KeyStates.None;
        }

        OrientationRecognizer.Directions direction;
        if (orientationType == OrientationType.Facing)
        {
            direction = OrientationRecognizer.Instance.GetFacingDirection(hand);
        }
        else
        {
            direction = OrientationRecognizer.Instance.GetForwardDirection(hand);
        }

        if (direction == targetDirection)
        {
            return KeyStates.Hit;
        }
        else if (direction == originalDirection)
        {
            return KeyStates.None;
        }
        else
        {
            return KeyStates.Fail;
        }
    }

    private void GetHand()
    {
        if (targetHand == SkeletonType.HandLeft) hand = OrientationRecognizer.Instance.handL.transform;
        else hand = OrientationRecognizer.Instance.handR.transform;
    }
}

