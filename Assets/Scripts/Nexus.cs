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

    void Start()
    {
        cam = Camera.main;
        currentHealth = maxHealth;

        currentFill = 1f;
        targetFill = 1f;

        if (healthBarRoot != null)
            UpdateHealthbarPosition();
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

        // Calcul fill
        targetFill = (float)currentHealth / maxHealth;

        if (currentHealth <= 0)
            NexusDeath();
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
        // Destroy(gameObject);
    }
}
