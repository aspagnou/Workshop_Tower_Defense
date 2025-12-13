using UnityEngine;
using TMPro;
using System.Collections;

public class StatChangeFeedback : MonoBehaviour
{
    private TMP_Text txt;
    private Coroutine currentRoutine;

    [Header("Settings")]
    public float duration = 1.2f;
    

    private void Awake()
    {
        txt = GetComponent<TMP_Text>();
        txt.enabled = false; // pas de SetActive !
    }

    public void ShowChange(float delta)
    {
        if (this.gameObject.activeInHierarchy == false)
            return;
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(Animate(delta));
    }

    private IEnumerator Animate(float delta)
    {
        txt.enabled = true;

        if (delta > 0)
        {
            txt.text = $"+{delta:F1}";  // <- 1 chiffre après la virgule
            txt.color = Color.green;
        }
        else if (delta < 0)
        {
            txt.text = $"{delta:F1}";   // <- 1 chiffre après la virgule
            txt.color = Color.red;
        }
        else
        {
            txt.enabled = false;
            yield break;
        }

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            Color c = txt.color;
            c.a = 1f - t;
            txt.color = c;

            yield return null;
        }

        txt.enabled = false;
    }

}
