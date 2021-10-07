using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using TMPro;

[System.Serializable]
public struct Task
{
    public string gesture;
    public string instructions;
}

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI bubbleText;
    [SerializeField] private List<Task> backlog;
    [SerializeField] private PoseRecognizer poseRecognizer;
    private Task currentTask;

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

    private void StartTask(Task task)
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
        ExitTutorial();
    }
    private void EnterTutorial()
    {
        poseRecognizer.PoseRecognizedEvent += OnGestureRecognized;
        tutorialRunning = true;
        if (backlog.Count > 0) StartTask(backlog[0]);
    }
    private void ExitTutorial()
    {
        if(tutorialRunning)
            poseRecognizer.PoseRecognizedEvent -= OnGestureRecognized;
    }

    public void OnGestureRecognized(string gestureName)
    {
        if (listening&&gestureName == currentTask.gesture)
        {
            StartCoroutine(AdvanceTutorial());
        }
    }

}
