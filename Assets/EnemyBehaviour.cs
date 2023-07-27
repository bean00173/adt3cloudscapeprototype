using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Enemy
{
    public float maxHealth;
    public float damage;
    public float speed;
}

public class EnemyBehaviour : MonoBehaviour
{

    public Enemy enemy = new Enemy();
    
    private float health;

    public Transform goal;
    public Transform pile;
    NavMeshAgent agent;

    private Vector3 spawnPos;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = enemy.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = goal.position;
        spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + 1.0f, this.transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            Transform player = other.transform.parent;
            PlayableCharacter pCharacter = player.GetComponent<PlayableCharacter>();

            if (health <= pCharacter.attackModifier)
            {
                EnemyDeath();
            }
            else
            {
                health -= pCharacter.attackModifier;
                agent.speed = agent.speed * (health / enemy.maxHealth);
            }
        }

    }

    private void EnemyDeath()
    {
        Debug.Log("DEAD");
        health = 0;
        agent.speed = 0;
        CorpseExplode();
    }

    private void CorpseExplode()
    {
        for(int i = 0; i < 5; i++)
        {
            Instantiate(pile.GetComponent<BodyPartContainer>().limb, spawnPos, Quaternion.identity, pile) ;
        }

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 1.0f);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(2.0f, explosionPos, 1.0f, 3.0f);
        }
    }

    public float GetHealth()
    {
        return health;
    }
}
