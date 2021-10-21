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
    private static PoseRecognizer _instance;
    public static PoseRecognizer Instance { get { return _instance; } }

    //public OVRSkeleton skeleton;
    public OVRCustomSkeleton skeleton;
    public bool debugMode = true;
    public PoseData poseLibrary;
    public float threshold = 0.05f;
    private List<OVRBone> fingerBones;
    private Pose previousPose;

    public delegate void StringDelegate(string s);
    public StringDelegate PoseRecognizedEvent;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    private void Start()
    {
        previousPose = new Pose();
    }

    private void Update()
    {
        if (debugMode && Input.GetKeyDown(KeyCode.Space))
        {
            Save();
        }

        Pose currentPose = Recognize();
        bool hasRecognized = !currentPose.Equals(new Pose());
        //check if new Pose
        if (hasRecognized && !currentPose.Equals(previousPose))
        {
            currentPose.onRecognized?.Invoke();
            PoseRecognizedEvent?.Invoke(currentPose.name);
            previousPose = currentPose;
        }
    }

    public void Save()
    {
        if (skeleton == null || skeleton.Bones == null||skeleton.Bones.Count==0) return;
        if (fingerBones == null)
        {
            fingerBones = new List<OVRBone>(skeleton.Bones);
        }

        Pose p = new Pose();
        p.name = "New Pose";
        List<Vector3> data = new List<Vector3>();
        foreach (OVRBone bone in fingerBones)
        {
            //position relative to hand
            data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
        }
        p.fingerData = data;
        poseLibrary.Add(p);
    }

    public Pose Recognize()
    {
        if (skeleton.Bones.Count==0)
        {
            Debug.Log("[PoseRecognizer] Fingers not found.");
            return new Pose();
        }
        if (fingerBones == null)
        {
            fingerBones = new List<OVRBone>(skeleton.Bones);
        }

        Pose currentPose = new Pose();
        float currentMin = Mathf.Infinity;

        //scan all poses for something that matches the current pose
        foreach (Pose pose in poseLibrary.poses)
        {
            float sumDistance = 0;
            bool isDiscarded = false;

            //calculate closest match for current pose
            for (int i = 0; i < fingerBones.Count; i++)
            {
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                float distance = Vector3.Distance(currentData, pose.fingerData[i]);
                if (distance > threshold)
                {
                    isDiscarded = true;
                    break;
                }
                sumDistance += distance;
            }

            //set as new recognized pose
            if (!isDiscarded && sumDistance < currentMin)
            {
                currentMin = sumDistance;
                currentPose = pose;
            }
        }
        return currentPose;
    }
}
