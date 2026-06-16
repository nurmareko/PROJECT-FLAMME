using UnityEngine;

public enum KalorDirection { None, Menerima, Melepas }

[System.Serializable]
public class Combination
{
    [Header("Card Pair")]
    public ElementType elementA;
    public ElementType elementB;

    [Header("Display")]
    public string perubahanName;
    public KalorDirection kalor = KalorDirection.None;
    public GameObject resultPrefab;

    [Header("Info Popup")]
    [TextArea(3, 6)] public string explanation;

    public bool IsReaction => resultPrefab != null;

    public bool Matches(ElementType a, ElementType b)
        => (elementA == a && elementB == b) || (elementA == b && elementB == a);
}