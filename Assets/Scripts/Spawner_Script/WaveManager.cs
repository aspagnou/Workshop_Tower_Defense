using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{

    [Header("Text UI")]
    [SerializeField] private TextMeshProUGUI waveCounterLabel;
    [SerializeField] private TextMeshProUGUI nextWaveLabel;
    //[SerializeField] private TextMeshProUGUI ManaLabel;
    public float totalMana;

    [Header("Wave system")]
    //public Transform[] wayPoints; //Array of waypoints for the enemies to follow
    public SpawnerManager spawnerManager;
    public WaveManager waveManager;
    public Wave[] waves;

    public int currentWaveIndex = -1;

    public Transform wayPointsParent;
    public Transform[] wayPoints;

    [Header("SpawnPoints")]
    [SerializeField] public Transform spawnPoint;
    //public Transform spawnpointsParent;
    //public Transform[] spawnpointsChildrens;

    [Header("Timing")]
    [SerializeField] public int TimeToStartFirstWave = 5;
    
    [SerializeField] public int timeModif;
    //[SerializeField] public float spawnInterval; //Time interval between enemy spawns
    //public float spawnTimer; //Timer tracking the spawn interval
    //public int enemiesSpawned = 0; //Counter for Spawned enemies

    public float countDown;

    [Header("Spawning System")]
    public int nbreEnemies;
    public int totalEnemies;
    public int nbreSpawnedEnemies;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        //Get all waves automatically
        Transform parent = waveManager.transform;

        List<Wave> wavesList = new List<Wave>();

        for (int i = 0; i < parent.childCount; i++)
        {
            Wave w = parent.GetChild(i).GetComponent<Wave>();
            if (w != null)
                wavesList.Add(w);
        }

        waves = wavesList.ToArray();
    }


    void Start()
    {

        countDown = spawnerManager.timeBeforeFirstWave;

       

        foreach (Wave w in waves)
        {
            w.manager = this;
            
        }

        //get waypoints
        wayPoints = new Transform[wayPointsParent.transform.childCount];

        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i] = wayPointsParent.GetChild(i);
        }

        //Start the first Wave
        StartNextWave(spawnerManager.timeBeforeFirstWave);

    }

    public void StartNextWave(int delayBetweenWavesSetByPreviousWave)
    {
        
        Invoke("startWaves", delayBetweenWavesSetByPreviousWave);
        //TimeBetweenWaves = TimeBetweenWaves * timeModif;
        if (currentWaveIndex >= 0)
        {
            countDown = spawnerManager.timeBetweenWaves;

        }

    }

    public void FirstWaveCountDown()
    {
        countDown = spawnerManager.timeBetweenWaves;
    }

    void startWaves()
    {
        if (currentWaveIndex < waves.Length - 1)
        { 
            currentWaveIndex++;
            waves[currentWaveIndex].StartWave();
        }
        
        
        //totalEnemies = spawnerManager.totalEnemiesPerWave;
        
        
    }


    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;
        waveCounterLabel.text = $"Wave {currentWaveIndex + 1} / 12";
        nextWaveLabel.text = $"Next Wave In : {Mathf.Round(countDown)}s";
        if (countDown <= 0)
        {
            nextWaveLabel.text = ("Spawning ...");
        }
        //ManaLabel.text = Mathf.Round(totalMana).ToString();
    }
}
