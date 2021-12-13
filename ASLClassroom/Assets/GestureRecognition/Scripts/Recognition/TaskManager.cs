using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TaskManager : MonoBehaviour
{
    private Task[] _tasks;
    private int _progress = 0;
    private Task _currentTask;
    public float delayBetweenTasks = 1.0f;
    public UnityEvent OnInit;
    public UnityEvent OnFinish;
    public UnityEvent OnTaskSuccessEvent;
    public UnityEvent OnTaskFailEvent;

    private void OnEnable()
    {
        StartCoroutine(InitTaskList());
        OnInit?.Invoke();
    }

    private void OnDisable()
    {
        OnFinish?.Invoke();
        UnsubscribeFromTask(_currentTask);
    }

    private void AdvanceTask()
    {
        UnsubscribeFromTask(_currentTask);
        OnTaskSuccessEvent?.Invoke();
        StartCoroutine(StartNextTaskAfterSeconds(delayBetweenTasks));
    }
    private IEnumerator StartNextTaskAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        //subscribe to next task
        if (_progress != _tasks.Length - 1)
        {
            _progress++;
            _currentTask = _tasks[_progress];
            SubscribeToTask(_currentTask);
        }
        //quest ends here, back to quest selection
        else
        {
            //TODO: exit quest
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator InitTaskList()
    {
        //wait until recognizers have been initialized
        while (OrientationRecognizer.Instance==null)
        {
            yield return null;
        }

        //register and start tasks
        _tasks = GetComponentsInChildren<Task>(true);
        if (_tasks.Length != 0)
        {
            _currentTask = _tasks[0];
            SubscribeToTask(_currentTask);
        }
    }

    private void FailTask()
    {
        OnTaskFailEvent?.Invoke();
    }

    private void SubscribeToTask(Task task)
    {
        task.gameObject.SetActive(true);
        task.onTaskSucceeded.AddListener(AdvanceTask);
        task.onTaskFailed.AddListener(FailTask);
    }
    private void UnsubscribeFromTask(Task task)
    {
        task.onTaskSucceeded.RemoveListener(AdvanceTask);
        task.onTaskFailed.RemoveListener(FailTask);
        task.gameObject.SetActive(false);

    }

}
