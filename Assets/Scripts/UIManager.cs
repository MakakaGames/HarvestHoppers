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
    public AudioSource buttonClickAudio;
    public AudioSource buttonHighlightAudio;

    private AudioSource[] audioSources;

    private void Start()
    {
        if (GameManager.instance.currentLevelId == 0)
        {
            ShowTutorialCanvas();
        }

        audioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
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
        foreach (var audio in audioSources)
        {
            if (audio)
                audio.Pause();
        }
        menuMusic.Play();
        GameManager.instance.LoadMainMenu();
    }

    public void OnPlayClicked()
    {
        buttonClickAudio.Play();
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
        foreach (var audio in audioSources)
        {
            if (audio)
                audio.Pause();
        }
        winMusic.Play();
        winMusic.loop = false;
        Cursor.visible = true;
        winCanvas.SetActive(true);
    }

    public void ShowLoseCanvas()
    {
        foreach (var audio in audioSources)
        {
            if (audio)
                audio.Pause();
        }
        loseMusic.Play();
        loseMusic.loop = false;
        Cursor.visible = true;
        loseCanvas.SetActive(true);
    }

    public void PlayAudioOnClick()
    {
        buttonClickAudio.Play();
    }

    public void OnHighlight()
    {
        buttonHighlightAudio.Play();
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
