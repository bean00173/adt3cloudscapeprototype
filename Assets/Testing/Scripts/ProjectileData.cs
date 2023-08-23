using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileData : MonoBehaviour
{
    [HideInInspector]
    public float damage;
    public Vector3 motion;

    private void Update()
    {
        if(this.GetComponent<DespawnTimer>().readyToDespawn) Destroy(this.gameObject);

        transform.position += motion * Time.deltaTime;
    }
    public void ProjectileDamage(float dmg, Vector3 mot)
    {
        damage = dmg;
        motion = mot;
    }

}
