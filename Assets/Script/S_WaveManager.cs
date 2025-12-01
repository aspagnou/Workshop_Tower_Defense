using TMPro;
using UnityEngine;

public class S_WaveManager : MonoBehaviour
{
    [Header("Text UI")]
    [SerializeField] private TextMeshProUGUI waveCounterLabel;
    
    [SerializeField] private TextMeshProUGUI nextWaveLabel;

    public Transform[] wayPoints; //Array of waypoints for the enemies to follow

    public S_Wave[] waves;

    public int currentWaveIndex = -1;

    [SerializeField] private int TimeToStartFirstWave;

    public float countDown;

    

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        countDown = TimeToStartFirstWave;
       

        foreach (S_Wave wave in waves)
        {
           wayPoints = wave.wayPoints ;
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
            countDown = waves[currentWaveIndex].delayBetweenWaveSetByThisWave;
            waves[currentWaveIndex].StartWave();
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;
        waveCounterLabel.text = Mathf.Round(currentWaveIndex+1).ToString();
        nextWaveLabel.text = Mathf.Round(countDown+2).ToString();
    }
}
