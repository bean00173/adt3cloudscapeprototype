using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Enemy // serializable class to store current enemy type & stats
{
    public enemyType enemyType;
    public float maxHealth;
    public float damage;
    public float speed;
    public float attackRadius;
    public int score;
}

public enum enemyType //stores current enemy type so when DropLimbs() is called, the BodyPartContainer can decide which list of limbs to use
{
    grunt,
    brute,
    ranger
}

public class EnemyBehaviour : MonoBehaviour
{

    public Enemy enemy = new Enemy(); // creating new enemy
    
    private float health;

    Transform goal;
    NavMeshAgent agent;
    BodyPartContainer bpc;
    Rigidbody rb;
    Animator ac;

    public Transform hitbox;
    Collider hit;

    bool atkReady;

    int hitInt;

    // Start is called before the first frame update
    void Start()
    {
        ac = this.GetComponentInChildren<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
        rb = this.GetComponent<Rigidbody>();
        health = enemy.maxHealth;
        goal = GameObject.Find("Player").transform; // changed to be GameObject.Find() because a prefab cannot store a reference to something in the heirarchy, and the enemies will be instantiated rather than already in heirarchy
        bpc = GameObject.Find("BodyParts").GetComponent<BodyPartContainer>();
        hit = hitbox.GetComponent<Collider>();

        hit.enabled = false;
        hit.isTrigger = true;

        LimitSpawnVelocity();
    }

    // Update is called once per frame
    void Update()
    {

        ac.SetFloat("speed", agent.speed);

        if (atkReady)
        {
            agent.speed = 0;
            ac.SetTrigger("atkReady");
        }

        if (!goal.GetComponent<PlayerHealth>().dead)
        {

            agent.speed = enemy.speed;

            RangeCheck(); // check if near player
        }
        else
        {
            agent.speed = 0;
        }

    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Weapon")) // if weapon trigger enter collider
        {
            Transform player = other.transform.root.GetChild(0); // collect collider root (player)
            PlayableCharacter pCharacter = player.GetComponent<PlayableCharacter>();

            Debug.Log("Hit for " + pCharacter.attackModifier + " Damage!");

            //Vector3 moveDirection = pCharacter.transform.GetComponent<PlayerMovement>().orientation.transform.position - this.transform.position;
            //rb.AddForce(moveDirection * -100f * pCharacter.knockBackModifier, ForceMode.Force);
            //StartCoroutine(ToString());

            // For Knockback Force, Animation is probably better

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

    private void RangeCheck()
    {
        Vector3 origin = new Vector3(this.transform.position.x, this.transform.position.y - (0.5f * this.transform.localScale.y), this.transform.position.z);
        Debug.DrawLine(origin, goal.position, Color.yellow);
        if (Vector3.Distance(origin, goal.position) < enemy.attackRadius) // check if distance between this and player is less than attack radius
        {
            agent.destination = this.transform.position; // stop agent at current position

            Vector3 delta = new Vector3(goal.position.x - this.transform.position.x, 0f, goal.position.z - this.transform.position.z);  // calculate x/z position difference between agent and player
            Quaternion target = Quaternion.LookRotation(delta); // create new target location based off of x/z diff

            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 5.0f); // slerp rotation based on time multiplied by a constant speed

            atkReady = true;
        }
        else
        {
            agent.destination = goal.position; // set agent destination to be player target (set in inspector)
        }
    }

    private void EnemyDeath() // separate function for death method
    {
        GameManager.instance.ScoreUp(this.enemy.score);
        health = 0;
        agent.speed = 0;
        CorpseExplode(); // call explosion

        this.GetComponent<Renderer>().enabled = false; // turn off renderer (not object as explosion references its transforms) to make invisible
    }

    private void CorpseExplode()
    {
        int x = Random.Range(2, 5); // random number of body parts between 2 and 5

        bpc.DropLimbs(x, this.transform.position, enemy.enemyType);// instantiate generated number of body parts along with particles, blood etc. from BodyPartContainer.cs

        Vector3 explosionPos = transform.position; // explosion origin is at enemy position 
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 2.0f); // finds all colliders within a radius
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null && rb.CompareTag("Drop"))
                rb.AddExplosionForce(Random.Range(5.0f, 10.0f) * 50f, explosionPos, 1.0f, 3.0f); // adds force to object rigidbody
        }

        Destroy(this.gameObject); // once the explosion has occured, remove the enemy object as transforms no longer required
    }


    public float GetHealth()
    {
        return health;
    }

    private IEnumerator LimitSpawnVelocity()
    {
        yield return new WaitForSeconds(.25f);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void dohit()
    {
        if(hitInt == 0)
        {
            hit.enabled = true;
            hitInt = 1;
        }
        else
        {
            hitInt = 0;
            hit.enabled = false;
        }
        
    }
}
