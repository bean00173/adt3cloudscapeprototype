using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceField : MonoBehaviour
{

    public float maxHealth;
    public float lifespan;

    [HideInInspector] public GameObject healthImg;
    [HideInInspector] public AlchemyCombat alchCombat;

    float health;
    float time;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        StartCoroutine(Decay());
    }

    // Update is called once per frame
    void Update()
    {
        //time += Time.deltaTime;

        healthImg.GetComponent<Image>().fillAmount = health / maxHealth;

        if (health <= 0 || time >= lifespan)
        {
            healthImg.SetActive(false);
            healthImg.GetComponent<Image>().fillAmount = 1.0f;
            alchCombat.Done();

            //ANIMATOR ?? SEPARATE DESTROY METHOD
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyProjectile") && other.GetComponent<ProjectileData>() != null)
        {
            //float a = ((health - other.GetComponent<ProjectileData>().damage) / maxHealth) + (time / lifespan);
            time += ((health - other.GetComponent<ProjectileData>().damage) / maxHealth) + (time / lifespan);
            health -= other.GetComponent<ProjectileData>().damage;
            Destroy(other.gameObject);
        }
    }

    private IEnumerator Decay()
    {
        time = 0;

        while(time < lifespan)
        {
            health = Mathf.Lerp(maxHealth, 0f, time / lifespan);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
