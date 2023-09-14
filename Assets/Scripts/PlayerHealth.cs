using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public Transform healthBar;
    [Tooltip("Insert the Width of the HealthBar Component Here")]
    public float healthBarMax, healthBarMin;
    private float health;
    private float currentHealth;

    public Transform blood;

    Animator ac;

    public UnityEvent onDeath = new UnityEvent();

    public bool dead { get ; private set; }


    private void Awake()
    {
        if (GameManager.instance.currentScene.name == "LevelTest")
        {
            //healthBar = GameManager.instance.ReturnUIComponent("health");
            this.enabled = true;
        }
        else
        {
            this.enabled = false;
        }

        //healthBar.localScale = Vector3.one;
        health = CharacterManager.instance.GetCurrentCharacter(GetComponent<PlayableCharacter>().currentCharacter).health;
        currentHealth = health * GameManager.instance.ReturnCharacterHealth();

        ac = this.GetComponent<PlayableCharacter>().ac;
    }
    // Start is called before the first frame update
    void Start()
    {
        //healthBar.localScale = Vector3.one;
        //health = CharacterManager.instance.GetCurrentCharacter(GetComponent<PlayableCharacter>().currentCharacter).health;
        //currentHealth = health;

        //ac = this.GetComponent<PlayableCharacter>().ac;
    }

    // Update is called once per frame
    void Update()
    {
        if (ac != this.GetComponent<PlayableCharacter>().ac) ac = this.GetComponent<PlayableCharacter>().ac;

        if (!dead)
        {
            float healthScale = currentHealth / health;
            float hbShift = (healthBarMax - healthBarMin) - (healthBarMax - healthBarMin * healthScale);
            Debug.Log(hbShift);
            if (healthScale >= 0)
            {
                healthBar.GetComponent<RectTransform>().position = new Vector2(hbShift, healthBar.GetComponent<RectTransform>().position.y);
            }
        }

        if (currentHealth <= 0 && !dead && GameManager.instance.capableOfDying)
        {
            dead = true;
            Debug.Log("LOSE");
            ac.Play("Armature_Death", -1, 0f);
            this.GetComponent<PlayerMovement>().MoveInterrupt(false);
            healthBar.localScale = new Vector3(0f, this.transform.localScale.y, this.transform.localScale.z);
            GameObject splatter = Instantiate(blood.gameObject, this.transform.position, Quaternion.identity, GameObject.Find("BloodContainer").transform);
            splatter.transform.localScale = Vector3.one * 5;

            GameManager.instance.ReturnUIComponent("YouDied").gameObject.SetActive(true);
        }
    }

    public void takeDamage(float damage)
    {
        Debug.Log(currentHealth);
        currentHealth -= damage * GameManager.scaleIndex;
    }

    public void Heal()
    {
        Debug.Log(currentHealth);

        if(currentHealth < health)
        {
            if((currentHealth += this.health * 0.05f) >= health)
            {
                currentHealth = health;
            }
            else
            {
                currentHealth += this.health * 0.05f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.GetComponent<PlayerHealth>().enabled == false) return;
        else
        {
            if (other.CompareTag("EnemyHit"))
            {
                Debug.Log(other.gameObject);
                takeDamage(other.GetComponentInParent<EnemyBehaviour>().enemy.damage);
                //Debug.Log("Taking " + other.GetComponentInParent<EnemyBehaviour>().enemy.damage + " Damage");
            }
            else if (other.CompareTag("Projectile"))
            {
                Debug.Log(other.gameObject);
                takeDamage(other.GetComponentInParent<ProjectileData>().damage);
                Destroy(other.transform.parent.gameObject);
            }
            else if (other.CompareTag("HealthOrb"))
            {
                Debug.Log(other.gameObject);
                Heal();
                Destroy(other.transform.parent.gameObject) ;
            }
        }
    }

    public float ReturnHealth()
    {
        return currentHealth / health;
    }
}
