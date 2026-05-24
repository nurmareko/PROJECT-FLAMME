using System.Collections.Generic;
using UnityEngine;

public class CombinationManager : MonoBehaviour
{
    public static CombinationManager Instance;
    private readonly HashSet<ElementType> active = new();

    void Awake()
    {
        Instance = this;
    }

    public void CardDetected(ElementType e)
    {
        active.Add(e); Evaluate();
    }
    public void CardLost(ElementType e)
    {
        active.Remove(e); Evaluate();
    }

    void Evaluate()
    {
        if (active.Count == 2)
        {
            var pair = new List<ElementType>(active);
            Debug.Log($"PAIR DETECTED: {pair[0]} + {pair[1]}");
        }
        else
        {
            Debug.Log($"Cards visible: {active.Count}");
        }
    }
}