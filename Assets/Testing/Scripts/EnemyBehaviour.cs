using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Enemy // serializable class to store current enemy stats
{
    public float maxHealth;
    public float damage;
    public float speed;
}

public class EnemyBehaviour : MonoBehaviour
{

    public Enemy enemy = new Enemy(); // creating new enemy
    
    private float health;

    public Transform goal;
    public Transform pile;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = enemy.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = goal.position; // set agent destination to be player target (set in inspector)
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Weapon")) // if weapon trigger enter collider
        {
            Transform player = other.transform.root; // collect collider root (player)
            PlayableCharacter pCharacter = player.GetComponent<PlayableCharacter>();

            if (health <= pCharacter.attackModifier) // if current health will be 0 after swing, death
            {
                EnemyDeath();
            }
            else
            {
                health -= pCharacter.attackModifier; // if not, remove health and lower speed because injured people aren't as fast as they once were
                agent.speed = agent.speed * (health / enemy.maxHealth);
            }
        }

    }

    private void EnemyDeath() // separate function for death method
    {
        health = 0;
        agent.speed = 0;
        CorpseExplode(); // call explosion

        this.GetComponent<Renderer>().enabled = false; // turn off collider (not enemy as explosion references enemy transforms)
    }

    private void CorpseExplode()
    {
        int x = Random.Range(5, 8);
        for (int i = 0; i < x; i++) // instantiate 5 body parts in a ring around player
        {
            Instantiate(pile.GetComponent<BodyPartContainer>().RandomLimb(), spawnPos(i + 1, x), Quaternion.identity, pile);
        }

        Vector3 explosionPos = transform.position; // explosion origin is at enemy position 
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 2.0f); // finds all colliders within a radius
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(Random.Range(5.0f, 10.0f), explosionPos, 1.0f, 3.0f); // adds force to object rigidbody
        }

        Destroy(this.gameObject);
    }

    private Vector3 spawnPos(int i, int x) 
    {
        float rad = 2 * Mathf.PI / x * i + Random.Range(-1f, 1f); // divides radius by how many objects are instantiated and spaces them semi evenly (with a little sprinkle of randomisation)
        float vert = Mathf.Sin(rad); // calculates x,z coordinates based on angle from origin
        float hor = Mathf.Cos(rad);

        Vector3 spawnDir = new Vector3(hor, 0, vert); // creates vector with coords

        return transform.position + spawnDir * .5f; // return spawnPos
    }

    public float GetHealth()
    {
        return health;
    }
}
