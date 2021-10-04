using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Pose
{
    public string name;
    public List<Vector3> fingerData;
    public UnityEvent onRecognized;
}

public class PoseRecognizer : MonoBehaviour
{
    public OVRSkeleton skeleton;
    public List<Pose> poses;
    public bool debugMode = true;
    public PoseData poseLibrary;
    private List<OVRBone> fingerBones;


    private void Update()
    {
        if (debugMode && Input.GetKeyDown(KeyCode.Space))
        {
            Save();
        }
    }

    void Save()
    {
        if (fingerBones == null)
        {
            fingerBones = new List<OVRBone>(skeleton.Bones);
        }

        Pose p = new Pose();
        p.name = "New Pose";
        List<Vector3> data = new List<Vector3>();
        foreach (OVRBone bone in fingerBones)
        {
            Debug.Log("add bone");
            data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
        }
        p.fingerData = data;
        poses.Add(p);
        poseLibrary.Add(p);
    }
}
