using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

[System.Serializable]
public class Enemy // serializable class to store current enemy type & stats
{
    public enemyType enemyType;
    public float maxHealth;
    public float damage;
    public float speed;
    public float attackRadius;
    public float attackCd;
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
    public Transform healthbar;
    private float health;

    [HideInInspector]
    public Transform goal;
    NavMeshAgent agent;
    BodyPartContainer bpc;
    Rigidbody rb;
    Animator ac;

    Transform player;
    PlayableCharacter pCharacter;
    float damage;

    public Transform hitbox;
    Collider hit;

    bool atkReady = true;

    Transform projectileSpawn;

    // Start is called before the first frame update
    void Start()
    {
        ac = this.GetComponentInChildren<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
        rb = this.GetComponent<Rigidbody>();
        health = enemy.maxHealth * GameManager.scaleIndex;
        goal = GameObject.Find("Player").transform; // changed to be GameObject.Find() because a prefab cannot store a reference to something in the heirarchy, and the enemies will be instantiated rather than already in heirarchy
        bpc = GameObject.Find("BodyParts").GetComponent<BodyPartContainer>();
        hit = hitbox.GetComponent<Collider>();

        if (enemy.enemyType != enemyType.ranger)
        {
            hit.enabled = false;
            hit.isTrigger = true;
        }
        else
        {
            projectileSpawn = this.transform.GetChild(1);
        }
        

        LimitSpawnVelocity();
    }

    // Update is called once per frame
    void Update()
    {

        

        //if (atkReady)
        //{
        //    agent.speed = 0;
        //    ac.SetTrigger("atkReady");
        //}

        //if (!goal.GetComponent<PlayerHealth>().dead)
        //{

        //    agent.speed = enemy.speed;

        //    RangeCheck(); // check if near player
        //}
        //else
        //{
        //    agent.speed = 0;
        //}

        if (InRange() && !ac.GetCurrentAnimatorStateInfo(0).IsName("Armature_Attack") && !goal.GetComponent<PlayerHealth>().dead && atkReady)
        {
            agent.speed = 0;

            ac.SetTrigger("atkReady");

            agent.destination = this.transform.position; // stop agent at current position

            Vector3 delta = new Vector3(goal.position.x - this.transform.position.x, 0f, goal.position.z - this.transform.position.z);  // calculate x/z position difference between agent and player
            Quaternion target = Quaternion.LookRotation(delta); // create new target location based off of x/z diff

            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 5.0f); // slerp rotation based on time multiplied by a constant speed
        }
        else if (InRange() || goal.GetComponent<PlayerHealth>().dead)
        {
            agent.speed = 0;
        }
        else
        {
            agent.speed = enemy.speed;
            HandleMove();
        }

