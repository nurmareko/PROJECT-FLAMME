using UnityEngine;
using Vuforia;

public class ElementCard : MonoBehaviour
{
    public ElementType element;

    void Start()
    {
        var observer = GetComponent<ObserverBehaviour>();
        if (observer != null)
            observer.OnTargetStatusChanged += OnStatusChanged;
    }

    void OnStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        bool tracked = status.Status == Status.TRACKED
                    || status.Status == Status.EXTENDED_TRACKED;
        if (tracked) CombinationManager.Instance.CardDetected(element);
        else         CombinationManager.Instance.CardLost(element);
    }
}