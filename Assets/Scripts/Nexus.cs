using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Nexus : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Healthbar UI")]
    public GameObject healthBarRoot;          // GameObject parent (background + fill)
    public Image healthFill;                  // Image en mode Fill
    public Vector3 worldOffset = new Vector3(0, 2f, 0);

    [Header("Animation Settings")]
    public float healthLerpSpeed = 4f;        // vitesse de l'animation de la barre

    private Camera cam;

    private float currentFill;                // valeur affichée
    private float targetFill;                 // valeur réelle à atteindre

    
    [Header("Scale Settings")]
    public float hoverScale = 1.2f;     // Taille au survol
    public float scaleSpeed = 10f;
    private Vector3 initialScale;
    private Vector3 targetScale;

    [Header("Damage Scale Animation")]
    [SerializeField] private float damageScaleMultiplier = 1.15f;
    [SerializeField] private float scaleAnimSpeed = 12f;

    [Header("DeathVFX")]
    [SerializeField] private GameObject deathVFX;
    private Vector3 baseScale;
    private Coroutine scaleRoutine;


    void Start()
    {
        cam = Camera.main;
        currentHealth = maxHealth;

        currentFill = 1f;
        targetFill = 1f;

        if (healthBarRoot != null)
        {
            baseScale = healthBarRoot.transform.localScale;
            UpdateHealthbarPosition();
        }
    }


    void Update()
    {
        if (healthBarRoot != null)
        {
            UpdateHealthbarPosition();
            FaceCamera();
        }

        AnimateHealthFill();
    }

    // --- DAMAGE ---
    public void NexusDamaged(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        targetFill = (float)currentHealth / maxHealth;

        // 🔥 Animation de scale
        PlayDamageScaleAnim();

        if (currentHealth <= 0)
            NexusDeath();
    }
    void PlayDamageScaleAnim()
    {
        if (healthBarRoot == null) return;

        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);

        scaleRoutine = StartCoroutine(DamageScaleRoutine());
    }

    IEnumerator DamageScaleRoutine()
    {
        Vector3 targetScale = baseScale * damageScaleMultiplier;

        float t = 0f;

        // Scale UP
        while (t < 1f)
        {
            t += Time.deltaTime * scaleAnimSpeed;
            healthBarRoot.transform.localScale = Vector3.Lerp(baseScale, targetScale, t);
            yield return null;
        }

        t = 0f;

        // Scale DOWN
        while (t < 1f)
        {
            t += Time.deltaTime * scaleAnimSpeed;
            healthBarRoot.transform.localScale = Vector3.Lerp(targetScale, baseScale, t);
            yield return null;
        }

        healthBarRoot.transform.localScale = baseScale;
        scaleRoutine = null;
    }


    // --- ANIMATED HEALTH FILL ---
    void AnimateHealthFill()
    {
        if (healthFill == null) return;

        // Lerp vers la nouvelle valeur
        currentFill = Mathf.Lerp(currentFill, targetFill, Time.deltaTime * healthLerpSpeed);

        healthFill.fillAmount = currentFill;
    }


    // --- POSITION + ROTATION ---
    void UpdateHealthbarPosition()
    {
        Vector3 worldPos = transform.position + worldOffset;
        healthBarRoot.transform.position = worldPos;
    }

    void FaceCamera()
    {
        healthBarRoot.transform.LookAt(cam.transform);
        healthBarRoot.transform.rotation = Quaternion.LookRotation(cam.transform.forward);
    }

    // --- DEATH ---
    void NexusDeath()
    {
        //GameObject vfx = Instantiate(deathVFX, transform);
        //Destroy(vfx,2);
    }
}
