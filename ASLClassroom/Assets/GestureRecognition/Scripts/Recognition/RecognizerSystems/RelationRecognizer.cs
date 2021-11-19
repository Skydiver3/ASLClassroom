using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static OVRPlugin;

public class RelationRecognizer : MonoBehaviour
{
    private static RelationRecognizer _instance;
    public static RelationRecognizer Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public enum RelativeObjects { HandLeft, HandRight, Head, Forehead, Nose, EarLeft, EarRight, Mouth, Chin, Chest, IndexTipLeft, IndexTipRight, ThumbTipLeft, ThumbTipRight, PalmLeft, PalmRight }
    public enum DistanceTypes { Far, Close, Touch, None }

    [SerializeField] private float maxTouching = 0.1f;
    [SerializeField] private float maxClose = 0.3f;

    [SerializeField] private Transform handL;
    [SerializeField] private Transform handR;

    [SerializeField] private Transform head;
    [SerializeField] private Transform forehead;
    [SerializeField] private Transform nose;
    [SerializeField] private Transform earL;
    [SerializeField] private Transform earR;
    [SerializeField] private Transform mouth;
    [SerializeField] private Transform chin;
    [SerializeField] private Transform chest;
    [SerializeField] private Transform indexTipLeft;
    [SerializeField] private Transform indexTipRight;
    [SerializeField] private Transform thumbTipLeft;
    [SerializeField] private Transform thumbTipRight;
    [SerializeField] private Transform palmLeft;
    [SerializeField] private Transform palmRight;

    [SerializeField] private Transform centerEye;

    [SerializeField] private TextMeshProUGUI logText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 target = indexTipLeft.position;
            Vector3 parent = centerEye.position;

            Vector3 local = target - parent;

            chin.localPosition = local;
            mouth.localPosition = local;
            earR.localPosition = local;
            earL.localPosition = local;
            chest.localPosition = local;
        }
    }


    //close together?
    //close to other body part
    //convert RelativeObjects type to Transform
    private Transform GetObject(RelativeObjects otherType)
    {
        Transform other = null;
        switch (otherType)
        {
            case RelativeObjects.HandLeft:
                other = handL;
                break;
            case RelativeObjects.HandRight:
                other = handR;
                break;
            case RelativeObjects.Head:
                other = head;
                break;
            case RelativeObjects.Forehead:
                other = forehead;
                break;
            case RelativeObjects.Nose:
                other = nose;
                break;
            case RelativeObjects.EarLeft:
                other = earL;
                break;
            case RelativeObjects.EarRight:
                other = earR;
                break;
            case RelativeObjects.Mouth:
                other = mouth;
                break;
            case RelativeObjects.Chin:
                other = chin;
                break;
            case RelativeObjects.Chest:
                other = chest;
                break;
            case RelativeObjects.IndexTipLeft:
                other = indexTipLeft;
                break;
            case RelativeObjects.IndexTipRight:
                other = indexTipRight;
                break;
            case RelativeObjects.ThumbTipLeft:
                other = thumbTipLeft;
                break;
            case RelativeObjects.ThumbTipRight:
                other = thumbTipRight;
                break;
            case RelativeObjects.PalmLeft:
                other = palmLeft;
                break;
            case RelativeObjects.PalmRight:
                other = palmRight;
                break;
            default:
                break;
        }
        return other;
    }
    //convert SkeletonType to Transform
    /* private Transform GetObject(SkeletonType handType)
    {
        Transform hand = handL;
        if (handType == SkeletonType.HandRight) hand = handR;
        return hand;
    }*/
    public DistanceTypes GetDistanceTo(RelativeObjects handType, RelativeObjects otherType)
    {
        Transform hand = GetObject(handType);
        Transform other = GetObject(otherType);

        return GetDistanceTo(hand, other);
    }
    private DistanceTypes GetDistanceTo(Transform hand, Transform other)
    {
        DistanceTypes type = DistanceTypes.None;

        float sqrDistance = Vector3.SqrMagnitude(other.position - hand.position);
        if (sqrDistance < maxTouching) type = DistanceTypes.Touch;
        else if (sqrDistance < maxClose) type= DistanceTypes.Close;
        else type= DistanceTypes.Far;

        logText.text = hand.name + " to " + other.name + " is " + type.ToString() + " (" + sqrDistance+")";
        return type;
    }
}


//gestures
//why: both palms, shake vertically
//learn: right palm, left pinch, index on palm to index close to head
//friend: both hook, middle index segments touch at both ends, 
