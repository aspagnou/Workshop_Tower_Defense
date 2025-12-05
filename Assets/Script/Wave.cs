using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Wave : MonoBehaviour
{



    public WaveManager manager;

    public int numberOfEnemies = 5; //Number of enemies spawning in the wave

    [SerializeField] public float spawnInterval; //Time interval between enemy spawns

    public float spawnTimer; //Timer tracking the spawn interval

    public int enemiesSpawned = 0; //Counter for Spawned enemies

    public int nbrEnemiesKillCond; //Checking how many enemies remains

    public bool waveActive = false; //Check if the wave is active

    [SerializeField] public int delayBetweenWaveSetByThisWave;
    


    [Header("Spawner1")]
    public Queue<S_Enemy> _enemyStack;
    [SerializeField]private List<S_Enemy> _enemyList;

    
    

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {   //Spawner1
        numberOfEnemies = _enemyList.Count;
        
        _enemyStack = new Queue<S_Enemy>(_enemyList);  
    }
    void Start()
    {

    }
    public void StartWave()
    {

        waveActive = true;
        manager.nbreEnemies = numberOfEnemies;

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

            //if (enemiesSpawned == numberOfEnemies)
            //{
            //    spawnInterval = 0.1f;
            //}
        }
    }

    private void OneEnemyIsDead(S_Enemy deadEn)
    {
        _enemyList.Remove(deadEn);
        
        nbrEnemiesKillCond--;
        manager.totalMana += deadEn.nbreMana;
       
    }


   

    void SpawnEnemy()
    {
        if (_enemyStack.Count > 0)
        {
            S_Enemy enemyCandidate = _enemyStack.Dequeue();
            S_Enemy enemyInstance = Instantiate(enemyCandidate, manager.spawnPoint.transform.position, Quaternion.identity);
            enemyInstance.GetComponent<S_Enemy>().WayPoints = manager.wayPoints;
            enemyInstance.OnDead += OneEnemyIsDead;

            _enemyList.Add(enemyInstance);

            //spawnInterval = enemyCandidate.timeToSpawn;
            enemiesSpawned++;
        }


        if (enemiesSpawned >= numberOfEnemies) 
        {
            waveActive = false;
            Debug.Log("Wave completed");
            manager.StartNextWave(manager.TimeBetweenWaves);
            //gameObject.SetActive(false);
        }


    }


    
}
