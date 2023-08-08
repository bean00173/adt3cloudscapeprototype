using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Transform healthBar;
    private float health;
    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.localScale = Vector3.one;
        health = this.GetComponent<PlayableCharacter>().healthModifier;
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {

        float healthScale = currentHealth / health;
        //healthBar.localScale = new Vector3(1f, healthScale, 1f);

        if(currentHealth <= 0)
        {
            Debug.Log("LOSE");
        }
    }

    public void takeDamage(float damage)
    {
        currentHealth -= damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            takeDamage(other.GetComponent<EnemyBehaviour>().enemy.damage);
        }
    }
}
