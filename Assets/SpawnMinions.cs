using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMinions : MonoBehaviour
{
    public int minionCount;
    public float cdPeriod;
    public int chance;
    EnemySpawner spawner;

    bool canTry;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GameObject.FindObjectOfType<EnemySpawner>();
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update()
    {
        if(canTry)
        {
            canTry = false;
            if (GameManager.instance.RandomChance(chance))
            {
                SpawnEnemies();
            }

        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(cdPeriod);
        canTry = true;
    }

    private void SpawnEnemies()
    {
        this.GetComponentInChildren<Animator>().SetTrigger("Summon");
        spawner.DoExternalEnemySpawn((int)Mathf.Round(Random.Range(.75f, 1.25f) * minionCount));
    }

    public void MinionDeath()
    {
        spawner.BossDied();
    }
}
