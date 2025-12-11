using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Rendering;

public class ButtonDouble : MonoBehaviour
{

    public Image targetImage;
    public Sprite hoverImage;
    public Sprite nrmImage;
    public Sprite pressedImage;

    public GameObject leftImage;
    public RectTransform _leftImage;
    public GameObject rightImage;
    public RectTransform _rightImage;
    [SerializeField] private Vector2 leftHiddenPos;
    [SerializeField] private Vector2 leftVisiblePos;
    [SerializeField] private Vector2 rightHiddenPos;
    [SerializeField] private Vector2 rightVisiblePos;


    public TMP_Text buttonText;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color pressedColor = Color.gray;


    [Header("Animation")]
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private AnimationCurve curve =  AnimationCurve.EaseInOut(0, 0, 1, 1);
    private Coroutine animRoutine;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {
        buttonText.color = normalColor;
        _leftImage.anchoredPosition = leftHiddenPos;
        _rightImage.anchoredPosition = rightHiddenPos;
    }
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void OnHoverEnter()
    {
        leftImage.SetActive(true);
        rightImage.SetActive(true);
        targetImage.sprite = hoverImage;
        StartSlide(leftVisiblePos, rightVisiblePos, false);
    }

    public void OnHoverExit()
    {
        
        targetImage.sprite = nrmImage;
        buttonText.color = normalColor;
        StartSlide(leftHiddenPos, rightHiddenPos, true);
        

    }

    public void OnPressed()
    {
        targetImage.sprite = pressedImage;
        buttonText.color = pressedColor;

        
    }
    private void StartSlide(Vector2 leftTarget,  Vector2 rightTarget, bool hideAtEnd = false)
    {
        if (animRoutine != null)
        {
            StopCoroutine(animRoutine);
        }

        animRoutine = StartCoroutine(SlideImages(leftTarget, rightTarget, hideAtEnd));
    }

    private IEnumerator SlideImages(Vector2 leftTarget, Vector2 rightTarget, bool hideAtEnd)
    {
        float t = 0f;

        Vector2 leftStart = _leftImage.anchoredPosition;
        Vector2 rightStart = _rightImage.anchoredPosition;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duration;

            float eased = curve.Evaluate(t);

            _leftImage.anchoredPosition = Vector2.Lerp(leftStart, leftTarget, eased);
            _rightImage.anchoredPosition = Vector2.Lerp(rightStart, rightTarget, eased);

            yield return null;

        }

        if (hideAtEnd)
        {
            leftImage.SetActive(false);
            rightImage.SetActive(false);
        }
    }

    public void ResetState()
    {
        targetImage.sprite = nrmImage;
        buttonText.color = normalColor;
        leftImage.SetActive(false);
        rightImage.SetActive(false);
    }

   
   

}
