using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GestureRecognizer : MonoBehaviour
{
    public Gesture templateGesture;
    public Gesture currentGesture;
    public TextMeshProUGUI progressLog;
    private void Start()
    {
        progressLog.text = "Init Gesture";
        currentGesture = InstantiateGesture(templateGesture);
        currentGesture.InitGesure();
    }
    private void Update()
    {
        //instantiating may take a while, don't update before finished
        if (currentGesture == null) return;

        Gesture.GestureStates gestureState;
        gestureState = currentGesture.UpdateProgress();

        if (gestureState == Gesture.GestureStates.Succeeded)
        {
            progressLog.text = "Gesture succeeded";
            currentGesture.InitGesure();
        }
        else if (gestureState == Gesture.GestureStates.Failed)
        {
            progressLog.text = "Gesture failed";
            currentGesture.InitGesure();
        }
        else if (gestureState == Gesture.GestureStates.Advance)
        {
            progressLog.text = "Gesture advanced: "+currentGesture.Progress;
        }
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
