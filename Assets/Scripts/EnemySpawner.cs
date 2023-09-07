using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform enemyContainer;
    private List<Transform> spawns = new List<Transform>(); 
    private List<Transform> validSpawns = new List<Transform>();
    private List<Wave> waves = new List<Wave>();

    private Transform player;
    private Transform furthest;
    private int waveIndex;
    public int enemiesLeft { get; private set; }
    private int aliveEnemies;

    bool nextFloor;

    public GameObject bossUI, hbCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        waves = GameManager.instance.ReturnTowerData().floors[GameManager.floorIndex].waves;
        foreach(Transform child in this.transform) // gather list of spawn points
        {
            spawns.Add(child);
        }
        waveIndex = 0;
        enemiesLeft = waves[0].waveEnemyCount; // set current amount of enemies to spawn to be maximum for current wave
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.instance.bossUI)
        {
            bossUI.SetActive(true);
        }

        if (enemiesLeft == 0 && aliveEnemies == 0) // if there are no more enemies to spawn, and no more enemies alive...
        {
            Debug.Log("Wave Beaten");
            waveIndex++;
            if (waveIndex <= waves.Count - 1) // check if that was last wave, then continue to next wave if true, or end if false
            {
                enemiesLeft = waves[waveIndex].waveEnemyCount;
            }
            else if (!nextFloor)
            {
                nextFloor = true;

                player.GetComponent<PlayableCharacter>().canInteract = true;
                Debug.Log("No Waves Left");
                GameManager.instance.NextFloor();
            }
        }

        if(aliveEnemies > 0)
        {
            player.GetComponent<PlayableCharacter>().canInteract = false;
        }
        //else if (enemiesLeft <= 10) // if the wave is down to its last 10, spawn all at once, with a greater radius
        //{
        //    SpawnEnemies(enemiesLeft, 5.0f);
        //}
        //else if (aliveEnemies < 3 && enemiesLeft >= 15) // if the previous wave is about to be finished, spawn 5 more, however only if there are more than 15 left, due to the spawn last 10 together rule
        //{
        //    SpawnEnemies(5, 2.0f);
        //}

        if(enemiesLeft > 0)
        {

            foreach (WaveEnemy enemy in waves[waveIndex].enemies)
            {
                
                Debug.Log(enemy.enemy.name);

                int currentOfType = 0;

                foreach (Transform enemyTransform in enemyContainer)
                {
                    if (enemyTransform.GetComponent<EnemyBehaviour>().enemy.enemyType == enemy.enemy.GetComponent<EnemyBehaviour>().enemy.enemyType)
                    {
                        Debug.Log("Existing Enemy Found");
                        currentOfType += 1;
                    }
                }

                Debug.Log(enemy.maxCount - currentOfType);

                int spawnNum = enemy.maxCount - currentOfType;
                if (spawnNum > enemiesLeft) spawnNum = enemiesLeft;

                if (spawnNum > 0)
                {
                    SpawnEnemies(enemy.enemy, spawnNum);
                }
            }
        }

        aliveEnemies = enemyContainer.childCount; // update alive enemies with those stored in heirarchy
    }

    //private void SpawnEnemies(int x, float radius) 
    //{
    //    Transform spawnPoint = FindSpawnPoint(); // gather spawn point
    //    for(int i = 0; i < x; i++) // within number of enemies to be spawned, instantiate random enemy type, at randomised spawn point (using radial spawn)
    //    {
    //        if (CanSpawnBrute())
    //        {
    //            Debug.Log("Brute");
    //            Instantiate(waves[waveIndex].enemies[waves[waveIndex].enemies.Count - 1], GameManager.instance.SpawnPosition(i, x, spawnPoint.position, radius), Quaternion.identity, enemyContainer);
    //        }
    //        else
    //        {
    //            Debug.Log("Grunt");
    //            Instantiate(waves[waveIndex].enemies[Random.Range(0, waves[waveIndex].enemies.Count - 1)], GameManager.instance.SpawnPosition(i, x, spawnPoint.position, radius), Quaternion.identity, enemyContainer);
    //        }
    //        enemiesLeft -= 1;
    //    }
    //}

    private void SpawnEnemies(GameObject enemy, int count)
    {
        Transform spawnPoint = FindSpawnPoint();
        float radius = enemy.GetComponent<EnemyBehaviour>().enemy.enemyType == enemyType.brute ? radius = 5.0f : radius = 2.0f;

        for (int i = 0; i < count; i++)
        {
            Debug.Log("Spawning Enemy");
            Transform agent = Instantiate(enemy, GameManager.instance.SpawnPosition(i, count, spawnPoint.position, radius), Quaternion.identity, enemyContainer).transform;
            agent.GetComponent<EnemyBehaviour>().SetupHealthBar(hbCanvas.transform);
            enemiesLeft -= 1;
        }

    }

    private Transform FindSpawnPoint()
    {
        float furthestDist = 0;
        //foreach(Transform potentialSpawn in spawns) // for each stored spawn point
        //{
        //    if(Vector3.Distance(potentialSpawn.position, player.position) > 30.0f) // calculate if spawn point isn't in certain range of player
        //    {
        //        validSpawns.Add(potentialSpawn); // add to valid spawn list
        //    }
        //}

        //if (validSpawns.Count > 0) // if there are some valid spawns
        //{
        //    return validSpawns[Random.Range(0, validSpawns.Count)]; // spawn using a random one of them
        //}
        //else 
        //{
        //    return spawns[Random.Range(0, spawns.Count)]; // if not, spawn at a random invalidated spawn and pray for the best
        //}
        foreach (Transform child in spawns)
        {
            float dist = Vector3.Distance(player.transform.position, child.transform.position);
            Debug.Log(child.name + " " + dist);
            if (furthestDist == 0)
            {
                furthestDist = dist;
                furthest = child;
            }
            else if (dist > furthestDist)
            {
                furthestDist = dist;
                furthest = child;
            }
        }

        return furthest;
    }

    //private bool CanSpawnBrute() // bool method to check if allowed to spawn a brute
    //{
    //    //foreach (GameObject go in waves[waveIndex].enemies) // search potential enemy spawns
    //    //{
    //    //    if (go.GetComponent<EnemyBehaviour>().enemy.enemyType == enemyType.brute)
    //    //    {
    //    //        return GameManager.instance.RandomChance(10); // if brute enemy is found in list, use random chance to return true or false for spawning 
    //    //    }
    //    //}
    //    //return false; // else no brute
    //}

}
