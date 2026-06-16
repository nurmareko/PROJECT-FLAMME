using UnityEngine;

public enum KalorDirection { None, Menerima, Melepas }

[System.Serializable]
public class Combination
{
    [Header("Card Pair")]
    public ElementType elementA;
    public ElementType elementB;

    [Header("Reaction (leave prefab empty for inert pairs)")]
    public string perubahanName;          // "Mencair", "Menguap", etc.
    public KalorDirection kalor = KalorDirection.None;
    public GameObject resultPrefab;       // null = inert pair

    [Header("Info / Feedback Text")]
    [TextArea(3, 6)] public string explanation;   // for Info popup
    [TextArea(2, 4)] public string inertMessage;  // for inert pairs only

    public bool IsReaction => resultPrefab != null;

    public bool Matches(ElementType a, ElementType b)
        => (elementA == a && elementB == b) || (elementA == b && elementB == a);
}