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
    public AudioSource menuMusic;
    public AudioSource winMusic;
    public AudioSource loseMusic;

    private void Start()
    {
        if (GameManager.instance.currentLevelId == 0)
        {
            ShowTutorialCanvas();
        }

        GameManager.OnWin += ShowWinCanvas;
        GameManager.OnLose += ShowLoseCanvas;
    }

    private void Update()
    {
        if (mainMenuCanvas.activeSelf && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)))
        {
            OnPlayClicked();
        }
        else if (tutorialCanvas.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            HideTutorialCanvas();
        }
        else if (GameManager.instance.isPlaying && Input.GetKeyDown(KeyCode.Escape))
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
        menuMusic.Play();
        GameManager.instance.LoadMainMenu();
    }

    public void OnPlayClicked()
    {
        menuMusic.Pause();
        GameManager.instance.currentLevelId = 0;
        GameManager.instance.LoadLevel(mainMusic);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void ShowTutorialCanvas()
    {
        mainMusic.Pause();
        Cursor.visible = true;
        Time.timeScale = 0;
        tutorialCanvas.gameObject.SetActive(true);
    }

    public void HideTutorialCanvas()
    {
        mainMusic.Play();
        Cursor.visible = false;
        Time.timeScale = 1;
        tutorialCanvas.SetActive(false);
    }

    public void Pause()
    {
        mainMusic.Pause();
        Cursor.visible = true;
        Time.timeScale = 0;
        pauseCanvas.SetActive(true);
    }

    public void Unpause()
    {
        mainMusic.Play();
        Cursor.visible = false;
        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
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
        loseMusic.Play();
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
        GameManager.instance.LoadLevel(mainMusic);
    }
    public void CreditsURL()
    {
        Application.OpenURL("https://makakaua.itch.io/harvest-hoppers");
    }
}
