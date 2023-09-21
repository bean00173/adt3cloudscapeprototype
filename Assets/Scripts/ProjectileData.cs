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
            transform.position = Vector3.Slerp(spawn, target.position, 1f);
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

    public void PlayerShot(float dmg, Transform tgt)
    {
        damage = dmg;
        target = tgt;
        player = true;
    }

}
