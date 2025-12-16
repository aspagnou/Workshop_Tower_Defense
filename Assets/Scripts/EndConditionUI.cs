using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class EndConditionUI : MonoBehaviour
{
    public AudioManager _audioManager;
    
    [SerializeField] private GameObject _mainWin;
    [SerializeField] private CanvasGroup _cgMainWin;
    [SerializeField] private GameObject _mainLoose;
    [SerializeField] private CanvasGroup _cgMainLoose;
    public float fadeDuration = 1f;
    private bool _paused = false;

    private Coroutine fadeCore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {
        ResetMainMenu();
    }

    void Start()
    {
        GameObject audioManager = GameObject.FindGameObjectWithTag("Audio");

        if (audioManager != null)
        {
            _audioManager = audioManager.GetComponent<AudioManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void Win()
    {
        PauseWin();
    }

    public void Loose()
    {
        PauseLoose();
    }
    public void PauseWin()
    {
        _mainWin.SetActive(true);

        StartCoroutine(FadeInUiWin());

        _audioManager.musicSource.Stop();
        _audioManager.PlayMusic(_audioManager.winJingle);

        if (_paused == false)
        {
            _mainWin.SetActive(true);
            Time.timeScale = 0.0f;
            _paused = true;
        }
        else
        {
            _mainWin.SetActive(false);
            Time.timeScale = 1.0f;
            _paused = false;
        }
    }
    public void PauseLoose()
    {
        _mainLoose.SetActive(true);

        StartCoroutine(FadeInUiLoose());

        _audioManager.musicSource.Stop();
        _audioManager.PlayMusic(_audioManager.loseJingle);

        if (_paused == false)
        {
            _mainLoose.SetActive(true);
            Time.timeScale = 0.0f;
            _paused = true;
        }
        else
        {
            _mainLoose.SetActive(false);
            Time.timeScale = 1.0f;
            _paused = false;
        }
    }

    private IEnumerator FadeInCg(CanvasGroup canvasGroup)
    {
        float t = 0f;
        float startAlpha = canvasGroup.alpha;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, t);
            yield return null;
        }
    }

    private IEnumerator FadeInUiWin()
    {
        yield return StartCoroutine(FadeInCg(_cgMainWin));
    }
    private IEnumerator FadeInUiLoose()
    {
        yield return StartCoroutine(FadeInCg(_cgMainLoose));
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        _paused = false;
    }
    public void Resume()
    {
        _mainWin.SetActive(false);
        Time.timeScale = 1.0f;
        _paused = false;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenZoo()
    {
        SceneManager.LoadScene(2);
    }

    public void ShowSettings()
    {
        _mainWin.SetActive(false);
    }
    public void CloseSettings()
    {
        _mainWin.SetActive(true);
    }
    private void ResetMainMenu()
    {
        Time.timeScale = 1f;

       
        _paused = false;

       

        _cgMainWin.alpha = 0f;
       _cgMainLoose.alpha = 0f;
    }
    //public void GraphicsShow()
    //{
    //    _graphicsPanel.SetActive(true);
    //    _audioPanel.SetActive(false);
    //    _controlsPanel.SetActive(false);
    //}

    //public void GraphicsClose()
    //{
    //    _graphicsPanel.SetActive(false);
    //}
    //public void AudioShow()
    //{
    //    _audioPanel.SetActive(true);
    //    _graphicsPanel.SetActive(false);
    //    _controlsPanel.SetActive(false);
    //}
    //public void AudioClose()
    //{
    //    _audioPanel.SetActive(false);
    //}
    //public void ControlsShow()
    //{
    //    _controlsPanel.SetActive(true);
    //    _audioPanel.SetActive(false);
    //    _graphicsPanel.SetActive(false);
    //}

    //public void ControlsClose()
    //{
    //    _controlsPanel.SetActive(false);
    //}

    public void QuitToDesktop()
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication.ExitPlaymode();

#else
        
        Application.Quit();

#endif
    }
}
