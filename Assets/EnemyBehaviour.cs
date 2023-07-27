using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Enemy
{
    public float health;
    public float damage;
    public float speed;
}

public class EnemyBehaviour : MonoBehaviour
{

    public Enemy enemy = new Enemy();

    public Transform goal;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = goal.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit");
        //if (other.CompareTag("Weapon"))
        //{
        //    PlayableCharacter pCharacter = other.GetComponent<PlayableCharacter>();

        //    if (enemy.health < other.GetComponent<PlayableCharacter>().attackModifier)
        //    {
        //        Destroy(this.gameObject);
        //    }
        //    else
        //    {
        //        enemy.health -= pCharacter.attackModifier;
        //    }
        //}
    }
}
