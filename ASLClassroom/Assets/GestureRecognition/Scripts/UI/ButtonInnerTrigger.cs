using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInnerTrigger : MonoBehaviour
{
    public TouchButton parentButton;
    private void OnTriggerEnter(Collider other)
    {
        if (!parentButton.interactive) return;
        parentButton.onTriggerEnter?.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        if (!parentButton.interactive) return;
        parentButton.onTriggerExit?.Invoke();
    }
}
