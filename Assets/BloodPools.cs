using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPools : MonoBehaviour
{
    public float poolSize;
    public GameObject bloodPrefab;
    ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    // Start is called before the first frame update
    void Start()
    {
        part = this.GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        int i = 0;

        while(i < numCollisionEvents)
        {
            GameObject pool = Instantiate(bloodPrefab, collisionEvents[i].intersection, Quaternion.identity);
            pool.transform.localScale = Vector3.one * poolSize;
        }
    }
}