        ac.SetFloat("speed", agent.speed);


    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Weapon") && health > 0) // if weapon trigger enter collider
        {

            if(other.GetComponent<ProjectileData>() != null)
            {
                player = other.transform.root.GetChild(0);

                damage = other.GetComponent<ProjectileData>().damage;
                other.gameObject.SendMessage("StopMove");

                Debug.Log("Hit for " + damage + " Damage!");

                other.transform.SetParent(this.transform);
                other.GetComponent<ProjectileData>().enabled = false;
            }
            else
            {
                player = other.transform.root.GetChild(0); // collect collider root (player)
                pCharacter = player.GetComponent<PlayableCharacter>();

                damage = pCharacter.attackModifier;
                
                Debug.Log("Hit for " + pCharacter.attackModifier + " Damage!");
            }

            //Vector3 moveDirection = pCharacter.transform.GetComponent<PlayerMovement>().orientation.transform.position - this.transform.position;
            //rb.AddForce(moveDirection * -100f * pCharacter.knockBackModifier, ForceMode.Force);
            //StartCoroutine(ToString());

            // For Knockback Force, Animation is probably better

            if (health <= damage) // if current health will be 0 after swing, death
            {
                EnemyDeath(player);
            }
            else
            {
                health -= damage; // if not, remove health and lower speed because injured people aren't as fast as they once were
                agent.speed = agent.speed * (health / enemy.maxHealth);


                MeshRenderer[] mr = GetComponentsInChildren<MeshRenderer>();
                foreach(MeshRenderer renderer in mr)
                {
                    StartCoroutine(ColourDamage(renderer, .1f));
                }
            }
        }

    }

    private bool InRange()
    {
        Vector3 origin = new Vector3(this.transform.position.x, this.transform.position.y - (0.5f * this.transform.localScale.y), this.transform.position.z);
        Debug.DrawLine(origin, goal.position, Color.yellow);

        return Vector3.Distance(origin, goal.position) < enemy.attackRadius;
    }

    private void HandleMove()
    {
        agent.destination = goal.position; // set agent destination to be player target (set in inspector)
    }

    //private void RangeCheck()
    //{
    //    Vector3 origin = new Vector3(this.transform.position.x, this.transform.position.y - (0.5f * this.transform.localScale.y), this.transform.position.z);
    //    Debug.DrawLine(origin, goal.position, Color.yellow);
    //    if (Vector3.Distance(origin, goal.position) < enemy.attackRadius) // check if distance between this and player is less than attack radius
    //    {
    //        agent.destination = this.transform.position; // stop agent at current position

    //        Vector3 delta = new Vector3(goal.position.x - this.transform.position.x, 0f, goal.position.z - this.transform.position.z);  // calculate x/z position difference between agent and player
    //        Quaternion target = Quaternion.LookRotation(delta); // create new target location based off of x/z diff

    //        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 5.0f); // slerp rotation based on time multiplied by a constant speed

    //        atkReady = true;
    //    }
    //    else
    //    {
    //        agent.destination = goal.position; // set agent destination to be player target (set in inspector)
    //    }
    //}

    private void EnemyDeath(Transform player) // separate function for death method
    {
        GameManager.instance.ScoreUp(this.enemy.score);
        if (GameManager.instance.RandomChance(player.GetComponent<PlayableCharacter>().slowMoChance) && GameManager.instance.timeSlow == false) GameManager.instance.SlowTime(player.GetComponent<PlayableCharacter>().slowMoDuration);

        Destroy(healthbar.gameObject); healthbar = null;

        health = 0;
        agent.speed = 0;
        CorpseExplode(); // call explosion

        this.GetComponent<Renderer>().enabled = false; // turn off renderer (not object as explosion references its transforms) to make invisible
    }

    private void CorpseExplode()
    {
        int x = Random.Range(2, 5); // random number of body parts between 2 and 5

        bpc.DropLimbs(x, this.transform.position, enemy.enemyType);// instantiate generated number of body parts along with particles, blood etc. from BodyPartContainer.cs
        bpc.HealthDrop(this.transform.position);

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

    public float GetHealthPercentage()
    {
        return health / enemy.maxHealth;
    }

    private IEnumerator LimitSpawnVelocity()
    {
        yield return new WaitForSeconds(.25f);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void dohit()
    {
        if (atkReady)
        {
            atkReady = false;

            hit.enabled = true;
        }
        else
        {
            hit.enabled = false;
        }

        StartCoroutine(AtkCD());

    }

    public void DoRangeAtk()
    {
        if (atkReady)
        {
            atkReady = false;

            Vector3 aimTarget = goal.position;
            Vector3 target = new Vector3(aimTarget.x - this.transform.position.x, 0f, aimTarget.z - this.transform.position.z);
            Quaternion aimDir = Quaternion.LookRotation(target);
            target.Normalize();
            Vector3 velocity = target * 60f;
            GameObject projectile = Instantiate(hitbox.gameObject, projectileSpawn.position, aimDir, this.transform);
            projectile.GetComponent<ProjectileData>().ProjectileDamage(this.enemy.damage, velocity);
            StartCoroutine(AtkCD());
        }
    }

    private IEnumerator AtkCD()
    {
        yield return new WaitForSeconds(this.enemy.attackCd);
        atkReady = true;
    }

    public void SetupHealthBar(Transform parent)
    {
        healthbar.transform.SetParent(parent);
        healthbar.GetComponent<Healthbar>().eb = this;
    }

    private IEnumerator ColourDamage(MeshRenderer renderer, float fadeTime)
    {
        if (renderer == null) yield break;

        Color start = renderer.material.color != Color.white ? renderer.material.color : Color.white;

        float time = 0;

        while(time < fadeTime)
        {
            renderer.material.color = Color.Lerp(start, Color.red, time/fadeTime);
            time += Time.deltaTime;

            yield return null;
        }

        renderer.material.color = Color.red;

        time = 0;

        while (time < (fadeTime * 2))
        {
            renderer.material.color = Color.Lerp(Color.red, start, time / (fadeTime * 2));
            time += Time.deltaTime;
            yield return null;
        }

        renderer.material.color = start;

    }
}
