using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GestureRecognizer : Task
{
    [Tooltip("The amount of time the task waits before listening for gestures in case there is an animation")]
    public float startDelay;

    public Gesture templateGesture;
    private Gesture gestureInstance;

    public float delayBetweenCPoses = 0.3f;

    private bool isUpdating = false;
    private Coroutine pauseChecklistCorioutine;

    private void OnEnable()
    {
        if (startDelay > 0) StartCoroutine(PauseUpdateForSeconds(startDelay));
        isUpdating = true;
        gestureInstance = InstantiateGesture(templateGesture);
        gestureInstance.InitGesure();
    }
    private void OnDisable()
    {
        //unsubscribe keys from observers
        gestureInstance.CloseGesture();
        Destroy(gestureInstance);
    }
    private void Update()
    {
        if (isUpdating)
        {
            //instantiating may take a while, don't update before finished
            if (gestureInstance == null) return;

            Gesture.GestureStates gestureState = Gesture.GestureStates.Pass;

            string[] log = null;
            KeyStates[] logStates = null;
            gestureState = gestureInstance.UpdateProgress(out log, out logStates);

            if (gestureState == Gesture.GestureStates.Succeeded)
            {
                onTaskSucceeded?.Invoke();
            }
            else if (gestureState == Gesture.GestureStates.Failed)
            {
                onTaskFailed?.Invoke();
                gestureInstance.InitGesure();
                pauseChecklistCorioutine = StartCoroutine(PauseUpdateForSeconds(delayBetweenCPoses));
            }
            else if (gestureState == Gesture.GestureStates.Advance)
            {
                onTaskAdvanced?.Invoke();
                //wait a bit before advancing to show user feedback
                pauseChecklistCorioutine = StartCoroutine(PauseUpdateForSeconds(delayBetweenCPoses));
            }
            string progressText = "(" + (gestureInstance.Progress + 1) + "/" + gestureInstance.complexPoses.Count + ")";
            DisplayCheckList(templateGesture.name + progressText, log, logStates);
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

    private void DisplayCheckList(string gestureName, string[] messages, KeyStates[] states)
    {
        ChecklistManager.Instance.UpdateItems(gestureName, messages, states);
    }

    private IEnumerator PauseUpdateForSeconds(float seconds)
    {
        isUpdating = false;
        yield return new WaitForSeconds(seconds);
        isUpdating = true;
    }
}
