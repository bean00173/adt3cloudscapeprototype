using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    EnemyBehaviour eb;
    // Start is called before the first frame update
    void Start()
    {
        eb = GetComponentInParent<EnemyBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale = new Vector3(eb.GetHealth() / eb.enemy.maxHealth, this.transform.localScale.y, this.transform.localScale.z); // scales the health bar to be a % of original size based on % of health remaining
    }
}
