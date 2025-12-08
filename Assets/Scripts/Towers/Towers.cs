using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Towers : MonoBehaviour
{
    [SerializeField] private float range = 10f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float criticalRate = 10f;
    [SerializeField] private float criticalDamage = 2f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] public float damage = 10f;
    private float fireCoolDown = 0f;

    [Header("Multiple Projectile")]
    [SerializeField] private int nbProjectile = 1;
    [SerializeField] private float spreadAngle = 30f;
    
    

    [SerializeField] private Transform pivot;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    private Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FindTarget();

        if (target != null)
        {
            AimAtTarget();
            Shoot();  
        }

        
    }

    void FindTarget()
    {
        Enemy[] enemies = UnityEngine.Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        float shortestDistance = Mathf.Infinity;
        Enemy nearestEnemy = null;

        foreach (Enemy enemy in enemies)
        {
            if (enemy == null)
                continue;

            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance && distance <= range)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
            target = nearestEnemy.transform;
        else 
            target = null;
    }

    void AimAtTarget()
    {
        Vector3 direction = target.position - pivot.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        pivot.rotation = Quaternion.Lerp(pivot.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    void Shoot()
    {
        if (fireCoolDown <= 0f)
        {
            if (nbProjectile == 1) //tir simple
            {
                SpawnProjectile(firePoint.rotation);
            }
            else
            {
                float angleStep = spreadAngle / (nbProjectile - 1);
                float startAngle = -spreadAngle / 2f;

                for (int i = 0; i < nbProjectile; i++)
                {
                    float angle = startAngle + angleStep * i;
                    Quaternion projectileRotation = firePoint.rotation * Quaternion.Euler(0, angle, 0);
                    
                    SpawnProjectile(projectileRotation);
                }
            }
            
            fireCoolDown = 1f / fireRate;
        }

        fireCoolDown -= Time.deltaTime;
    }

    void SpawnProjectile(Quaternion rotation)
    {
        //calcul critique
        bool isCrit = Random.value <= (criticalRate / 100f);
        float finalDamage = isCrit ? damage * criticalDamage : damage;

        GameObject proj = Instantiate(projectilePrefab,firePoint.position, rotation);

        //For classical projectile
        Projectile_classic pC = proj.GetComponent<Projectile_classic>();
        if (pC != null)
        {
            pC.damage = finalDamage;
            pC.isCritical = isCrit;
        }
        //For canon projectile
        Projectile_Canon pCa = proj.GetComponent<Projectile_Canon>();
        if (pCa != null)
        {
            pCa.damage = finalDamage;
            pCa.isCritical = isCrit;
        }

        if (isCrit) Debug.Log("Crit" + damage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
