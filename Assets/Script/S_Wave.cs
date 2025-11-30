using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class S_Wave : MonoBehaviour
{

    public Transform[] wayPoints;

    //public GameObject[] enemy;

    public int numberOfEnemies = 5; //Number of enemies spawning in the wave

    public float spawnInterval = 2f; //Time interval between enemy spawns

    public float spawnTimer; //Timer tracking the spawn interval

    public int enemiesSpawned = 0; //Counter for Spawned enemies

    public int nbrEnemiesKillCond; //Checking how many enemies remains

    public bool waveActive = false; //Check if the wave is active

    [SerializeField] public int delayBetweenWaveSetByThisWave;

    [Header("Sequence")]
    public Queue<S_Enemy> _enemyStack;
    [SerializeField]private List<S_Enemy> _enemyList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {
        nbrEnemiesKillCond = _enemyList.Count;
        _enemyStack = new Queue<S_Enemy>(_enemyList);
    }
    void Start()
    {
        //Invoke("StartWave", 2);
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

        if (nbrEnemiesKillCond <= 0) //enemiesSpawned >= numberOfEnemies &&
        {
            waveActive = false;
            Debug.Log("Wave completed");
            GetComponentInParent<S_WaveManager>().StartNextWave(delayBetweenWaveSetByThisWave);
            gameObject.SetActive(false);
        }
    }

    private void OneEnemyIsDead(S_Enemy deadEn)
    {
        _enemyList.Remove(deadEn);
        nbrEnemiesKillCond--;
    }


   

    void SpawnEnemy()
    {
        //int rand = Random.Range(0, enemy.Length); // Randomely select an enemy type from the array
        //GameObject newEnemy = Instantiate(enemy[rand], transform.position, Quaternion.identity);
        //newEnemy.GetComponent<S_Enemy>().WayPoints = wayPoints;
        
        S_Enemy enemyCandidate = _enemyStack.Dequeue();
        S_Enemy enemyInstance = Instantiate(enemyCandidate, transform.position, Quaternion.identity);
        enemyInstance.GetComponent<S_Enemy>().WayPoints = wayPoints;
        enemyInstance.OnDead += OneEnemyIsDead;

        _enemyList.Add(enemyInstance);

        enemiesSpawned++;
        

        

    }


    
}
