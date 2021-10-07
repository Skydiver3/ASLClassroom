using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class HierarchyRecorder : MonoBehaviour
{
    // The clip the recording is going to be saved to.
    public AnimationClip clip;

    // Checkbox to start/stop the recording.
    public bool record = false;

    // The main feature: the actual recorder.
    private GameObjectRecorder m_Recorder;

    void Start()
    {
        // Create the GameObjectRecorder.
        m_Recorder = new GameObjectRecorder(this.gameObject);

        // Bind all the Transforms on the GameObject and all its children.
        m_Recorder.BindComponentsOfType<Transform>(gameObject, true);
    }

    // The recording needs to be done in LateUpdate in order
    // to be done once everything has been updated
    // (animations, physics, scripts, etc.).
    void LateUpdate()
    {
        if (clip == null)
            return;

        if (record)
        {
            Debug.Log("recording...");
            // As long as "record" is on: take a snapshot.
            m_Recorder.TakeSnapshot(Time.deltaTime);
        }
        else if (m_Recorder.isRecording)
        {
            Debug.Log("----------------------------------------save");
            // "record" is off, but we were recording:
            // save to clip and clear recording.
            m_Recorder.SaveToClip(clip);
            m_Recorder.ResetRecording();
        }
    }
}