using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceField : MonoBehaviour
{

    public float maxHealth;
    public float lifespan;

    [HideInInspector] public GameObject healthImg;

    float health;
    float time;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        healthImg.GetComponent<Image>().fillAmount = health / maxHealth;

        if (health <= 0 || time >= lifespan)
        {
            healthImg.SetActive(false);
            healthImg.GetComponent<Image>().fillAmount = 1.0f;
            //ANIMATOR ?? SEPARATE DESTROY METHOD
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyProjectile") && other.GetComponent<ProjectileData>() != null)
        {
            health -= other.GetComponent<ProjectileData>().damage;
            Destroy(other.gameObject);
        }
    }

}
