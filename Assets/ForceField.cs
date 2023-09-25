using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{

    public float health;
    public float lifespan;

    float time;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (health <= 0 || time >= lifespan)
        {
            //ANIMATOR ?? SEPARATE DESTROY METHOD
            Destroy(this.gameObject);
        }       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyProjectile") && other.GetComponent<ProjectileData>() != null)
        {
            health -= other.GetComponent<ProjectileData>().damage;
            Destroy(other.gameObject);
        }
    }

}
