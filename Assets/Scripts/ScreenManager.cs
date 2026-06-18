using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ScreenManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject aboutPanel;
    public GameObject scannerHUD;
    [SerializeField] private CombinationManager combinationManager;
    [SerializeField] private float buttonAnimationDelay = 0.45f;

    private Coroutine screenChangeCoroutine;

    void Start() => ChangeScreen(mainMenuPanel);

    public void ShowMainMenu()
    {
        Debug.Log("game started");
        StartDelayedScreenChange(mainMenuPanel);
    }

    public void ShowScanner()
    {
        StartDelayedScreenChange(scannerHUD);
    }

    public void ShowAbout()
    {
        Debug.Log("about button clicked");
        StartDelayedScreenChange(aboutPanel);
    }

    private void StartDelayedScreenChange(GameObject targetPanel)
    {
        PlaySelectedButtonPressAnimation();

        if (screenChangeCoroutine != null)
        {
            StopCoroutine(screenChangeCoroutine);
        }

        screenChangeCoroutine = StartCoroutine(ChangeScreenAfterDelay(targetPanel));
    }

    private void PlaySelectedButtonPressAnimation()
    {
        GameObject selectedButton = EventSystem.current != null
            ? EventSystem.current.currentSelectedGameObject
            : null;

        if (selectedButton == null || !selectedButton.TryGetComponent(out Animator animator))
        {
            return;
        }

        animator.ResetTrigger("Normal");
        animator.ResetTrigger("Highlighted");
        animator.ResetTrigger("Selected");
        animator.SetTrigger("Pressed");
    }

    private IEnumerator ChangeScreenAfterDelay(GameObject targetPanel)
    {
        yield return new WaitForSeconds(buttonAnimationDelay);
        ChangeScreen(targetPanel);
        screenChangeCoroutine = null;
    }

    private void ChangeScreen(GameObject targetPanel)
    {
        mainMenuPanel.SetActive(targetPanel == mainMenuPanel);
        aboutPanel.SetActive(targetPanel == aboutPanel);
        scannerHUD.SetActive(targetPanel == scannerHUD);

        if (combinationManager != null)
            combinationManager.enabled = (targetPanel == scannerHUD);
    }
}
