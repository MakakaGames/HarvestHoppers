using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject loseCanvas;
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject mainMenuCanvas;

    private AudioSource mainMusic;

    private void Start()
    {
        if(GameManager.instance.currentLevelId == 0)
        {
            ShowTutorialCanvas();
        }

        GameManager.OnWin += ShowWinCanvas;
        GameManager.OnLose += ShowLoseCanvas;
    }

    private void Update()
    {
        Debug.Log(GameManager.instance.isPlaying);

        if (GameManager.instance.isPlaying && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("kekw");
            if (pauseCanvas.activeSelf)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }

    public void LoadMainMenu()
    {
        GameManager.instance.LoadMainMenu();
    }

    public void OnPlayClicked()
    {
        GameManager.instance.currentLevelId = 0;
        GameManager.instance.LoadLevel();
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void ShowTutorialCanvas()
    {       
        Cursor.visible = true;
        Time.timeScale = 0;
        tutorialCanvas.gameObject.SetActive(true);
    }

    public void HideTutorialCanvas()
    {
        Cursor.visible = false;
        Time.timeScale = 1;
        tutorialCanvas.SetActive(false);
    }

    public void Pause()
    {
        Cursor.visible = true;
        Time.timeScale = 0;
        pauseCanvas.SetActive(true);
    }

    public void Unpause()
    {
        Cursor.visible = false;
        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
    }

    public void ShowWinCanvas()
    {
        Cursor.visible = true;
        winCanvas.SetActive(true);
    }

    public void ShowLoseCanvas()
    {
        Cursor.visible = true;
        loseCanvas.SetActive(true);
    }

    private void OnDestroy()
    {
        GameManager.OnWin -= ShowWinCanvas;
        GameManager.OnLose -= ShowLoseCanvas;
    }

    public void LoadNewLevel()
    {
        GameManager.instance.LoadLevel();
    }
    
    public void CreditsLink()
    {
         Application.OpenURL("https://globalgamejam.org/2023/games/harvest-hoppers-1");
    }
}
