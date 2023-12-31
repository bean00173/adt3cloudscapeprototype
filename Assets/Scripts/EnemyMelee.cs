using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyBehaviour
{
    public Transform hitCenter;
    public float shakeAmount = .5f;

    public ParticleSystem lHitPs, rHitPs;

    int hits;

    public void dohit()
    {
        //if (atkReady)
        //{
        //    atkReady = false;

        //    hit.enabled = true;
        //}
        //else
        //{
        //    hit.enabled = false;
        //}

        //if (atkReady)
        //{
        //    atkReady = false;

        //    this.GetComponent<CinemachineImpulseSource>().GenerateImpulseWithForce(shakeAmount);

        //    Collider[] colliders = Physics.OverlapSphere(hitCenter.position, enemy.attackRadius);
        //    foreach (Collider col in colliders)
        //    {
        //        if (col.transform.GetComponent<PlayerHealth>() != null)
        //        {
        //            col.transform.GetComponent<PlayerHealth>().TakeDamage(enemy.damage);
        //        }
        //    }
        //}

        atkReady = false;

        ParticleSystem ps = hitIndex == 0 ? rHitPs : lHitPs;
        ps.Play();

        this.GetComponent<CinemachineImpulseSource>().GenerateImpulseWithForce(shakeAmount);

        Collider[] colliders = Physics.OverlapSphere(hitCenter.position, enemy.attackRadius);
        foreach (Collider col in colliders)
        {
            if (col.transform.GetComponent<PlayerHealth>() != null)
            {
                col.transform.GetComponent<PlayerHealth>().TakeDamage(enemy.damage);
            }
        }

        StartCoroutine(AtkCD());
    }

    public void DoQuarterHit()
    {
        //if (atkReady)
        //{
        //    atkReady = false;

        //    hit.enabled = true;
        //}
        //else
        //{
        //    hit.enabled = false;
        //}

        //if (atkReady)
        //{
        //    atkReady = false;

        //    Collider[] colliders = Physics.OverlapSphere(hitCenter.position, enemy.attackRadius);
        //    foreach (Collider col in colliders)
        //    {
        //        if (col.transform.GetComponent<PlayerHealth>() != null)
        //        {
        //            col.transform.GetComponent<PlayerHealth>().TakeDamage(enemy.damage / 4);
        //        }
        //    }
        //}

        atkReady = false;

        ParticleSystem ps = hitIndex == 0 ? rHitPs : lHitPs;
        ps.Play();

        Collider[] colliders = Physics.OverlapSphere(hitCenter.position, enemy.attackRadius);
        foreach (Collider col in colliders)
        {
            if (col.transform.GetComponent<PlayerHealth>() != null)
            {
                col.transform.GetComponent<PlayerHealth>().TakeDamage(enemy.damage / 4);
            }
        }

    }

}
