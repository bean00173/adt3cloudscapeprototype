using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyBehaviour
{
    public Transform hitCenter;

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

        if (atkReady)
        {
            atkReady = false;

            GameObject sphereVisualise = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), hitCenter.position, Quaternion.identity);
            sphereVisualise.transform.localScale = Vector3.one * 2f * enemy.attackRadius;
            sphereVisualise.GetComponent<Collider>().enabled = false;
            sphereVisualise.GetComponent<Renderer>().material = null;

            Collider[] colliders = Physics.OverlapSphere(hitCenter.position, enemy.attackRadius);
            foreach (Collider col in colliders)
            {
                if (col.transform.GetComponent<PlayerHealth>() != null)
                {
                    col.transform.GetComponent<PlayerHealth>().TakeDamage(enemy.damage);
                }
            }
        }

        StartCoroutine(WaitTillNext());

        hitIndex = hitIndex == 0 ? 1 : 0;
        ac.SetFloat("hitIndex", hitIndex);

        StartCoroutine(AtkCD());
    }

    private IEnumerator WaitTillNext()
    {
        yield return new WaitUntil(() => ac.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
    }
}
