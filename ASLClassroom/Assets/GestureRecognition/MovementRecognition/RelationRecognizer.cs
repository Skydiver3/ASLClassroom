using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationRecognizer : MonoBehaviour
{
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
    [SerializeField] private Transform chest;

    //close together?
    //close to other body part

    private bool GetCloseTo(Transform hand, Transform t)
    {
        return Vector3.SqrMagnitude(t.position - hand.position) < maxClose;
    }
    private bool GetTouch(Transform hand, Transform t)
    {
        return Vector3.SqrMagnitude(t.position - hand.position) < maxTouching;
    }
}


//gestures
//why: both palms, shake vertically
//learn: right palm, left pinch, index on palm to index close to head
//friend: both hook, middle index segments touch at both ends, 
