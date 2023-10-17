using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class OnStep : MonoBehaviour
{
    public float stepShakeForce = 0.1f;
    AudioSource source;
    public EnemyBehaviour eb;
    [HideInInspector] public UnityEvent onStep = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        source = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StepSound()
    {
        string clip = "";
        if (eb != null)
        {
            //this.GetComponent<CinemachineImpulseSource>().GenerateImpulseWithForce(stepShakeForce);
            switch (eb.enemy.enemyType)
            {
                case enemyType.grunt: clip = "Grunt_Step"; break;
                case enemyType.brute: clip = "Brute_Step"; break;
                case enemyType.ranger: clip = "Ranger_Step"; break;
            }
            source.clip = AudioManager.instance.GetClip(AudioType.enemy, clip);
            source.Play();
        }
        else
        {
            onStep.Invoke();
            //switch (this.GetComponent<PlayableCharacter>().currentCharacter)
            //{
            //    case Character.CharacterId.seb: clip = "Seb_Step"; break;
            //    case Character.CharacterId.abi: clip = "Abi_Step"; break;
            //    case Character.CharacterId.rav: clip = "Rav_Step"; break;
            //}
            //this.GetComponent<SoundHandler>().PlaySound(clip);
        }
    }
}
