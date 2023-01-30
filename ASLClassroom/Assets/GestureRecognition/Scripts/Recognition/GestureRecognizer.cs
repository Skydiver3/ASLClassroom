using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The Gesture Recognizer is one possible Task from the ASL Tutorial. It is completed when the user performs a certain gesture. 
/// This class handles navigation between the different stages of the gesture and notifies the display accordingly.
/// </summary>
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
        onTaskBegin?.Invoke();
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

            //analyze current hand state in regards to the gesture we are looking for
            string[] log = null;
            KeyStates[] logStates = null;
            Sprite[] keySprites = null;
            gestureState = gestureInstance.UpdateProgress(out log, out logStates, out keySprites);

            //how does the hand state interact with the gesture? does it complete the gesture? does it break it? does it advance it to the next stage? or does it do nothing?
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

            //Display the current state of the gesture on the checklist screen
            string progressText = "(" + (gestureInstance.Progress + 1) + "/" + gestureInstance.complexPoses.Count + ")";
            DisplayCheckList(templateGesture.name + progressText, templateGesture.description, log, logStates, keySprites);
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

    protected virtual void DisplayCheckList(string gestureName, string gestureDescription, string[] messages, KeyStates[] states, Sprite[] sprites)
    {
        ChecklistManager.Instance.UpdateItems(gestureName, gestureDescription, messages, states, sprites);
    }

    private IEnumerator PauseUpdateForSeconds(float seconds)
    {
        isUpdating = false;
        yield return new WaitForSeconds(seconds);
        isUpdating = true;
    }
}
