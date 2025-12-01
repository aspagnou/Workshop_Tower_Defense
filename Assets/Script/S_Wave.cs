using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class S_Wave : MonoBehaviour
{

    

    //public GameObject[] enemy;

    public int numberOfEnemies = 5; //Number of enemies spawning in the wave

    [SerializeField] public float spawnInterval; //Time interval between enemy spawns

    public float spawnTimer; //Timer tracking the spawn interval

    public int enemiesSpawned = 0; //Counter for Spawned enemies

    public int nbrEnemiesKillCond; //Checking how many enemies remains

    public bool waveActive = false; //Check if the wave is active

    [SerializeField] public int delayBetweenWaveSetByThisWave;
    [Space(12)]

    [Header("Spawner1")]
    public Queue<S_Enemy> _enemyStack;
    [SerializeField]private List<S_Enemy> _enemyList;

    public Transform wayPointsParent;
    public Transform[] wayPoints;
    [Space(50)]

    [Header("Spawner2")]
    public Queue<S_Enemy> _enemyStack2;
    [SerializeField] private List<S_Enemy> _enemyList2;
    public Transform wayPointsParent2;
    public Transform[] wayPoints2;
    [Space(12)]

    [Header("Spawner3")]
    public Queue<S_Enemy> _enemyStack3;
    [SerializeField] private List<S_Enemy> _enemyList3;
    public Transform wayPointsParent3;
    public Transform[] wayPoints3;
    [Space(12)]

    [Header("Spawner4")]
    public Queue<S_Enemy> _enemyStack4;
    [SerializeField] private List<S_Enemy> _enemyList4;
    public Transform wayPointsParent4;
    public Transform[] wayPoints4;
    [Space(12)]

    [Header("Spawner5")]
    public Queue<S_Enemy> _enemyStack5;
    [SerializeField] private List<S_Enemy> _enemyList5;
    public Transform wayPointsParent5;
    public Transform[] wayPoints5;
    [Space(12)]

    [Header("Spawner6")]
    public Queue<S_Enemy> _enemyStack6;
    [SerializeField] private List<S_Enemy> _enemyList6;
    public Transform wayPointsParent6;
    public Transform[] wayPoints6;
    [Space(12)]

    [Header("Spawner7")]
    public Queue<S_Enemy> _enemyStack7;
    [SerializeField] private List<S_Enemy> _enemyList7;
    public Transform wayPointsParent7;
    public Transform[] wayPoints7;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {   //Spawner1
        numberOfEnemies = _enemyList.Count + _enemyList2.Count + _enemyList3.Count + _enemyList4.Count + _enemyList5.Count + _enemyList6.Count + _enemyList7.Count;
        nbrEnemiesKillCond = _enemyList.Count + _enemyList2.Count + _enemyList3.Count + _enemyList4.Count + _enemyList5.Count + _enemyList6.Count + _enemyList7.Count;
        _enemyStack = new Queue<S_Enemy>(_enemyList);

        //Spawner2
        _enemyStack2 = new Queue<S_Enemy>(_enemyList2);
        
        //Spawner3
        _enemyStack3 = new Queue<S_Enemy>(_enemyList3);
        
        //Spawner4
        _enemyStack4 = new Queue<S_Enemy>(_enemyList4);

        //Spawner5
        _enemyStack5 = new Queue<S_Enemy>(_enemyList5);
        
        //Spawner6
        _enemyStack6 = new Queue<S_Enemy>(_enemyList6);
        
        //Spawner7
        _enemyStack7 = new Queue<S_Enemy>(_enemyList7);
    }
    void Start()
    {   //Automatic set up waypoints 1
        wayPoints = new Transform[wayPointsParent.transform.childCount];

        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i] = wayPointsParent.GetChild(i);
        }
        
        //Automatic set up waypoints 2
        wayPoints2 = new Transform[wayPointsParent2.transform.childCount];

        for (int i = 0; i < wayPoints2.Length; i++)
        {
            wayPoints2[i] = wayPointsParent2.GetChild(i);
        }
        
        //Automatic set up waypoints 3
        wayPoints3 = new Transform[wayPointsParent3.transform.childCount];

        for (int i = 0; i < wayPoints3.Length; i++)
        {
            wayPoints3[i] = wayPointsParent3.GetChild(i);
        }

        //Automatic set up waypoints 4
        wayPoints4 = new Transform[wayPointsParent4.transform.childCount];

        for (int i = 0; i < wayPoints4.Length; i++)
        {
            wayPoints4[i] = wayPointsParent4.GetChild(i);
        }

        //Automatic set up waypoints 5
        wayPoints5 = new Transform[wayPointsParent5.transform.childCount];

        for (int i = 0; i < wayPoints5.Length; i++)
        {
            wayPoints5[i] = wayPointsParent5.GetChild(i);
        }

        //Automatic set up waypoints 6
        wayPoints6 = new Transform[wayPointsParent6.transform.childCount];

        for (int i = 0; i < wayPoints6.Length; i++)
        {
            wayPoints6[i] = wayPointsParent6.GetChild(i);
        }

        //Automatic set up waypoints 7
        wayPoints7 = new Transform[wayPointsParent7.transform.childCount];

        for (int i = 0; i < wayPoints7.Length; i++)
        {
            wayPoints7[i] = wayPointsParent7.GetChild(i);
        }
        
    }
    public void StartWave()
    {
        waveActive = true;
        
        spawnTimer = spawnInterval;
        enemiesSpawned = 0;
        
    }
    // Update is called once per frame
    void Update()
    {
        if (waveActive)
        {
            spawnTimer -= Time.deltaTime; 

            if (spawnTimer <= 0f && enemiesSpawned < numberOfEnemies)
            {
                SpawnEnemy();
                spawnTimer = spawnInterval;
            }
        }

        


    }

    private void OneEnemyIsDead(S_Enemy deadEn)
    {
        _enemyList.Remove(deadEn);
        _enemyList2.Remove(deadEn);
        _enemyList3.Remove(deadEn);
        _enemyList4.Remove(deadEn);
        _enemyList5.Remove(deadEn);
        _enemyList6.Remove(deadEn);
        _enemyList7.Remove(deadEn);
        nbrEnemiesKillCond--;
        GetComponentInParent<S_WaveManager>().totalMana += deadEn.nbreMana;
    }


   

    void SpawnEnemy()
    {
        //int rand = Random.Range(0, enemy.Length); // Randomely select an enemy type from the array
        //GameObject newEnemy = Instantiate(enemy[rand], transform.position, Quaternion.identity);
        //newEnemy.GetComponent<S_Enemy>().WayPoints = wayPoints;
        if (_enemyStack.Count > 0)
        {
            S_Enemy enemyCandidate = _enemyStack.Dequeue();
            S_Enemy enemyInstance = Instantiate(enemyCandidate, GetComponentInParent<S_WaveManager>().spawnpointsChildrens[0].transform.position, Quaternion.identity);
            enemyInstance.GetComponent<S_Enemy>().WayPoints = wayPoints;
            enemyInstance.OnDead += OneEnemyIsDead;

            _enemyList.Add(enemyInstance);

            enemiesSpawned++;
        }

        if (_enemyStack2.Count > 0)
        {
            S_Enemy enemyCandidate2 = _enemyStack2.Dequeue();
            S_Enemy enemyInstance2 = Instantiate(enemyCandidate2, GetComponentInParent<S_WaveManager>().spawnpointsChildrens[1].transform.position, Quaternion.identity);
            enemyInstance2.GetComponent<S_Enemy>().WayPoints = wayPoints2;
            enemyInstance2.OnDead += OneEnemyIsDead;

            _enemyList2.Add(enemyInstance2);

            enemiesSpawned++;
        }

        if (_enemyStack3.Count > 0)
        {
            S_Enemy enemyCandidate3 = _enemyStack3.Dequeue();
            S_Enemy enemyInstance3 = Instantiate(enemyCandidate3, GetComponentInParent<S_WaveManager>().spawnpointsChildrens[2].transform.position, Quaternion.identity);
            enemyInstance3.GetComponent<S_Enemy>().WayPoints = wayPoints3;
            enemyInstance3.OnDead += OneEnemyIsDead;

            _enemyList3.Add(enemyInstance3);

            enemiesSpawned++;
        }
        
        if (_enemyStack4.Count > 0)
        {
            S_Enemy enemyCandidate4 = _enemyStack4.Dequeue();
            S_Enemy enemyInstance4 = Instantiate(enemyCandidate4, GetComponentInParent<S_WaveManager>().spawnpointsChildrens[3].transform.position, Quaternion.identity);
            enemyInstance4.GetComponent<S_Enemy>().WayPoints = wayPoints4;
            enemyInstance4.OnDead += OneEnemyIsDead;

            _enemyList4.Add(enemyInstance4);

            enemiesSpawned++;
        }

        if (_enemyStack5.Count > 0)
        {
            S_Enemy enemyCandidate5 = _enemyStack5.Dequeue();
            S_Enemy enemyInstance5 = Instantiate(enemyCandidate5, GetComponentInParent<S_WaveManager>().spawnpointsChildrens[4].transform.position, Quaternion.identity);
            enemyInstance5.GetComponent<S_Enemy>().WayPoints = wayPoints5;
            enemyInstance5.OnDead += OneEnemyIsDead;

            _enemyList5.Add(enemyInstance5);

            enemiesSpawned++;
        }

        if (_enemyStack6.Count > 0)
        {
            S_Enemy enemyCandidate6 = _enemyStack6.Dequeue();
            S_Enemy enemyInstance6 = Instantiate(enemyCandidate6, GetComponentInParent<S_WaveManager>().spawnpointsChildrens[5].transform.position, Quaternion.identity);
            enemyInstance6.GetComponent<S_Enemy>().WayPoints = wayPoints6;
            enemyInstance6.OnDead += OneEnemyIsDead;

            _enemyList6.Add(enemyInstance6);

            enemiesSpawned++;
        }


        if (_enemyStack7.Count > 0)
        {
            S_Enemy enemyCandidate7 = _enemyStack7.Dequeue();
            S_Enemy enemyInstance7 = Instantiate(enemyCandidate7, GetComponentInParent<S_WaveManager>().spawnpointsChildrens[6].transform.position, Quaternion.identity);
            enemyInstance7.GetComponent<S_Enemy>().WayPoints = wayPoints7;
            enemyInstance7.OnDead += OneEnemyIsDead;

            _enemyList7.Add(enemyInstance7);

            enemiesSpawned++;
        }







        if (enemiesSpawned >= numberOfEnemies) // && nbrEnemiesKillCond <= 0 || _enemyList.Count == 0
        {
            waveActive = false;
            Debug.Log("Wave completed");
            GetComponentInParent<S_WaveManager>().StartNextWave(delayBetweenWaveSetByThisWave);
            gameObject.SetActive(false);
        }


    }


    
}
