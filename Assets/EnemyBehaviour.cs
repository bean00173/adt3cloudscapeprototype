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

        this.GetComponent<Renderer>().enabled = false;
    }

    private void CorpseExplode()
    {
        for(int i = 0; i < 5; i++)
        {
            Instantiate(pile.GetComponent<BodyPartContainer>().RandomLimb(), spawnPos(i + 1), Quaternion.identity, pile) ;
        }

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 1.0f);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(Random.Range(5.0f, 10.0f), explosionPos, 1.0f, 3.0f); ;
        }

        Destroy(this.gameObject);
    }

    private Vector3 spawnPos(int i)
    {
        float rad = 2 * Mathf.PI / 5 * i;
        float vert = Mathf.Sin(rad);
        float hor = Mathf.Cos(rad);

        Vector3 spawnDir = new Vector3(hor, 0, vert);

        return transform.position + spawnDir * .5f;
    }

    public float GetHealth()
    {
        return health;
    }
}
