using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    [SerializeField] private GameObject _settingsPausePanel;
    [SerializeField] private GameObject _mainPausePanel;
    [SerializeField] private GameObject _audioPanel;
    [SerializeField] private GameObject _graphicsPanel;
    [SerializeField] private GameObject _controlsPanel;
    private bool _paused = false;

    [Header("PressAny")]
    [SerializeField] private TMP_Text textPressAny;
    public CanvasGroup cgPressAny;
    public CanvasGroup panelStart;
    [SerializeField] private Image KeyArtBackground;
    [SerializeField] private Image KeyArtPart1;
    [SerializeField] private Image KeyArtPart2;

    public float fadeDuration = 0.5f;
    public float visibleTime = 1.5f;
    public float hiddenTime = 1.5f;

    public float fadeInStartDuration = 1f;


    public Animator animator;

    private bool inputTriggered = false;
    private Coroutine fadeLoopRoutine;

    private void OnEnable()
    {
        //StopAllCoroutines();


        cgPressAny.alpha = 0f;
        textPressAny.gameObject.SetActive(true);

        animator.Rebind();
        animator.Update(0f);


        inputTriggered = false;

        if (fadeLoopRoutine != null)
        {
            StopCoroutine(fadeLoopRoutine);
        }

        fadeLoopRoutine = StartCoroutine(FadeLoop());
        
    }
    private void OnDisable()
    {
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!inputTriggered && Input.anyKeyDown)
        {
            HidePressAny();
            _mainPausePanel.SetActive(true);
            CloseSettings();
            //animator.Play("Idle", -1, 0f);
            animator.SetTrigger("PlayMenuAnim");
            //StartCoroutine(FadeInStartPanel(1f, fadeInStartDuration));
            inputTriggered = true;
        }

    }

    private IEnumerator EnableInputNextFrame()
    {
        yield return null;
        inputTriggered = false;
    }

    //private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
    //{
    //    if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
    //        return;

    //    if (device is Gamepad || device is Keyboard || device is Mouse)
    //    {
    //        HidePressAny();
    //        _mainPausePanel.SetActive(true);

    //    }

    //}

    private IEnumerator FadeLoop()
    {
        while (true)
        {
            yield return StartCoroutine(FadeText(1f));

            yield return new WaitForSeconds(visibleTime);

            yield return StartCoroutine(FadeText(0f));

            yield return new WaitForSeconds(hiddenTime);
        }
    }

    private IEnumerator FadeText(float targetAlpha)
    {
        float t = 0f;
        float start = cgPressAny.alpha;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            cgPressAny.alpha = Mathf.Lerp(start, targetAlpha, t);
            yield return null;
        }

        cgPressAny.alpha = targetAlpha;
    }

    private IEnumerator FadeInStartPanel(float targetAlpha, float fadeTime)
    {
        float t = 0f;
        float start = panelStart.alpha;
        yield return new WaitForSeconds (fadeTime);
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            panelStart.alpha = Mathf.Lerp(start, targetAlpha, t);
            yield return null;
        }

        panelStart.alpha = targetAlpha;
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

    public void HidePressAny()
    {
        if (fadeLoopRoutine != null)
        {
            StopCoroutine(fadeLoopRoutine);
        }
        StartCoroutine(FadeText(0f));
    }
    public void StartGame()
    {
        animator.SetTrigger("ReturnIdle");
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

    public void OpenZoo()
    {
        animator.SetTrigger("ReturnIdle");
        SceneManager.LoadScene(2);
    }

    public void LaunchCredit()
    {
        Debug.Log("Credit");
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
