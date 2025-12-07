using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Towers : MonoBehaviour
{
    [SerializeField] private float range = 10f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float criticalRate = 0f;
    [SerializeField] private float criticalDamage = 0f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private int nbProjectile = 1;
    private float fireCoolDown = 0f;

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

            for (int i = 0; i < nbProjectile; i++)
            {
                Shoot();
            }
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
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            fireCoolDown = 1f / fireRate;
        }

        fireCoolDown -= Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
