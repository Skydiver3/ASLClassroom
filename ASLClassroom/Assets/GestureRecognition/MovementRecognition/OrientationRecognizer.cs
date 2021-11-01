using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrientationRecognizer : MonoBehaviour
{
    private static OrientationRecognizer _instance;
    public static OrientationRecognizer Instance { get { return _instance; } }

    public enum Directions
    {
        Self, Away, Up, Down, Left, Right
    }

    //TODO: rotate by head rotation (not y)
    private Vector3[] axesR = { Vector3.back, Vector3.forward, Vector3.up, Vector3.down, Vector3.left, Vector3.right };
    private Vector3[] axesL = { Vector3.forward, Vector3.back, Vector3.down, Vector3.up, Vector3.right, Vector3.left };

    [SerializeField]
    public Transform handL;
    [SerializeField]
    public Transform handR;

    [SerializeField]
    private TextMeshProUGUI logTextL;
    [SerializeField]
    private TextMeshProUGUI logTextR;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    private void Update()
    {
        logTextL.text = "forward: " + GetForwardDirection(handL) + "\nfacing: " + GetFacingDirection(handL);
        logTextR.text = "forward: " + GetForwardDirection(handR) + "\nfacing: " + GetFacingDirection(handR);
    }

    public Directions GetForwardDirection(Transform hand)
    {
        return GetDirection(hand.right, hand);
    }

    public Directions GetFacingDirection(Transform hand)
    {
        return GetDirection(-hand.up, hand);
    }
    private Directions GetDirection(Vector3 v,Transform hand)
    {
        Vector3[] axes;
        if (hand == handL) axes = axesL;
        else axes = axesR;

        float minDistance = Mathf.Infinity;
        int minIndex = 0;
        for (int i = 0; i < axes.Length; i++)
        {
            float sqrDistance = (axes[i] - v).sqrMagnitude;
            if (sqrDistance < minDistance)
            {
                minDistance = sqrDistance;
                minIndex = i;
            }
        }
        return (Directions)minIndex;        
    }


}