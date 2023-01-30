using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OVRPlugin;

/// <summary>
/// The Key for hand orientation. Where is the hand pointing? Where is it facing? 
/// Directions relative to world coordinates.
/// Subscribes to Orientation Recognizer in the scene and compares the recognized current orientation to success and fail criteria.
/// </summary>
[CreateAssetMenu(fileName = "OrientationKey", menuName = "Gesture Keys/Orientation Key")]
public class OrientationKey : Key
{
    public PlayerData.BodyParts targetObject = PlayerData.BodyParts.HandDominant;

    public enum OrientationType { Pointing, Facing }
    public OrientationType orientationType = OrientationType.Facing;

    public OrientationRecognizer.Directions targetDirection;
    private OrientationRecognizer.Directions originalDirection;

    [HideInInspector] public Transform orientedObject = null;

    //Get initial hand orientation, that one would not count as a failure.
    public override void InitKey()
    {
        orientedObject = null;
        GetObject();

        if (orientationType == OrientationType.Facing)
        {
            originalDirection = OrientationRecognizer.Instance.GetFacingDirection(orientedObject);
        }
        else
        {
            originalDirection = OrientationRecognizer.Instance.GetForwardDirection(orientedObject);
        }
    }
    public override void ExitKey()
    {
        orientedObject = null;
    }

    //Compares current orientation data from the recognizer object in the scene to specified success and fail criteria.
    public override KeyStates GetKeyMet()
    {
        if (orientedObject==null)
        {
            InitKey();
            if (orientedObject ==null) return KeyStates.None;
        }

        OrientationRecognizer.Directions direction;
        if (orientationType == OrientationType.Facing)
        {
            direction = OrientationRecognizer.Instance.GetFacingDirection(orientedObject);
        }
        else
        {
            direction = OrientationRecognizer.Instance.GetForwardDirection(orientedObject);
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

    private void GetObject()
    {
        orientedObject = PlayerObjectReferences.Instance.GetObject(targetObject);
    }
}

