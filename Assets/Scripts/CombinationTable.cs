using System.Collections.Generic;
using UnityEngine;

public class CombinationTable : MonoBehaviour
{
    private const string GenericInertMessage = "Tidak ada perubahan wujud.";

    public List<ReactionData> reactions = new();

    private readonly Dictionary<int, ReactionData> _map = new();
    private readonly Dictionary<int, string> _inertMessages = new();

    void Awake()
    {
        BuildLookup();
    }

    public static int PairKey(ElementType a, ElementType b)
    {
        int x = (int)a, y = (int)b;
        return Mathf.Min(x, y) * 10 + Mathf.Max(x, y);
    }

    public ReactionData Resolve(ElementType a, ElementType b)
        => _map.TryGetValue(PairKey(a, b), out var reaction) ? reaction : null;

    public string GetInertMessage(ElementType a, ElementType b)
        => _inertMessages.TryGetValue(PairKey(a, b), out var message)
            ? message
            : GenericInertMessage;

    private void BuildLookup()
    {
        _map.Clear();
        _inertMessages.Clear();

        foreach (var reaction in reactions)
        {
            if (reaction == null) continue;

            int key = PairKey(reaction.cardA, reaction.cardB);
            if (_map.ContainsKey(key))
            {
                Debug.LogWarning($"Duplicate reaction pair ignored: {reaction.cardA} + {reaction.cardB}", this);
                continue;
            }

            _map.Add(key, reaction);
        }

        AddInert(ElementType.Es, ElementType.Air, "Tidak ada perubahan wujud - tapi perhatikan, es mengapung di air!");
        AddInert(ElementType.Panas, ElementType.Dingin, "Kalor berpindah dari yang panas ke yang dingin sampai suhunya sama.");
        AddInert(ElementType.Panas, ElementType.Uap, "Uap makin panas, tapi wujudnya tetap gas.");
        AddInert(ElementType.Dingin, ElementType.Es, "Es tetap padat - sudah dingin.");
    }

    private void AddInert(ElementType a, ElementType b, string message)
    {
        _inertMessages[PairKey(a, b)] = message;
    }
}
