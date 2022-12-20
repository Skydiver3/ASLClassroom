using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static PlayerData;

public class PlayerObjectReferences : MonoBehaviour
{
    private static PlayerObjectReferences _instance;
    public static PlayerObjectReferences Instance { get { return _instance; } }

    [SerializeField] private PlayerData _playerData;
    private int domIndex; //left=0, right=1
    private int nonDomIndex; //left=0, right=1


    public Transform head;
    public Transform forehead;
    public Transform nose;
    public Transform mouth;
    public Transform chin;
    public Transform chest;
    public Transform[] hand = new Transform[2];
    public Transform[] ear = new Transform[2];
    public Transform[] indexTip = new Transform[2];
    public Transform[] thumbTip = new Transform[2];
    public Transform[] palm = new Transform[2];
    public Transform[] handEdge = new Transform[2];

    public Transform centerEye;

    private void Awake()
    {
        if (_instance) Destroy(gameObject);
        else _instance = this;

        domIndex = (int)_playerData.dominantHand;
        nonDomIndex = (domIndex == 0) ? 1 : 0;
    }

    //returns 0 or 1
    public int GetIndexByHandType(HandTypes type)
    {
        int t = (int)type;
        domIndex = (int)_playerData.dominantHand;
        nonDomIndex = (domIndex == 0) ? 1 : 0;

        int i;
        switch (t)
        {
            case 0:
                i = domIndex;
                break;
            case 1:
                i = nonDomIndex;
                break;
            default:
                i = t - 2;
                break;
        }
        return i;
    }

    public Transform GetObject(BodyParts type)
    {
        Transform m_object = null;
        switch (type)
        {
            case BodyParts.HandDominant:
                m_object = GetHand(HandTypes.DominantHand);
                break;
            case BodyParts.HandNonDominant:
                m_object = GetHand(HandTypes.NonDominantHand);
                break;
            case BodyParts.Head:
                m_object = head;
                break;
            case BodyParts.Forehead:
                m_object = forehead;
                break;
            case BodyParts.Nose:
                m_object = nose;
                break;
            case BodyParts.EarDominant:
                m_object = GetEar(HandTypes.DominantHand);
                break;
            case BodyParts.EarNonDominant:
                m_object = GetEar(HandTypes.NonDominantHand);
                break;
            case BodyParts.Mouth:
                m_object = mouth;
                break;
            case BodyParts.Chin:
                m_object = chin;
                break;
            case BodyParts.Chest:
                m_object = chest;
                break;
            case BodyParts.IndexTipDominant:
                m_object = GetIndexTip(HandTypes.DominantHand);
                break;
            case BodyParts.IndexTipNonDominant:
                m_object = GetIndexTip(HandTypes.NonDominantHand);
                break;
            case BodyParts.ThumbTipDominant:
                m_object = GetThumbTip(HandTypes.DominantHand);
                break;
            case BodyParts.ThumbTipNonDominant:
                m_object = GetThumbTip(HandTypes.NonDominantHand);
                break;
            case BodyParts.PalmDominant:
                m_object = GetPalm(HandTypes.DominantHand);
                break;
            case BodyParts.PalmNonDominant:
                m_object = GetPalm(HandTypes.NonDominantHand);
                break;
            case BodyParts.HandEdgeDominant:
                m_object = GetHandEdge(HandTypes.DominantHand);
                break;
            case BodyParts.HandEdgeNonDominant:
                m_object = GetHandEdge(HandTypes.NonDominantHand);
                break;
            case BodyParts.Count:
                break;
            default:
                break;
        }
        return m_object;
    }

    public Transform GetHand(HandTypes type)
    {
        return hand[GetIndexByHandType(type)];
    }
    public Transform GetEar(HandTypes type)
    {
        return ear[GetIndexByHandType(type)];
    }
    public Transform GetIndexTip(HandTypes type)
    {
        return indexTip[GetIndexByHandType(type)];
    }
    public Transform GetThumbTip(HandTypes type)
    {
        return thumbTip[GetIndexByHandType(type)];
    }
    public Transform GetPalm(HandTypes type)
    {
        return palm[GetIndexByHandType(type)];
    }
    public Transform GetHandEdge(HandTypes type)
    {
        return handEdge[GetIndexByHandType(type)];
    }


    //[CustomEditor(typeof(PlayerObjectReferences))]
    //public class PlayerObjectReferencesEditor : Editor
    //{
    //    public override void OnInspectorGUI()
    //    {
    //        serializedObject.Update();
    //        var script = (PlayerObjectReferences)target;

    //        // TODO: replace get functions with list of property fields named after corresponding enums -> automatically extended if enum has new entry. what about center/single types like head?

    //        DrawDefaultInspector();

    //    }
    //}
}