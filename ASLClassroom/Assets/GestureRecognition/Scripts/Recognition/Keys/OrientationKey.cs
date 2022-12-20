using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OVRPlugin;

[CreateAssetMenu(fileName = "OrientationKey", menuName = "Gesture Keys/Orientation Key")]
public class OrientationKey : Key
{
    public PlayerData.BodyParts targetObject = PlayerData.BodyParts.HandDominant;

    //target orientation (pointing)
    //target orientation (facing)
    public enum OrientationType { Pointing, Facing }
    public OrientationType orientationType = OrientationType.Facing;

    public OrientationRecognizer.Directions targetDirection;
    private OrientationRecognizer.Directions originalDirection;

    [HideInInspector] public Transform orientedObject = null;

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

