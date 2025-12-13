using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public enum EnemyType
{
    Flying,
    Rapid,
    Normal,
    Heavy,
    Global,

}
public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    
    [Header ("Movement")]
    
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] public float timeToSpawn;

    public int currentWaypointIndex = 0;


    public Transform[] WayPoints;
    public Transform currentWayPoint;

    public Action<Enemy> OnDead;

    [Header("Economy")]
    public ItemSO deathScrap;
    public float nbreMana = 20;
    [SerializeField] private LayerMask layers;

    [Header("Explosion VFX")]
    public GameObject explosionVFXPrefab;

    [Header("Damage")]
    
    [SerializeField] public int lifePoints = 50;
    private float currentHealth;
    [SerializeField] public int nexusDamage = 10;
    [SerializeField] private float timeToExplode = 2;
    public GameObject damageTextPrefab;
    public bool canDamageNexus = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = lifePoints; 
        currentWayPoint = WayPoints[0];
    }

    // Update is called once per frame
    void Update()
    {   //lifePoints check
        if (lifePoints <= 0)
        {
            Destroy(gameObject);
        }

        // Direction system and waypoints
        float distToCurrentWaypoint = Vector3.Distance(transform.position, currentWayPoint.position);

        if (distToCurrentWaypoint < 0.1f && currentWaypointIndex < WayPoints.Length - 1)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex>= WayPoints.Length)
            {
                currentWaypointIndex = 0;
            }
            currentWayPoint = WayPoints[currentWaypointIndex];
        }
        else if (distToCurrentWaypoint < 0.1f && currentWaypointIndex == WayPoints.Length - 1)
        {
            StartCoroutine (StopAndApplyDamageToBase());
        }

        MoveTowards(currentWayPoint);

        

    }

    void MoveTowards(Transform target)
    {
        //Calculate direction from this object to the target
        Vector3 direction = (target.position - transform.position).normalized;

        // Create a rotation
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Gradually rotate
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed*Time.deltaTime);

        // Move the enemy towards the target
        transform.position += direction * speed * Time.deltaTime;

    }

    private IEnumerator StopAndApplyDamageToBase()
    {   
        yield return new WaitForSeconds(timeToExplode);
        canDamageNexus = true;
        nbreMana = 0;
        Destroy(gameObject);

    
    }

    public void TakeDamage(float amount, bool isCrit = false)
    {
        currentHealth -= amount;
        ShowDamage(amount, isCrit);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameObject deathVFX = Instantiate(explosionVFXPrefab, transform.position, Quaternion.identity);
        Destroy(deathVFX, 3f);
        ResourceManager.Instance.AddMana((int)nbreMana);
        ResourceManager.Instance.AddResource(deathScrap, 1);

        if (deathScrap != null)
            FlyingTextManager.Instance.SpawnScrap(transform.position, deathScrap);

        Destroy(gameObject);
    }

    public Color DamageToColor(float dmg, float maxDmg)
    {
        float t = Mathf.Clamp01(dmg / maxDmg);

        // Blanc → Jaune → Orange → Rouge
        if (t < 0.33f) return Color.Lerp(Color.white, Color.yellow, t * 3);
        if (t < 0.66f) return Color.Lerp(Color.yellow, new Color(1f, 0.5f, 0f), (t - 0.33f) * 3);
        return Color.Lerp(new Color(1f, 0.5f, 0f), Color.red, (t - 0.66f) * 3);
    }

    public void ShowDamage(float amount, bool isCrit = false)
    {
        float maxDamage = 50f;

        Color dmgColor;
        float scale;

        if (isCrit)
        {
            // 💥 CRIT STYLE — Violet + Plus Gros
            dmgColor = new Color(0.8f, 0.2f, 1f); // Violet flashy
            scale = 2.2f; // Toujours plus gros qu’un non-crit
        }
        else
        {
            // 🎨 Couleur dynamique normale
            dmgColor = DamageToColor(amount, maxDamage);

            float minScale = 1f;
            float maxScale = 2f;

            float t = Mathf.Clamp01(amount / maxDamage);
            scale = Mathf.Lerp(minScale, maxScale, t);
        }

        FlyingTextManager.Instance.SpawnText(
            transform.position,
            amount.ToString(),
            dmgColor,
            scale
        );
    }




    private void OnDestroy()
    {
        // ⚠ Empêche les erreurs si OnDead n’a pas d'abonné
        OnDead?.Invoke(this);
        

        // ✔ Spawn scrap si défini
        
    }

}
