using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject _settingsPausePanel;
    [SerializeField] private GameObject _mainPausePanel;

    private bool _paused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        
    }
    public void Pause()
    {
        if (_paused == false)
        {
            _mainPausePanel.SetActive(true);
            Time.timeScale = 0.0f;
            _paused = true;
        }
        else
        {
            _mainPausePanel.SetActive(false);
            Time.timeScale = 1.0f;
            _paused = false;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void Resume()
    {
        _mainPausePanel.SetActive(false);
        Time.timeScale = 1.0f;
        _paused = false;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ShowSettings()
    {
        _settingsPausePanel.SetActive(true);
        _mainPausePanel.SetActive(false);
    }
    public void CloseSettings()
    {
        _settingsPausePanel.SetActive(false);
        _mainPausePanel.SetActive(true);
    }

    public void QuitToDesktop()
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication.ExitPlaymode();

#else
        
        Application.Quit();

#endif
    }
}
