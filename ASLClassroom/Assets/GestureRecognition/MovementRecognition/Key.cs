
using UnityEngine;
using static OVRPlugin;

public enum KeyStates
{
    Hit,
    Fail,
    None
}
public abstract class Key : ScriptableObject
{
    public abstract void InitKey();
    public abstract void ExitKey();
    public abstract KeyStates GetKeyMet();
    //pose
    //orientation
    //wriggle, shake, turn
    //close to other body part
    //wriggle, shake
}

