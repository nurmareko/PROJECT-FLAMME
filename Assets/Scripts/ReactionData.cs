using UnityEngine;

[CreateAssetMenu(menuName = "Fullcard/Reaction")]
public class ReactionData : ScriptableObject
{
    public ElementType cardA;
    public ElementType cardB;
    public Perubahan perubahan;
    public KalorDir kalor;
    public GameObject effectPrefab;
    public string resultName;
    [TextArea(3, 6)] public string explanation;
    public string everydayExample;
}
