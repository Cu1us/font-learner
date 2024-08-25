using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
[CreateAssetMenu(fileName = "Learner Pack", menuName = "ScriptableObjects/Learner Pack")]
public class LearnerPack : ScriptableObject
{
    public TMP_FontAsset Font;
    public char[] CharacterList;
    public int FontOffsetX;
}