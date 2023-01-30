
using UnityEngine;
using UnityEngine.UI;
using static OVRPlugin;

public enum KeyStates
{
    Hit,
    Fail,
    None
}
/// <summary>
/// Atomic parts that gestures are made of. Describes one dimension of a Complex Pose, be it directional movement, hand shape, orientation, or relative position. 
/// </summary>
public abstract class Key : ScriptableObject
{
    public string description;
    public Sprite sprite;
    public abstract void InitKey();
    public abstract void ExitKey();
    public abstract KeyStates GetKeyMet();
    //pose
    //orientation
    //wriggle, shake, turn
    //close to other body part
    //wriggle, shake
}

