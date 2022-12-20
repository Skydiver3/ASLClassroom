using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public enum HandTypes { DominantHand, NonDominantHand, LeftHand, RightHand };
    public enum BodyParts { HandDominant, HandNonDominant, Head, Forehead, Nose, EarDominant, EarNonDominant, Mouth, Chin, Chest, IndexTipDominant, IndexTipNonDominant, ThumbTipDominant, ThumbTipNonDominant, PalmDominant, PalmNonDominant, HandEdgeDominant, HandEdgeNonDominant, Count }

    public enum DirectionHandTypes { LeftHand, RightHand};
    public DirectionHandTypes dominantHand;

}
