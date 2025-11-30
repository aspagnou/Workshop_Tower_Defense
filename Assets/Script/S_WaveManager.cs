using UnityEngine;

public class S_WaveManager : MonoBehaviour
{

    public Transform[] wayPoints; //Array of waypoints for the enemies to follow

    public S_Wave[] waves;

    public int currentWaveIndex = -1;

    public int TimeToStartFirstWave = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (S_Wave wave in waves)
        {
            wave.wayPoints = wayPoints;
        }

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
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
