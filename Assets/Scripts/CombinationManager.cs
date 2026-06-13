using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CombinationTable))]
public class CombinationManager : MonoBehaviour
{
    [Header("Info Popup")]
    public Button infoButton;
    public GameObject infoPopup;
    public TextMeshProUGUI popupTitle;
    public TextMeshProUGUI popupBody;
    public Button closeButton;

    private ReactionData currentReaction;
    [Header("UI")]
    public TextMeshProUGUI feedbackText;
    public static CombinationManager Instance;

    [Header("Reaction Lookup")]
    public CombinationTable combinationTable;

    private readonly Dictionary<ElementType, Transform> active = new();
    private GameObject currentResult;
    private int currentPairKey = -1;

    void Awake()
    {
        Instance = this;
        if (combinationTable == null) combinationTable = GetComponent<CombinationTable>();
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
        int pairKey = CombinationTable.PairKey(keys[0], keys[1]);
        if (pairKey == currentPairKey) return;

        ClearResult();
        currentPairKey = pairKey;

        var reaction = combinationTable != null ? combinationTable.Resolve(keys[0], keys[1]) : null;
        if (reaction == null)
        {
            currentReaction = null;
            if (infoButton != null) infoButton.gameObject.SetActive(false);
            string inertMessage = combinationTable != null
                ? combinationTable.GetInertMessage(keys[0], keys[1])
                : "Tidak ada perubahan wujud.";
            ShowFeedback(inertMessage);
            return;
        }

        currentReaction = reaction;
        if (infoButton != null) infoButton.gameObject.SetActive(true);

        Vector3 mid = Midpoint();
        if (reaction.effectPrefab != null)
            currentResult = Instantiate(reaction.effectPrefab, mid, Quaternion.identity);

        ShowFeedback(reaction.resultName);
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
        currentPairKey = -1;
        currentReaction = null;
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
        if (currentReaction == null || infoPopup == null) return;
        popupTitle.text = currentReaction.resultName;
        popupBody.text = BuildPopupBody(currentReaction);
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

    string BuildPopupBody(ReactionData reaction)
    {
        string body = reaction.explanation;

        if (!string.IsNullOrWhiteSpace(reaction.everydayExample))
            body += $"\n\nContoh: {reaction.everydayExample}";

        if (reaction.kalor != KalorDir.None)
            body += $"\nKalor: {reaction.kalor}";

        return body;
    }
}
