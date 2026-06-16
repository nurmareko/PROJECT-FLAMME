using UnityEngine;

public enum KalorDirection { None, Menerima, Melepas }

[System.Serializable]
public class Combination
{
    [Header("Card Pair")]
    public ElementType elementA;
    public ElementType elementB;

    [Header("Display")]
    public string perubahanName;   // For reactions: "Mencair". For inert: "Berenang Bersama"
    public KalorDirection kalor = KalorDirection.None;  // None for inert pairs
    public GameObject resultPrefab; // null = inert pair (no AR effect)

    [Header("Info Popup")]
    [TextArea(3, 6)] public string explanation;  // Science explanation OR "Cerita Serunya"

    public bool IsReaction => resultPrefab != null;

    public bool Matches(ElementType a, ElementType b)
        => (elementA == a && elementB == b) || (elementA == b && elementB == a);
}