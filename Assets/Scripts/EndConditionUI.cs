using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class EndConditionUI : MonoBehaviour
{

    
    [SerializeField] private GameObject _mainWin;
    [SerializeField] private GameObject _mainLoose;
    //[SerializeField] private GameObject _audioPanel;
    //[SerializeField] private GameObject _graphicsPanel;
    //[SerializeField] private GameObject _controlsPanel;

    private bool _paused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

    

    public void StartGame()
    {
        SceneManager.LoadScene("Basics");
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
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenZoo()
    {
        SceneManager.LoadScene("Zoo");
    }

    public void ShowSettings()
    {
        _mainWin.SetActive(false);
    }
    public void CloseSettings()
    {
        _mainWin.SetActive(true);
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
