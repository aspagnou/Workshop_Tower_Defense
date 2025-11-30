using System.Runtime.CompilerServices;
using UnityEngine;

public class S_Wave : MonoBehaviour
{

    public Transform[] wayPoints;

    public GameObject[] enemy;

    public int numberOfEnemies = 5; //Number of enemies spawning in the wave

    public float spawnInterval = 2f; //Time interval between enemy spawns

    public float spawnTimer; //Timer tracking the spawn interval

    public int enemiesSpawned = 0; //Counter for Spawned enemies

    public bool waveActive = false; //Check if the wave is active

    [SerializeField] public int delayBetweenWaveSetByThisWave;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
    }


   

    void SpawnEnemy()
    {
        int rand = Random.Range(0, enemy.Length); // Randomely select an enemy type from the array
        GameObject newEnemy = Instantiate(enemy[rand], transform.position, Quaternion.identity);
        newEnemy.GetComponent<S_EnemyMovement>().WayPoints = wayPoints;
        enemiesSpawned++;

        if (enemiesSpawned >= numberOfEnemies)
        {
            waveActive = false;
            Debug.Log("Wave completed");
            GetComponentInParent<S_WaveManager>().StartNextWave(delayBetweenWaveSetByThisWave);
            gameObject.SetActive(false);
        }

    }
}
