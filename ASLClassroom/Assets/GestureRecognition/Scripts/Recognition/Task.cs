using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Task : MonoBehaviour
{
    public UnityEvent onTaskSucceeded;
    public UnityEvent onTaskAdvanced;
    public UnityEvent onTaskFailed;
}
