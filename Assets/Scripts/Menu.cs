using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public static Menu instance;

    public GameObject EnterNamePanel;
    public GameObject HighScoresPanel;
    public GameObject MainPanel;
    public GameObject StoryPanel;

    private GameObject? currentPanel = null;
    private float speed = 1;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        HidePanels();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (currentPanel && currentPanel == MainPanel)
            {
                Close();
            } else if (currentPanel && currentPanel == EnterNamePanel)
            {
                NewGame();
            } else
            {
                SetMainPanel();
            }
        }
    }

    public void Close()
    {
        HidePanels();

        currentPanel = null;
        Time.timeScale = speed;
    }

    public void HidePanels()
    {
        EnterNamePanel.SetActive(false);
        HighScoresPanel.SetActive(false);
        MainPanel.SetActive(false);
        StoryPanel.SetActive(false);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = speed;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SetEnterNamePanel()
    {
        SwitchPanel(EnterNamePanel, MainPanel);
    }

    public void SetHighScoresPanel()
    {
        SwitchPanel(HighScoresPanel, MainPanel);
    }

    public void SetMainPanel()
    {
        SwitchPanel(MainPanel, null);
    }

    public void SetStoryPanel()
    {
        SwitchPanel(StoryPanel, MainPanel);
    }

    public void SwitchPanel(GameObject newPanel, GameObject? backPanel)
    {
        HidePanels();
        newPanel.SetActive(true);
        
        currentPanel = newPanel;
        
        Time.timeScale = 0;
    }
}
