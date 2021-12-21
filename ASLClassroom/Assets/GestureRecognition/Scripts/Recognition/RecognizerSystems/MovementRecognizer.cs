using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovementRecognizer : MonoBehaviour
{
    private static MovementRecognizer _instance;
    public static MovementRecognizer Instance { get { return _instance; } }

    enum MovementTypes { Arc, StraightLine, Circle, AlternatingInOut, WristTwist, FingerFlick}
    public enum Directions { Up, Down, Left, Right, Forward, Back, Still, Invalid }

    [SerializeField] TextMeshProUGUI logTextR;
    [SerializeField] TextMeshProUGUI logTextL;

    private float minSpeed = 100;

    [SerializeField] Transform handL;
    [SerializeField] Transform handR;

    //direction recognition
    private Vector3 previousPositionL;
    private Vector3 previousPositionR;
    private Vector3 velocityL= Vector3.zero;
    private Vector3 velocityR= Vector3.zero;
    private float currentMoveSpeedR;
    private float currentMoveSpeedL;
    private Directions currentMoveDirectionR;
    private Directions currentMoveDirectionL;

    //line recognition
    private float minLineDistance = 0.05f;
    private float currentLineDistanceR;
    private Directions currentLineDirectionL = Directions.Invalid;
    private Directions currentLineDirectionR = Directions.Invalid;
    private Vector3 lineOriginPositionL;
    private Vector3 lineOriginPositionR;
    public Directions currentLineL = Directions.Invalid;
    public Directions currentLineR = Directions.Invalid;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(this.gameObject);
    }

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
                yield return new WaitForFixedUpdate();
            }

            currentMoveDirectionL = GetMoveDirection(handL);
            currentMoveDirectionR = GetMoveDirection(handR);
            CheckForStraightLine();

            if (logTextR) logTextR.text = "Direction: " + currentMoveDirectionR + " " + currentMoveSpeedR + "\n" + "Line drawn: " + currentLineR + "\n" + currentLineDistanceR;
            if (logTextL) logTextL.text = "Direction: " + currentMoveDirectionL + " " + currentMoveSpeedL + "\n" + "Line drawn: " + currentLineL;

            previousPositionL = handL.transform.position;
            previousPositionR = handR.transform.position;
        }
    }

    private Directions GetMoveDirection(Transform hand)
    {
        //get velocity of hand, get speed
        Vector3 previousPosition;
        previousPosition = (hand == handL) ? previousPositionL : previousPositionR;

        //get velocity
        Vector3 velocity;
        velocity = hand.transform.position - previousPosition;
        velocity *= 10000;

        //set currentMoveSpeed
        float currentMoveSpeed;
        currentMoveSpeed = Vector3.Magnitude(velocity);

        //assign velocity and speed to global hand-specific variables
        if (hand == handL) { currentMoveSpeedL = currentMoveSpeed; velocityL = velocity; }
        else { currentMoveSpeedR = currentMoveSpeed; velocityR = velocity; }

        //determine where velocity is pointing
        if (currentMoveSpeed < minSpeed) return Directions.Still;

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
        //abort line if still or invalid
        if (currentMoveDirectionL == Directions.Still || currentMoveDirectionL == Directions.Invalid)
        {
            lineOriginPositionL = handL.position;
            currentLineDirectionL = currentMoveDirectionL;
            currentLineL = Directions.Invalid;
        }
        if (currentMoveDirectionR == Directions.Still || currentMoveDirectionR == Directions.Invalid)
        {
            lineOriginPositionR = handR.position;
            currentLineDirectionR = currentMoveDirectionR;
            currentLineR = Directions.Invalid;
        }

        //if has been moving in the same direction for a certain distance: line success
        bool movingInLineDirectionL = currentMoveDirectionL == currentLineDirectionL;
        if (movingInLineDirectionL)
        {
            float lineLength = (lineOriginPositionL - handL.position).sqrMagnitude;
            bool lineLongEnough = lineLength >= minLineDistance * minLineDistance;

            //line success
            if (lineLongEnough) currentLineL = currentLineDirectionL;
            //line not long enough
            else currentLineL = Directions.Invalid;
        }
        else
        {
            //direction has changed, reset origin position, line failed
            //weakness: only updated when checked. Check constantly for accurate result.
            lineOriginPositionL = handL.position;
            currentLineDirectionL = currentMoveDirectionL;
            currentLineL = Directions.Invalid;
        }

        //the same for other hand
        bool movingInLineDirectionR = currentMoveDirectionR == currentLineDirectionR;
        if (movingInLineDirectionR)
        {
            float lineLength = (lineOriginPositionR - handR.position).sqrMagnitude;
            bool lineLongEnough = lineLength >= minLineDistance * minLineDistance;
            currentLineDistanceR = (lineOriginPositionR - handR.position).magnitude;

            if (lineLongEnough) currentLineR = currentLineDirectionR;
            else currentLineR = Directions.Invalid;
        }
        else
        {
            lineOriginPositionR = handR.position;
            currentLineDirectionR = currentMoveDirectionR;
            currentLineR = Directions.Invalid;
        }
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
