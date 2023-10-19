using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ProjectileData : MonoBehaviour
{
    [HideInInspector] public float damage;
    [HideInInspector] public bool special;
    [HideInInspector] public Vector3 motion;
    [HideInInspector] public BowCombat bowArrow;

    public float duration;
    public bool player;
    public GameObject arrowPs;
    public GameObject bloodPs;

    Transform target;

    Vector3 spawn;

    float time;

    private void Start()
    {
        spawn = transform.position;
    }

    private void Update()
    {
        if(this.GetComponent<DespawnTimer>().readyToDespawn) Destroy(this.gameObject);

        if (player)
        {
            //time = 0;
            //duration = 1.5f;

            //target = bowArrow.FindNearestEnemy();

            //time += Time.deltaTime;

            //if (time < duration)
            //{
            //    if (target == null)
            //    {
            //        Destroy(this.gameObject);
            //    }
            //    else
            //    {
            //        Debug.LogError($"{time} / {duration} = {time / duration}");
            //        transform.position = Vector3.Lerp(spawn, target.position, time / duration);
            //    }
            //}

            StartCoroutine(Move());
        }
        else
        {
            transform.position += motion * Time.deltaTime;
        }
    }
    public void ProjectileDamage(float dmg, Vector3 mot)
    {
        damage = dmg;
        motion = mot;
    }

    public void PlayerShot(float dmg, BowCombat bow, bool spc)
    {
        damage = dmg;
        player = true;
        this.bowArrow = bow;
        special = spc;
    }

    private void StopMove()
    {
        time = duration;
        arrowPs.SetActive(false);
        bloodPs.SetActive(true);
    }

    private IEnumerator Move()
    {
        time = 0;

        target = bowArrow.FindNearestEnemy();

        Vector3 startPos = spawn;
        float yValue = (target.GetComponent<EnemyBehaviour>().enemy.enemyType == enemyType.brute || target.GetComponent<EnemyBehaviour>().enemy.enemyType == enemyType.boss) ? spawn.y + (target.GetComponent<NavMeshAgent>().height / 2) : spawn.y;

        while (time < duration)
        {
            if(target == null)
            {
                Destroy(this.gameObject);
                break;
            }
            
            Vector3 movePos = new Vector3(target.position.x, yValue, target.position.z);
            transform.position = Vector3.Lerp(spawn, movePos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
    }

}
