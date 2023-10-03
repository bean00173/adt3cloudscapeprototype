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
    [Range(0, 100)] public int interruptResist;
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
    protected float health;

    [HideInInspector]
    public Transform goal;
    protected NavMeshAgent agent;
    protected BodyPartContainer bpc;
    protected Rigidbody rb;
    protected Animator ac;

    protected Transform player;
    protected PlayableCharacter pCharacter;
    protected float damage;

    protected Transform deathWeapon;
    protected bool specialAtk;
    protected Vector3 explodePos;

    protected Collider hit;

    protected bool atkReady;

    protected Transform projectileSpawn;

    protected int hitIndex;

    // Start is called before the first frame update
    public virtual void Start()
    {
        ac = this.GetComponentInChildren<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
        rb = this.GetComponent<Rigidbody>();
        health = enemy.maxHealth * GameManager.scaleIndex;
        goal = GameObject.Find("Player").transform; // changed to be GameObject.Find() because a prefab cannot store a reference to something in the heirarchy, and the enemies will be instantiated rather than already in heirarchy
        bpc = GameObject.Find("BodyParts").GetComponent<BodyPartContainer>();

        LimitSpawnVelocity();

        StartCoroutine(AtkCD());
    }

    // Update is called once per frame
    public virtual void Update()
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
        LookAtTarget();

        if (InRange(enemy.attackRadius) && !goal.GetComponent<PlayerHealth>().dead && atkReady)
        {
            agent.speed = 0;

            //Debug.LogError($"IN RANGE !! : {InRange()}");

            ac.SetTrigger("atkReady");

            agent.destination = this.transform.position; // stop agent at current position

            Vector3 delta = new Vector3(goal.position.x - this.transform.position.x, 0f, goal.position.z - this.transform.position.z);  // calculate x/z position difference between agent and player
            Quaternion target = Quaternion.LookRotation(delta); // create new target location based off of x/z diff

            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 5.0f); // slerp rotation based on time multiplied by a constant speed
        }
        else if (InRange(enemy.attackRadius) || goal.GetComponent<PlayerHealth>().dead)
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

    private void LookAtTarget()
    {
        Vector3 aimTarget = goal.position;
        Vector3 target = new Vector3(aimTarget.x - this.transform.position.x, 0f, aimTarget.z - this.transform.position.z);
        Quaternion aimDir = Quaternion.LookRotation(target);
        transform.rotation = Quaternion.Slerp(transform.rotation, aimDir, Time.deltaTime * 2.0f);
    }

    private void OnTriggerEnter(Collider other) 
    {
        bool specialAtk = false;
        if (other.CompareTag("Weapon") && health > 0) // if weapon trigger enter collider
        {
            //if(other.transform.root.GetChild(0).TryGetComponent(out GreatswordCombat gc))
            //{
            //    explodePos = gc.sword.position;
            //}
            //else
            //{
            //    explodePos = other.transform.position;
            //}

            if(other.GetComponent<Potion>() == null)
            {
                if (other.GetComponent<ProjectileData>() != null)
                {
                    player = other.GetComponent<ProjectileData>().bowArrow.transform.root.GetChild(0);

                    damage = other.GetComponent<ProjectileData>().damage;
                    specialAtk = other.GetComponent<ProjectileData>().special;
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
            }

            //Vector3 moveDirection = pCharacter.transform.GetComponent<PlayerMovement>().orientation.transform.position - this.transform.position;
            //rb.AddForce(moveDirection * -100f * pCharacter.knockBackModifier, ForceMode.Force);
            //StartCoroutine(ToString());

            // For Knockback Force, Animation is probably better

            if (other.GetComponent<GreatswordCombat>() == null)
            {
                TakeDamage(damage, player, other.transform, specialAtk);
            }
            else
            {
                TakeDamage(damage, player, other.transform.root.GetChild(0).GetComponent<PlayableCharacter>().ReturnCurrentCharacter().GetComponent<GreatswordCombat>().sword, false);
            }
            
        }

    }

    public void TakeDamage(float dmg, Transform player, Transform weapon, bool spc)
    {
        agent.speed = 0f;
        specialAtk = spc;
        if (GameManager.instance.RandomChance(100 - this.enemy.interruptResist)) ac.Play("Hit", -1, 0f);

        if(this.player == null) this.player = player;

        if (health <= dmg) // if current health will be 0 after swing, death
        {
            deathWeapon = weapon;
            EnemyDeath(player);
        }
        else
        {
            health -= dmg; // if not, remove health and lower speed because injured people aren't as fast as they once were
            agent.speed = enemy.speed * (health / enemy.maxHealth);

            deathWeapon = null;
            
            MeshRenderer[] mr = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in mr)
            {
                StartCoroutine(ColourDamage(renderer, .1f));
            }
        }
    }

    protected bool InRange(float range)
    {

        //Vector3 origin = checkPos.position;
        //Debug.DrawLine(origin, goal.position, Color.yellow);
        //Debug.DrawRay(origin, transform.forward * enemy.attackRadius, Color.green);

        Vector3 start = new Vector3(this.transform.position.x, 0f, this.transform.position.z);
        Vector3 end = new Vector3(goal.transform.position.x, 0f, goal.transform.position.z);

        Debug.DrawLine(start, end, Color.yellow);
        return Vector3.Distance(start, end) < range;

        //Collider[] colliders = Physics.OverlapSphere(this.transform.position, range);
        //foreach (Collider col in colliders)
        //{
        //    if(col.GetComponent<PlayableCharacter>() != null)
        //    {
        //        return true;
        //    }
        //}

        //return false;
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

        if(healthbar != null && healthbar.gameObject != null) Destroy(healthbar.gameObject); healthbar = null;

        health = 0;
        agent.speed = 0;
        CorpseExplode(); // call explosion
         // turn off renderer (not object as explosion references its transforms) to make invisible
    }

    private void CorpseExplode()
    {
        /*int x = Random.Range(2, 5);*/ // random number of body parts between 2 and 5

        /*bpc.DropLimbs(x, this.transform.position, enemy.enemyType);*/// instantiate generated number of body parts along with particles, blood etc. from BodyPartContainer.cs
        GameObject ragdoll = bpc.SpawnRagdoll(this.transform.position, this.transform.rotation, enemy.enemyType, specialAtk);
        bpc.HealthDrop(this.transform.position);
        if(deathWeapon != null)
        {
            if(deathWeapon.GetComponent<ProjectileData>() != null)
            {
                Destroy(deathWeapon.GetComponent<Rigidbody>());
                Destroy(deathWeapon.GetComponent<Collider>());
                deathWeapon.SetParent(ReturnSkeletonBase(ragdoll.transform));
            }
            else if(deathWeapon.GetComponent<Potion>() != null)
            {
                deathWeapon.SetParent(ragdoll.transform.GetChild(ragdoll.transform.childCount - 1));
            }
        }
        //Rigidbody[] rbs = ragdoll.GetComponentsInChildren<Rigidbody>();
        //foreach(Rigidbody rb in rbs)
        //{
        //    float upwardsForce = GameManager.activeCharacter.Id == Character.CharacterId.abi ? -10f : 50f;
        //    rb.AddExplosionForce(10000f, explodePos, 25f, upwardsForce);
        //}

        //Vector3 explosionPos = transform.position; // explosion origin is at enemy position 
        //Collider[] colliders = Physics.OverlapSphere(explosionPos, 2.0f); // finds all colliders within a radius
        //foreach (Collider hit in colliders)
        //{
        //    Rigidbody rb = hit.GetComponent<Rigidbody>();

        //    if (rb != null && rb.CompareTag("Drop"))
        //        rb.AddExplosionForce(Random.Range(5.0f, 10.0f) * 50f, explosionPos, 1.0f, 3.0f); // adds force to object rigidbody
        //}
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


    protected IEnumerator AtkCD()
    {
        yield return new WaitForSeconds(this.enemy.attackCd);
        atkReady = true;

        if (this.enemy.enemyType != enemyType.ranger)
        {
            hitIndex = hitIndex == 0 ? 1 : 0;
            ac.SetFloat("hitIndex", hitIndex);
        }
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

    private Transform ReturnSkeletonBase(Transform ragdoll)
    {
        foreach(Transform transform in ragdoll.GetChild(0))
        {
            if(transform.childCount > 0) return transform;
        }

        return null;
    }
}
