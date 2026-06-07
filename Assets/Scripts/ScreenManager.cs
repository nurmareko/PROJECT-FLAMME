using UnityEngine;
using System.Collections;

public class ScreenManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject aboutPanel;
    public GameObject scannerHUD;
    [SerializeField] private float buttonAnimationDelay = 0.33f;

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
        if (screenChangeCoroutine != null)
        {
            StopCoroutine(screenChangeCoroutine);
        }

        screenChangeCoroutine = StartCoroutine(ChangeScreenAfterDelay(targetPanel));
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
    }
}
