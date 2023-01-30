using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A variation of the Gesture Recognizer, but the Checklist display only shows feedback (right and wrong items), and not the instructions for completing the gesture (so no empty items).
/// </summary>
public class GestureTaskRepeat : GestureRecognizer
{
    protected override void DisplayCheckList(string gestureName, string gestureDescription, string[] messages, KeyStates[] states, Sprite[] sprites)
    {
        ChecklistManager.Instance.UpdateOnlyExplicitItems(gestureName, gestureDescription, messages, states, sprites);
    }
}
