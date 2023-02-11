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

    public AudioSource mainMusic;
    public AudioSource winMusic;
    public AudioSource looseMusic;
    public AudioSource menuMusic;
   
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
        mainMusic.Pause();
    }

    public void OnPlayClicked()
    {
        GameManager.instance.currentLevelId = 0;
        GameManager.instance.LoadLevel();
        mainMusic.Pause();
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void ShowTutorialCanvas()
    {       
        Cursor.visible = true;
        mainMusic.Pause();
        Time.timeScale = 0;
        tutorialCanvas.gameObject.SetActive(true);
    }

    public void HideTutorialCanvas()
    {
        Cursor.visible = false;
        Time.timeScale = 1;
        tutorialCanvas.SetActive(false);
        mainMusic.Play();
    }

    public void Pause()
    {
        Cursor.visible = true;
        mainMusic.Pause();
        Time.timeScale = 0;
        pauseCanvas.SetActive(true);
    }

    public void Unpause()
    {
        Cursor.visible = false;
        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
        mainMusic.Play();
    }

    public void ShowWinCanvas()
    {
        mainMusic.Pause();
        winMusic.Play();
        Cursor.visible = true;
        winCanvas.SetActive(true);
    }

    public void ShowLoseCanvas()
    {
        mainMusic.Pause();
        looseMusic.Play();
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
        looseMusic.Pause();
        winMusic.Pause();
        GameManager.instance.LoadLevel();
        mainMusic.Play();
    }
    
    public void CreditsLink()
    {
         Application.OpenURL("https://makakaua.itch.io/harvest-hoppers");
    }
}
