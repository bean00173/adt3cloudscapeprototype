using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class OnStep : MonoBehaviour
{
    public float stepShakeForce = 0.1f;
    AudioSource source;
    public EnemyBehaviour eb;

    // Start is called before the first frame update
    void Start()
    {
        source = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StepShake()
    {
        this.GetComponent<CinemachineImpulseSource>().GenerateImpulseWithForce(stepShakeForce);
        string clip = "";
        switch (eb.enemy.enemyType)
        {
            case enemyType.grunt: clip = "Grunt_Step"; break;
            case enemyType.brute: clip = "Brute_Step"; break;
            case enemyType.ranger: clip = "Ranger_Step"; break;
        }
        source.clip = AudioManager.instance.GetClip(AudioType.enemy, clip);   
        source.Play();
    }
}
