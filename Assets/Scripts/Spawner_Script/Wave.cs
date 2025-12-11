using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Wave : MonoBehaviour
{


    public SpawnerManager spawnerManager;
    public WaveManager manager;

    public int numberOfEnemies = 5; //Number of enemies spawning in the wave

    [SerializeField] public float spawnInterval; //Time interval between enemy spawns

    public float spawnTimer; //Timer tracking the spawn interval

    public int enemiesSpawned = 0; //Counter for Spawned enemies

    public int nbrEnemiesKillCond; //Checking how many enemies remains

    public bool waveActive = false; //Check if the wave is active

    [SerializeField] public int delayBetweenWaveSetByThisWave;
    


    [Header("Spawner1")]
    public Queue<Enemy> _enemyStack;
    [SerializeField]public List<Enemy> _enemyList;

    
    

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {   //Spawner1
        numberOfEnemies = _enemyList.Count;
        nbrEnemiesKillCond = _enemyList.Count;

        _enemyStack = new Queue<Enemy>(_enemyList);  
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
        manager.nbreSpawnedEnemies = 0;


    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
        

        if (waveActive)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0f && enemiesSpawned < numberOfEnemies)// manager.totalEnemies
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

    private void OneEnemyIsDead(Enemy deadEn)
    {

        
        spawnerManager.totalEnemiesToKill--;
        //manager.totalMana += deadEn.nbreMana;

        if (deadEn.canDamageNexus == true)
        {
            spawnerManager.nexus.NexusDamaged(deadEn.nexusDamage);
            deadEn.nbreMana = 0;
        }
       
    }


   

    void SpawnEnemy()
    {
        if (_enemyStack.Count > 0)
        {
            Enemy enemyCandidate = _enemyStack.Dequeue();
            Enemy enemyInstance = Instantiate(enemyCandidate, manager.spawnPoint.transform.position, Quaternion.identity);
            enemyInstance.GetComponent<Enemy>().WayPoints = manager.wayPoints;
            enemyInstance.OnDead += OneEnemyIsDead;

            spawnerManager.totalEnemiesToKill++;
            spawnerManager.RemoveEnemy(enemyInstance);

            spawnInterval = enemyCandidate.timeToSpawn;
            enemiesSpawned++;
            spawnerManager.totalSpawnedEnemiesPerWave++;
        }


        //if (spawnerManager.totalSpawnedEnemiesPerWave == spawnerManager.totalEnemiesPerWave) 
        //{
        //    waveActive = false;
        //    spawnerManager.totalSpawnedEnemiesPerWave = 0;
        //    Debug.Log("Wave completed");
        //    manager.StartNextWave(manager.TimeBetweenWaves);
        //    gameObject.SetActive(false);
        //}


    }


    
}
