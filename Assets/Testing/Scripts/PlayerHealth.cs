using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Transform healthBar;
    private float health;
    private float currentHealth;

    Animator ac;

    bool dead;


    private void Awake()
    {
        if (GameManager.instance.ReturnCurrentScene().name == "ScriptTesting")
        {
            healthBar = GameManager.instance.ReturnUIComponent("health");
            this.enabled = true;
        }
        else
        {
            this.enabled = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        healthBar.localScale = Vector3.one;
        health = GameManager.activeCharacter.health;
        currentHealth = health;

        ac = this.GetComponent<PlayableCharacter>().ac;
    }

    // Update is called once per frame
    void Update()
    {
        if (ac != this.GetComponent<PlayableCharacter>().ac) ac = this.GetComponent<PlayableCharacter>().ac;

        if (!dead)
        {
            float healthScale = currentHealth / health;
            healthBar.localScale = new Vector3(healthScale, this.transform.localScale.y, this.transform.localScale.z);
        }

        if (currentHealth <= 0)
        {
            dead = true;
            Debug.Log("LOSE");
            ac.SetTrigger("Dead");
        }
    }

    public void takeDamage(float damage)
    {
        currentHealth -= damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.GetComponent<PlayerHealth>().enabled == false) return;
        else
        {
            if (other.CompareTag("Enemy"))
            {
                Debug.Log("Taking Damage");
                takeDamage(other.GetComponentInParent<EnemyBehaviour>().enemy.damage);
            }
        }
    }
}
