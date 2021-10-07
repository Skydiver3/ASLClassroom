using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pose Data", menuName = "ScriptableObjects/PoseData")]
public class PoseData : ScriptableObject
{
    [SerializeField]
    public List<Pose> poses;

    public void Add(Pose newPose)
    {
        if(poses == null)
        {
            poses = new List<Pose>();
        }
        poses.Add(newPose);
    }
}
