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
        bool present = status.Status == Status.TRACKED;
        if (present) CombinationManager.Instance.CardDetected(element, transform);
        else         CombinationManager.Instance.CardLost(element);
    }
}