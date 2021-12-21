using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideTask : Task
{
    [SerializeField] private GameObject _slidePrefab;
    [SerializeField] private Transform _displayParent;
    private GameObject _slideInstance;
    private ContextSlide _slide;

    private void OnEnable()
    {
        _slideInstance = Instantiate(_slidePrefab, _displayParent);
        _slide = _slideInstance.GetComponent<ContextSlide>();
        if (!_slide) Debug.LogError("[Slide Task] No ContextSlide script attached to slide");
        _slide.finishButton.onTriggerEnter.AddListener(CloseTask);
    }

    private void CloseTask()
    {
        onTaskSucceeded?.Invoke();
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _slide.finishButton.onTriggerEnter.RemoveListener(CloseTask);
        Destroy(_slideInstance);
    }
}
