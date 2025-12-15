using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SpawnerManager : MonoBehaviour
{
    public Nexus nexus;
    public EndConditionUI endUi;

    public Dictionary<EnemyType, int> totalEnemiesPerWave = new();
    public int totalSpawnedEnemiesPerWave;
    public int currentSpawnerIndex = 0;
    public int timeBetweenWaves;
    public int timeBeforeFirstWave;

    public int totalEnemiesToKill;
    
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
        if (enemy == null)
            return;

        if (totalEnemiesPerWave.ContainsKey(enemy.enemyType))
            totalEnemiesPerWave[enemy.enemyType]--;

        totalEnemiesPerWave[EnemyType.Global]--;

        if (totalEnemiesPerWave[EnemyType.Global] < 0)
            totalEnemiesPerWave[EnemyType.Global] = 0;

        // 🔥 Fin réelle de la vague
        if (totalEnemiesPerWave[EnemyType.Global] == 0)
        {
            // Stop uniquement les fades actifs
            foreach (WaveManager wm in allWavesManager)
            {
                wm.OnWaveFinished();
            }

            if (currentSpawnerIndex < allWavesManager[0].waves.Length - 1)
            {
                currentSpawnerIndex++;
                CalculTotalEnemies(currentSpawnerIndex);

                foreach (WaveManager wm in allWavesManager)
                {
                    wm.StartNextWave(timeBetweenWaves);
                }
            }
        }
    }



    // Update is called once per frame
    void FixedUpdate()
    {

        if (currentSpawnerIndex == 11 && totalEnemiesToKill == 0 && nexus.currentHealth > 0)
        {
            Debug.Log("you won");
            endUi.Win();
        }

        if (nexus.currentHealth <= 0)
        {
            Debug.Log("you lost");
            endUi.Loose();
        }
    }
}
