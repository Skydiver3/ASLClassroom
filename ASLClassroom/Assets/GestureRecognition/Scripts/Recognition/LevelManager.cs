using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    //TODO: make this a branching thing
    private int _progress = 1;
    [SerializeField] private List<TouchButton> _levelButtons = new List<TouchButton>();
    public TaskManager[] _levels;

    [SerializeField] private GameObject levelSelectView;
    [SerializeField] private GameObject taskView;

    private void Start()
    {
        _levels = GetComponentsInChildren<TaskManager>(true);

        //connect buttons to levels
        for (int i = 0; i < _levels.Length; i++)
        {
            if (!_levels[i] || !_levelButtons[i]) 
            { 
                Debug.LogError("[Level Manager] Level or Button not assigned.");
                return;
            }

            int _i = i;
            _levelButtons[i].onTriggerEnter.AddListener(delegate { print(_i); StartLevel(_i); });
            _levels[i].OnClear.AddListener(delegate { _progress = _i + 2; });
            _levels[i].OnFinish.AddListener(delegate { ExitLevel(_levels[_i]); });            
        }

        //enable buttons based on progress
        UpdateButtonsByProgress();
    }


    private void UpdateButtonsByProgress()
    {
        foreach (TouchButton button in _levelButtons)
        {
            button.SetActive(false);
        }
        for (int i = 0; i < _levelButtons.Count; i++)
        {
            if (i < _progress) EnableButton(_levelButtons[i]);
            else DisableButton(_levelButtons[i]);
        }
    }

    private void EnableButton(TouchButton button)
    {
        button.SetActive(true);
    }
    private void DisableButton(TouchButton button)
    {
        button.SetActive(false);
    }
    private void StartLevel(int i)
    {
        print(i);
        StartLevel(_levels[i]);
    }
    private void StartLevel(TaskManager level)
    {
        levelSelectView.SetActive(false);
        taskView.SetActive(true);

        level.gameObject.SetActive(true);

        ChecklistManager.Instance.exitButton.onTriggerEnter.AddListener(ExitCurrentLevel);
    }
    private void ExitLevel(TaskManager level)
    {
        if (!level) return;
        ChecklistManager.Instance.exitButton.onTriggerEnter.RemoveListener(ExitCurrentLevel);

        if(levelSelectView)levelSelectView.SetActive(true);
        if(taskView)taskView.SetActive(false);

        UpdateButtonsByProgress();
    }
    private void ExitCurrentLevel()
    {
        ExitLevel(_levels[_progress]);
    }
}
