using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static OVRPlugin;

/// <summary>
/// Prototype of course management system. Deprecated.
/// </summary>
public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public struct Task_
    {
        public string gesture;
        public string instructions;
    }

    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI bubbleText;
    [SerializeField] private TextMeshProUGUI logText;
    [SerializeField] private List<Task_> backlog;
    [SerializeField] private PoseRecognizer poseRecognizer;
    private Task_ currentTask;

    [SerializeField] private float startDelay = 7;
    [SerializeField] private float afterPoseDelay = 1;
    [SerializeField] private string successMessage = "Good job!";
    [SerializeField] private string finishMessage = "Well done. You made it. Until next time!";
    //queue with tasks

    private bool tutorialRunning;
    private bool listening = true;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(startDelay);
        EnterTutorial();
    }
    private void OnDisable()
    {
        ExitTutorial();
    }

    private void StartTask(Task_ task)
    {
        currentTask = task;
        bubbleText.text = task.instructions;
        animator.Play(task.gesture);
    }

    private IEnumerator AdvanceTutorial()
    {
        listening = false;
        yield return new WaitForSeconds(afterPoseDelay);

        FinishTask();

        //wait until animation from finishTask has stopped playing
        yield return null;
        float delay = animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex("Base Layer")).length;        
        yield return new WaitForSeconds(delay);

        listening = true;

        if (backlog.Count > 0) backlog.RemoveAt(0);
        if (backlog.Count > 0) StartTask(backlog[0]);
        else FinishTutorial();
    }

    private void FinishTask()
    {
        bubbleText.text = successMessage;
        animator.Play("Success");
    }

    private void FinishTutorial()
    {
        bubbleText.text = finishMessage;
        listening = false;
    }
    private void EnterTutorial()
    {
        poseRecognizer.PoseRecognizedEvent += OnGestureRecognized;
        tutorialRunning = true;
        if (backlog.Count > 0) StartTask(backlog[0]);
    }
    private void ExitTutorial()
    {
        if (tutorialRunning)
        {
            poseRecognizer.PoseRecognizedEvent -= OnGestureRecognized;
            listening = false;
        }
    }

    public void OnGestureRecognized(string gestureName, SkeletonType handType)
    {
        if (listening&&gestureName == currentTask.gesture)
        {
            StartCoroutine(AdvanceTutorial());
        }
    }

}
