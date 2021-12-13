using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovementRecognizer : MonoBehaviour
{
    enum MovementTypes { Arc, StraightLine, Circle, AlternatingInOut, WristTwist, FingerFlick}
    enum Directions { Up, Down, Left, Right, Forward, Back, Still, Invalid }

    [SerializeField] TextMeshProUGUI logText;

    //hand left
    //hand right
    [SerializeField] Transform handL;
    [SerializeField] Transform handR;
    private Vector3 previousPositionL;
    private Vector3 previousPositionR;
    private float minSpeed = 170;
    private float speed;
    private Vector3 velocity= Vector3.zero;


    private void Start()
    {
        StartCoroutine(SlowUpdate(7));
    }


    private IEnumerator SlowUpdate(int t)
    {
        while (true)
        {
            for (int i = 0; i < t; i++)
            {
                yield return null;
            }

            logText.text = GetMoveDirection(handR) + " " + speed;
            previousPositionL = handL.transform.position;
            previousPositionR = handR.transform.position;
        }
    }

    private Directions GetMoveDirection(Transform hand)
    {
        //get velocity of hand, get speed
        Vector3 previousPosition;
        previousPosition = (hand == handL) ? previousPositionL : previousPositionR;
        velocity = hand.transform.position - previousPosition;
        velocity *= 10000;
        speed = Vector3.Magnitude(velocity);
        //velocity = Vector3.Normalize(velocity);

        if (speed < minSpeed) return Directions.Still;

        //determine where velocity is pointing
        Vector3[] DirectionVectors = new Vector3[] { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
        float smallestDistance = Mathf.Infinity;
        int directionIndex=(int)Directions.Invalid;
        for (int i = 0; i < DirectionVectors.Length; i++)
        {
            Vector3 dv = DirectionVectors[i] - velocity;
            float dm = Vector3.SqrMagnitude(dv);
            if (dm < smallestDistance)
            {
                smallestDistance = dm;
                directionIndex = i;
            }
        }

        return (Directions)directionIndex;
    }

    private void CheckForStraightLine()
    {
        //compare velocity vector to previous vector v, save v
        //if angle of vector is bigger than x -> not straight anymore
        //save new v
        //save new v every time that number has been exceeded
        //if v has been the same for y time -> straight line = true
        //also tell in which of six directions relative to body(middle eye) the movement went
    }

    private void CheckForArc()
    {
        //horizontal arc:
        //moves up, then down
        //while moving along a straight axis on the xz plane
    }

    private void CheckForCircle()
    {
        //determine plane
        //get points with polling
        //get normal vector to line between points
        //poll again
        //get intersection point with one of the last normal vectors
        //poll again
        //if intersection point is near average of past intersection points: circular motion in progress
    }

    
}
