using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public AudioManager _audioManager;

    [SerializeField] private GameObject _settingsPausePanel;
    [SerializeField] private GameObject _mainPausePanel;
    [SerializeField] private GameObject _audioPanel;
    [SerializeField] private GameObject _graphicsPanel;
    [SerializeField] private GameObject _controlsPanel;

    private bool _paused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject audioManager = GameObject.FindGameObjectWithTag("Audio");

        if (audioManager != null)
        {
            _audioManager = audioManager.GetComponent<AudioManager>();
        }

        _audioManager.PlayMusic(_audioManager.backgroundInGame);

        Resume();
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
            InventoryControler controler = FindAnyObjectByType<InventoryControler>();
            controler.TryEquipInFirstFreeGearSlot();
            
        }
        else
        {
            _mainPausePanel.SetActive(false);
            _settingsPausePanel.SetActive(false);
            Time.timeScale = 1.0f;
            _paused = false;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        _paused = false;
    }
    public void Resume()
    {
        _mainPausePanel.SetActive(false);
        _settingsPausePanel.SetActive(false);
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
        _settingsPausePanel.SetActive(true);
        _mainPausePanel.SetActive(false);
    }
    public void CloseSettings()
    {
        _settingsPausePanel.SetActive(false);
        _mainPausePanel.SetActive(true);
    }

    public void GraphicsShow()
    {
        _graphicsPanel.SetActive(true);
        _audioPanel.SetActive(false);
        _controlsPanel.SetActive(false);
    }

    public void GraphicsClose()
    {
        _graphicsPanel.SetActive(false);
    }
    public void AudioShow()
    {
        _audioPanel.SetActive(true);
        _graphicsPanel.SetActive(false);
        _controlsPanel.SetActive(false);
    }
    public void AudioClose()
    {
        _audioPanel.SetActive(false);
    }
    public void ControlsShow()
    {
        _controlsPanel.SetActive(true);
        _audioPanel.SetActive(false);
        _graphicsPanel.SetActive(false);
    }

    public void ControlsClose()
    {
        _controlsPanel.SetActive(false);
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
