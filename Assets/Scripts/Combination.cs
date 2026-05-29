using UnityEngine;

[System.Serializable]
public class Combination
{
    public ElementType elementA;
    public ElementType elementB;
    public string resultName;
    [TextArea(2, 4)] public string explanation;
    public GameObject resultPrefab;

    public bool Matches(ElementType a, ElementType b)
        => (elementA == a && elementB == b) || (elementA == b && elementB == a);
}