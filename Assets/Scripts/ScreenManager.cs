using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject aboutPanel;
    public GameObject scannerHUD;

    void Start() => ShowMainMenu();

    public void ShowMainMenu()
    {
        Debug.Log("game started");
        mainMenuPanel.SetActive(true);
        aboutPanel.SetActive(false);
        scannerHUD.SetActive(false);
    }

    public void ShowScanner()
    {
        mainMenuPanel.SetActive(false);
        aboutPanel.SetActive(false);
        scannerHUD.SetActive(true);
    }

    public void ShowAbout()
    {
        Debug.Log("about button clicked");
        mainMenuPanel.SetActive(false);
        aboutPanel.SetActive(true);
        scannerHUD.SetActive(false);
    }
}
