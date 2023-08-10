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
    }

    // Update is called once per frame
    void Update()
    {

        float healthScale = currentHealth / health;
        healthBar.localScale = new Vector3(healthScale, this.transform.localScale.y, this.transform.localScale.z);

        if (currentHealth <= 0)
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
        if (this.GetComponent<PlayerHealth>().enabled == false) return;
        else
        {
            if (other.CompareTag("Enemy"))
            {
                Debug.Log("Taking Damage");
                takeDamage(other.GetComponent<EnemyBehaviour>().enemy.damage);
            }
        }
    }
}
