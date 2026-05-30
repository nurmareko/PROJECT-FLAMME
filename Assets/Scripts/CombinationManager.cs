using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombinationManager : MonoBehaviour
{
    [Header("Info Popup")]
    public Button infoButton;
    public GameObject infoPopup;
    public TextMeshProUGUI popupTitle;
    public TextMeshProUGUI popupBody;
    public Button closeButton;

    private Combination currentCombo;
    [Header("UI")]
    public TextMeshProUGUI feedbackText;
    public static CombinationManager Instance;

    [Header("Configure all valid combinations here")]
    public List<Combination> combinations = new();

    private readonly Dictionary<ElementType, Transform> active = new();
    private GameObject currentResult;
    private string currentPairKey = "";

    void Awake()
    {
        Instance = this;
        if (infoButton != null) infoButton.onClick.AddListener(OpenPopup);
        if (closeButton != null) closeButton.onClick.AddListener(ClosePopup);
        if (infoButton != null) infoButton.gameObject.SetActive(false);
        if (infoPopup != null) infoPopup.SetActive(false);
    }

    public void CardDetected(ElementType e, Transform t) { active[e] = t; Evaluate(); }
    public void CardLost(ElementType e) { if (active.Remove(e)) Evaluate(); }

    void Evaluate()
    {
        if (active.Count != 2) { ClearResult(); return; }

        var keys = new List<ElementType>(active.Keys);
        string pairKey = PairKey(keys[0], keys[1]);
        if (pairKey == currentPairKey) return;

        ClearResult();
        currentPairKey = pairKey;

        var combo = FindCombination(keys[0], keys[1]);
        // inside Evaluate(), after FindCombination:
        if (combo == null)
        {
            currentCombo = null;
            if (infoButton != null) infoButton.gameObject.SetActive(false);
            ShowFeedback("These elements don't react with each other.");
            return;
        }

        currentCombo = combo;
        if (infoButton != null) infoButton.gameObject.SetActive(true);

        Vector3 mid = Midpoint();
        if (combo.resultPrefab != null)
            currentResult = Instantiate(combo.resultPrefab, mid, Quaternion.identity);

        ShowFeedback(combo.resultName);
    }

    void ShowFeedback(string msg)
    {
        if (feedbackText == null) return;
        feedbackText.text = msg;
        feedbackText.gameObject.SetActive(true);
    }

    void HideFeedback()
    {
        if (feedbackText != null) feedbackText.gameObject.SetActive(false);
    }

    void ClearResult()
    {
        if (currentResult != null) Destroy(currentResult);
        currentResult = null;
        currentPairKey = "";
        currentCombo = null;
        HideFeedback();
        if (infoButton != null) infoButton.gameObject.SetActive(false);
        if (infoPopup != null) infoPopup.SetActive(false);
    }

    void Update()
    {
        // keep the result floating between the cards as they move/jitter
        if (currentResult != null && active.Count == 2)
            currentResult.transform.position = Midpoint();
    }


    void OpenPopup()
    {
        if (currentCombo == null || infoPopup == null) return;
        popupTitle.text = currentCombo.resultName;
        popupBody.text  = currentCombo.explanation;
        infoPopup.SetActive(true);
    }

    void ClosePopup()
    {
        if (infoPopup != null) infoPopup.SetActive(false);
    }

    Vector3 Midpoint()
    {
        var keys = new List<ElementType>(active.Keys);
        return (active[keys[0]].position + active[keys[1]].position) * 0.5f;
    }

    Combination FindCombination(ElementType a, ElementType b)
    {
        foreach (var c in combinations)
            if (c.Matches(a, b)) return c;
        return null;
    }

    string PairKey(ElementType a, ElementType b)
        => a < b ? $"{a}_{b}" : $"{b}_{a}";
}
