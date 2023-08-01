using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.Image;

public class EnemySpawner : MonoBehaviour
{
    public Transform enemyContainer;
    private List<Transform> spawns = new List<Transform>(); 
    private List<Transform> validSpawns = new List<Transform>();
    public List<Wave> waves = new List<Wave>();

    private Transform player;
    private int waveIndex;
    private int enemiesLeft;
    private int aliveEnemies;

    
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in this.transform)
        {
            spawns.Add(child);
        }
        waveIndex = 0;
        enemiesLeft = waves[0].enemyCount;
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

        if (enemiesLeft == 0 && aliveEnemies == 0)
        {
            Debug.Log("Wave Beaten");
            waveIndex++;
            if(waveIndex <= waves.Count -1)
            {
               enemiesLeft = waves[waveIndex].enemyCount;
            }
            else
            {
               Debug.Log("No Waves Left");
            }
        }
        else if(enemiesLeft <= 10)
        {
            SpawnEnemies(enemiesLeft, 5.0f);
        }
        else if(aliveEnemies < 3 && enemiesLeft >= 15)
        {
            SpawnEnemies(5, 2.0f);
        }

        aliveEnemies = enemyContainer.childCount;
    }

    private void SpawnEnemies(int x, float radius)
    {
        Transform spawnPoint = FindSpawnPoint();
        for(int i = 0; i < x; i++)
        {
            Instantiate(waves[waveIndex].enemies[Random.Range(0, waves[waveIndex].enemies.Count -1)], GameManager.instance.SpawnPosition(i, x, spawnPoint.position, radius), Quaternion.identity, enemyContainer);
            enemiesLeft -= 1;
        }
    }

    private Transform FindSpawnPoint()
    {
        foreach(Transform potentialSpawn in spawns)
        {
            Debug.DrawLine(potentialSpawn.position, player.position, Color.yellow);
            if(Vector3.Distance(potentialSpawn.position, player.position) > 15.0f)
            {
                validSpawns.Add(potentialSpawn);
            }
        }

        if (validSpawns.Count > 0)
        {
            return validSpawns[Random.Range(0, validSpawns.Count)];
        }
        else
        {
            return enemyContainer.transform;
        }
    }
}
