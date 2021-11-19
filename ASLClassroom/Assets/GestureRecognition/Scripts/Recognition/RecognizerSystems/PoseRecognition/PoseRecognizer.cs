using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static OVRPlugin;

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
    public OVRCustomSkeleton skeletonL;
    public OVRCustomSkeleton skeletonR;
    public bool debugMode = true;
    public PoseData poseLibraryL;
    public PoseData poseLibraryR;
    public float threshold = 0.05f;
    private List<OVRBone> fingerBonesL;
    private List<OVRBone> fingerBonesR;
    private Pose previousPoseL;
    private Pose previousPoseR;

    public TextMeshProUGUI poseTextL;
    public TextMeshProUGUI poseTextR;

    public delegate void StringHandDelegate(string s, SkeletonType handType);
    public StringHandDelegate PoseRecognizedEvent;

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
        previousPoseL = new Pose();
        previousPoseR = new Pose();
    }

    private void Update()
    {
        if (debugMode && Input.GetKeyDown(KeyCode.Space))
        {
            Save(skeletonL, fingerBonesL, poseLibraryL);
            Save(skeletonR, fingerBonesR, poseLibraryR);
        }

        //Left hand
        Pose currentPose = Recognize(skeletonL, fingerBonesL, poseLibraryL);
        bool hasRecognized = !currentPose.Equals(new Pose());
        //check if new Pose
        poseTextL.text = currentPose.name;
        if (hasRecognized && !currentPose.Equals(previousPoseL))
        {
            currentPose.onRecognized?.Invoke();
            PoseRecognizedEvent?.Invoke(currentPose.name, SkeletonType.HandLeft);
            previousPoseL = currentPose;
        }

        //Right hand
        currentPose = Recognize(skeletonR, fingerBonesR, poseLibraryR);
        hasRecognized = !currentPose.Equals(new Pose());
        //check if new Pose
        poseTextR.text = currentPose.name;
        if (hasRecognized && !currentPose.Equals(previousPoseR))
        {
            currentPose.onRecognized?.Invoke();
            PoseRecognizedEvent?.Invoke(currentPose.name, SkeletonType.HandRight);
            previousPoseR = currentPose;
        }
    }

    private void Save(OVRCustomSkeleton skeleton, List<OVRBone> fingerBones, PoseData poseLibrary)
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
    public void SaveLeft()
    {
        Save(skeletonL, fingerBonesL, poseLibraryL);
    }
    public void SaveRight()
    {
        Save(skeletonR, fingerBonesR, poseLibraryR);
    }

    public Pose Recognize(SkeletonType skeletonType)
    {
        if (skeletonType == SkeletonType.HandLeft)
        {
            return Recognize(skeletonL, fingerBonesL, poseLibraryL);
        }
        else
        {
            return Recognize(skeletonR, fingerBonesR, poseLibraryR);
        }
    }

    private Pose Recognize(OVRCustomSkeleton skeleton, List<OVRBone> fingerBones, PoseData poseLibrary)
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
