using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureTaskRepeat : GestureRecognizer
{
    protected override void DisplayCheckList(string gestureName, string gestureDescription, string[] messages, KeyStates[] states, Sprite[] sprites)
    {
        ChecklistManager.Instance.UpdateOnlyExplicitItems(gestureName, gestureDescription, messages, states, sprites);
    }
}
