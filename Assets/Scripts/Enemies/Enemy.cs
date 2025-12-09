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
    public float nbreMana = 20;
    [SerializeField] private LayerMask layers;


    [Header("Damage")]
    [SerializeField] public int lifePoints = 50;
    private float currentHealth;
    [SerializeField] public float nexusDamage = 10;
    [SerializeField] private float timeToExplode = 2;
    

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
        nbreMana = 0;
        Destroy(gameObject);

    
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) 
        { 
            Destroy(gameObject);
        }
    }
    

    private void OnDestroy()
    {
        OnDead.Invoke(this);

    }
}
