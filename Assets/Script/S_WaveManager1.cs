using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S_WaveManager : MonoBehaviour
{

    [Header("Text UI")]
    [SerializeField] private TextMeshProUGUI waveCounterLabel;
    
    [SerializeField] private TextMeshProUGUI nextWaveLabel;
    [SerializeField] private TextMeshProUGUI ManaLabel;
    public float totalMana;

    [Header("Wave system")]

    [SerializeField] public GameObject nexus;

    public Transform[] wayPoints; //Array of waypoints for the enemies to follow

    public S_Wave[] waves;

    public int currentWaveIndex = -1;

    [Header("SpawnPoints")]

    public Transform spawnpointsParent;
    public Transform[] spawnpointsChildrens;

    [Header("Timing")]

    [SerializeField] public int TimeToStartFirstWave;

    public float countDown;
    

    

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        countDown = TimeToStartFirstWave;

        //Get all spwaners in a list
        spawnpointsChildrens = new Transform[spawnpointsParent.transform.childCount];

        for (int i = 0; i < spawnpointsChildrens.Length; i++)
        {
            spawnpointsChildrens[i] = spawnpointsParent.GetChild(i);
        }
       

        //foreach (S_Wave wave in waves)
        //{
        //   wayPoints = wave.wayPoints ;
        //}

        StartNextWave(TimeToStartFirstWave);
    }

    public void StartNextWave(int delayBetweenWavesSetByPreviousWave)
    {
        
        Invoke("startWaves", delayBetweenWavesSetByPreviousWave);
        
    }

    void startWaves()
    {
        
        if (currentWaveIndex < waves.Length - 1)
        {
            
            currentWaveIndex++;
            waves[currentWaveIndex].StartWave();
            countDown = waves[currentWaveIndex].countDownBetween;
        }
    }

    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;
        waveCounterLabel.text = Mathf.Round(currentWaveIndex+1).ToString();
        nextWaveLabel.text = Mathf.Round(countDown).ToString();
        ManaLabel.text = Mathf.Round(totalMana).ToString();
    }
}
