using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaveWarningFade : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float blinkSpeed = 2f;
    [SerializeField] private float maxAlpha = 0.6f;

    private Coroutine blinkRoutine;

    void Awake()
    {
        SetAlpha(0f);
    }

    public void StartBlink()
    {
        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        blinkRoutine = StartCoroutine(Blink());
    }

    public void StopBlink()
    {
        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        blinkRoutine = null;
        SetAlpha(0f);
    }

    IEnumerator Blink()
    {
        float t = 0f;

        while (true)
        {
            t += Time.deltaTime * blinkSpeed;
            float alpha = Mathf.Abs(Mathf.Sin(t)) * maxAlpha;
            SetAlpha(alpha);
            yield return null;
        }
    }

    void SetAlpha(float a)
    {
        Color c = fadeImage.color;
        c.a = a;
        fadeImage.color = c;
    }
}
