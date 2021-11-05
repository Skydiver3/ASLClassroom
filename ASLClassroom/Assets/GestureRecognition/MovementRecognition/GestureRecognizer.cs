using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GestureRecognizer : MonoBehaviour
{
    public Gesture templateGesture;
    public Gesture currentGesture;
    public TextMeshProUGUI progressLog2;
    private void Start()
    {
        currentGesture = InstantiateGesture(templateGesture);
        currentGesture.InitGesure();
    }
    private void Update()
    {
        //instantiating may take a while, don't update before finished
        if (currentGesture == null) return;

        Gesture.GestureStates gestureState;

        string log;
        gestureState = currentGesture.UpdateProgress(out log);

        if (gestureState == Gesture.GestureStates.Succeeded)
        {
            log += "\n\n Gesture succeeded";
            currentGesture.InitGesure();
        }
        else if (gestureState == Gesture.GestureStates.Failed)
        {
            log += "\n\n Gesture failed";
            currentGesture.InitGesure();
        }
        else if (gestureState == Gesture.GestureStates.Advance)
        {
            log += "\n\n Gesture advanced: " + currentGesture.Progress;
        }
        progressLog2.text = log;
    }

    private Gesture InstantiateGesture(Gesture templateGesture)
    {
        Gesture instanceGesture = Object.Instantiate(templateGesture);

        foreach (ComplexPose pose in instanceGesture.complexPoses)
        {
            for (int i = 0; i < pose.keys.Count; i++)
            {
                pose.keys[i] = Object.Instantiate(pose.keys[i]);
            }
        }

        return instanceGesture;
    }
}
