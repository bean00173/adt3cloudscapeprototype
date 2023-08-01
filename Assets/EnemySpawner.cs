using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.Image;

public class EnemySpawner : MonoBehaviour
{
    public Transform enemyContainer;
    public Transform[] spawnPoints;
    private Transform[] validSpawns;
    public List<Wave> waves = new List<Wave>();

    private Transform player;
    private int waveIndex;
    private int enemiesLeft;
    private int aliveEnemies;
    
    // Start is called before the first frame update
    void Start()
    {
        waveIndex = 0;
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemiesLeft == 0)
        {
            Debug.Log("Wave Beaten");
        }
        else if(aliveEnemies < 3)
        {
            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        for(int i = 0; i < 5; i++)
        {
            Instantiate(waves[waveIndex].enemies[Random.Range(0, waves[waveIndex].enemies.Length - 1)], FindSpawnPoint().position, Quaternion.identity, enemyContainer);
            
        }
    }

    private Transform FindSpawnPoint()
    {
        foreach(Transform potentialSpawn in  spawnPoints)
        {
            if(Vector3.Distance(potentialSpawn.position, player.position) < 5.0f)
            {
                validSpawns.Append(potentialSpawn);
            }
        }

        return validSpawns[Random.Range(0, validSpawns.Length - 1)];
    }
}
