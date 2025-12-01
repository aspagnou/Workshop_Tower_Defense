using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class S_Enemy : MonoBehaviour
{

    [Header ("Movement")]
    
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float timeToSpawn;

    public int currentWaypointIndex = 0;

    public Transform[] WayPoints;
    public Transform currentWayPoint;

    public Action<S_Enemy> OnDead;

    [Header("Economy")]
    [SerializeField] private float nbreMana = 20;
    //[SerializeField] private GameObject scrapType; Pour plus tard pour l'inventaire

    [Header("Damage")]
    [SerializeField] private float lifePoints = 50;
    [SerializeField] private float nexusDamage = 10;
    [SerializeField] private float timeToExplode = 2;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentWayPoint = WayPoints[0];
    }

    // Update is called once per frame
    void Update()
    {
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

    Destroy(gameObject);
 
    }

  


    private void OnDestroy()
    {
        OnDead.Invoke(this);

    }
}
