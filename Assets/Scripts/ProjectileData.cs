using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileData : MonoBehaviour
{
    [HideInInspector]
    public float damage;
    public Vector3 motion;

    public bool player;

    Transform target;
    BowCombat bowArrow;
    Vector3 spawn;


    private void Start()
    {
        spawn = transform.position;
    }

    private void Update()
    {
        if(this.GetComponent<DespawnTimer>().readyToDespawn) Destroy(this.gameObject);

        if (player)
        {
            float time = 0;
            float duration = 1.5f;

            target = bowArrow.FindNearestEnemy();

            Debug.LogError(target);

            while (time < duration)
            {
                if (target == null)
                {
                    Destroy(this.gameObject);
                    break;
                }

                transform.position = Vector3.Lerp(spawn, target.position, time / duration);
                time += Time.deltaTime;

            }

            
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

    public void PlayerShot(float dmg, BowCombat bow)
    {
        damage = dmg;
        player = true;
        this.bowArrow = bow;
    }

}
