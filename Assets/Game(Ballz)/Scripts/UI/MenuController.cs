using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public enum Market { PlayStore, CafeBazaar, }
    public Market m_Market = Market.PlayStore;

    public GameObject m_MainMenuQuitPanel;
    public GameObject m_SettingsPanel;
    public GameObject m_PauseMenu;  // or backMenu (panel)

    private float m_timeScale;
    public GameObject panel_loading;

    void Start ()
    {
        m_SettingsPanel.SetActive(false);
    }

    void Update()
    {
        switch(GameManager_ballz.Instance.m_GameState)
        {
            case GameManager_ballz.GameState.MainMenu:
                if (Input.GetKeyUp(KeyCode.Escape) && !m_MainMenuQuitPanel.activeInHierarchy)
                    m_MainMenuQuitPanel.SetActive(true);
                else if (Input.GetKeyUp(KeyCode.Escape) && m_MainMenuQuitPanel.activeInHierarchy)
                    m_MainMenuQuitPanel.SetActive(false);
                break;

            case GameManager_ballz.GameState.Playable:
                if (Input.GetKeyUp(KeyCode.Escape) && !m_PauseMenu.activeInHierarchy)
                    ShowPauseMenu();
                else if (Input.GetKeyUp(KeyCode.Escape) && m_PauseMenu.activeInHierarchy)
                    HidePauseMenu();
                break;
        }
    }

    public void StartGame()
    {
        GameManager_ballz.Instance.m_GameState = GameManager_ballz.GameState.Playable;
    }

    public void ShowPauseMenu()
    {
        // 1 - stop the time scale
        m_timeScale = Time.timeScale;
        Time.timeScale = 0;

        // 2 - active m_PauseMenu
        m_PauseMenu.SetActive(true);
    }

    public void HidePauseMenu()
    {
        // 1 - relaunch the time scale
        Time.timeScale = m_timeScale;
        m_timeScale = 0;

        // 2 - deactive m_PauseMenu
        m_PauseMenu.SetActive(false);
    }

    public void ShowSettingsPanel()
    {
        // 1 - stop the time scale
        m_timeScale = Time.timeScale;
        Time.timeScale = 0;

        m_SettingsPanel.SetActive(true);
    }

    public void HideSettingsPanel()
    {
        // 1 - relaunch the time scale
        Time.timeScale = m_timeScale;
        m_timeScale = 0;

        m_SettingsPanel.SetActive(false);
    }

    #region GameOver Menu
    public void GotoMainMenuAfterGameOver()
    {
        GameManager_ballz.Instance.m_GameState = GameManager_ballz.GameState.MainMenu;
        Saver.Instance.Save(true);
    }

    public void ReplayAfterGameOver()
    {
        GameManager_ballz.Instance.m_GameState = GameManager_ballz.GameState.MainMenu;
        GameManager_ballz.Instance.m_GameState = GameManager_ballz.GameState.Playable;
        Saver.Instance.Save(true);
    }
    #endregion

    #region Pause Menu
    public void GotoMainMenu()
    {
        GameManager_ballz.Instance.m_GameState = GameManager_ballz.GameState.MainMenu;
        HidePauseMenu();
    }

    public void ResumeGame()
    {
        HidePauseMenu();
    }

    public void QuitGame()
    {
       panel_loading.SetActive(true); 
       SceneManager.LoadSceneAsync(0 , LoadSceneMode.Single); 
    }
    #endregion
}