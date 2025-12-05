using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{

    public int totalEnemiesPerWave;
    
    public SpawnerManager spawnerManager;
    public WaveManager[] allWavesManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform parent = spawnerManager.transform;

        List<WaveManager> wavesManagerList = new List<WaveManager>();

        for (int i = 0; i < parent.childCount; i++)
        {
            WaveManager wM = parent.GetChild(i).GetComponent<WaveManager>();
            if (wM != null)
                wavesManagerList.Add(wM);
        }

        allWavesManager = wavesManagerList.ToArray();

        CalculTotalEnemies();
    }

    public void CalculTotalEnemies()
    {
        
        totalEnemiesPerWave = 0;
        
        for (int i = 0; i < allWavesManager.Length; i++)
        {
            totalEnemiesPerWave += allWavesManager[i].nbreEnemies;
        }
        Debug.Log("J'aiFait");
    }

    
    // Update is called once per frame
    void Update()
    {
        

    }
}
