using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SpawnerManager : MonoBehaviour
{
    
    public Dictionary<EnemyType, int> totalEnemiesPerWave = new();
    public int totalSpawnedEnemiesPerWave;
    public int currentSpawnerIndex = 0;
    public int timeBetweenWaves;
    
    public SpawnerManager spawnerManager;
    public WaveManager[] allWavesManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalEnemiesPerWave[EnemyType.Global] = 0;

        Transform parent = spawnerManager.transform;

        List<WaveManager> wavesManagerList = new List<WaveManager>();

        for (int i = 0; i < parent.childCount; i++)
        {
            WaveManager wM = parent.GetChild(i).GetComponent<WaveManager>();
            if (wM != null)
                wavesManagerList.Add(wM);
        }

        allWavesManager = wavesManagerList.ToArray();

        CalculTotalEnemies(currentSpawnerIndex);
    }

    public void CalculTotalEnemies(int currentWaveIndex)
    {
        
        
        
        for (int i = 0; i < allWavesManager.Length; i++)
        {
            for (int j = 0; j < allWavesManager[i].waves[currentWaveIndex]._enemyList.Count; j++)
            {
                
                if (totalEnemiesPerWave.ContainsKey(allWavesManager[i].waves[currentWaveIndex]._enemyList[j].enemyType))
                {
                    totalEnemiesPerWave[allWavesManager[i].waves[currentWaveIndex]._enemyList[j].enemyType] += 1;
                }
                else
                {
                    totalEnemiesPerWave[allWavesManager[i].waves[currentWaveIndex]._enemyList[j].enemyType] = 1;
                }

                totalEnemiesPerWave[EnemyType.Global] ++;
            }
           
        }
        foreach (KeyValuePair<EnemyType, int> item in totalEnemiesPerWave)
        {
            //Debug.LogFormat("Key={0}, Value={1}", item.Key, item.Value);
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        totalEnemiesPerWave[enemy.enemyType] -= 1;
        totalEnemiesPerWave[EnemyType.Global]--; 

        foreach (KeyValuePair<EnemyType, int> item in totalEnemiesPerWave)
        {
            //Debug.LogFormat("Key={0}, Value={1}", item.Key, item.Value);
        }

        if (totalEnemiesPerWave[EnemyType.Global] == 0 && currentSpawnerIndex < allWavesManager[0].waves.Length-1)
        {
            currentSpawnerIndex++;
            CalculTotalEnemies(currentSpawnerIndex);
            for (int i = 0; i < allWavesManager.Length; i++)
            {
                allWavesManager[i].StartNextWave(timeBetweenWaves);
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        

    }
}
