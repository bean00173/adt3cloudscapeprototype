using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public enum type
{
    explosive,
    flame
}

public class Potion : MonoBehaviour
{
    private type _potionType;
    private float _damage;
    private Vector3 target;

    public GameObject explodePS, flamePS;

    public float timeToTarget;
    public float flameRadius;
    public float flameDuration;
    private float tickDmgModifier = .5f;
    public float explosionRadius;

    private bool special;

    public UnityEvent onExplode = new UnityEvent();
    public UnityEvent onFlame = new UnityEvent();

    [HideInInspector] public Transform _player;

    Vector3 startPos;
    Vector3 endPos;
    bool hit;
    
    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;

        //StartCoroutine(DoMoveHor());
        //StartCoroutine(DoMoveVert());
    }

    // Update is called once per frame
    void Update()
    {
        if (hit)
        {
            this.transform.position = endPos;
        }
    }

    public void StoreData(type potionType, float damage, Transform player, Vector3 target, float time)
    {
        this._damage = damage;
        this._potionType = potionType;
        this._player = player;
        this.target = target;
        this.timeToTarget = time;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(this._potionType == type.flame)
        {
            special = true;

            if (collision.gameObject.tag == "Ground")
            {
                StartCoroutine(FirePool());
                EndPos();
            }
        }
        else if(this._potionType == type.explosive)
        {
            if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ragdoll")
            {
                Explode();
                EndPos();
            }
        }
        
    }

    private void EndPos()
    {
        hit = true;
        endPos = this.transform.position;
    }

    private void Explode()
    {

        onExplode.Invoke();

        GameObject ps = Instantiate(explodePS, this.transform.position, Quaternion.identity);
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(ps, UnityEngine.SceneManagement.SceneManager.GetSceneByName("LevelTest"));

        this.GetComponent<CinemachineImpulseSource>().GenerateImpulseWithForce(1f);
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, explosionRadius);
        foreach (Collider col in colliders)
        {
            if(col.gameObject.GetComponent<EnemyBehaviour>() != null)
            {
                col.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(_damage, _player, this.transform, special);
                Debug.Log($"{col.gameObject} took {_damage} damage!");
            }
            else if(col.gameObject.GetComponent<PlayerHealth>() != null)
            {
                col.gameObject.GetComponent<PlayerHealth>().TakeDamage(_damage);
            }
        }

        Destroy(this.gameObject);
    }

    private IEnumerator FirePool()
    {
        onFlame.Invoke();

        this.transform.eulerAngles = Vector3.zero;
        GameObject ps = Instantiate(flamePS, this.transform.position, Quaternion.identity);
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(ps, UnityEngine.SceneManagement.SceneManager.GetSceneByName("LevelTest"));

        //yield return new WaitForSeconds(.75f);
        float time = 0;

        while(time < flameDuration)
        {
            this.GetComponent<CinemachineImpulseSource>().GenerateImpulseWithForce(.1f);
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, flameRadius);
            foreach(Collider col in colliders)
            {
                if (col.gameObject.GetComponent<EnemyBehaviour>() != null)
                {
                    col.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(_damage * tickDmgModifier, _player, this.transform, special);
                    Debug.LogError($"{col.gameObject} took {_damage * tickDmgModifier} flame damage!");
                }
                else if (col.gameObject.GetComponent<PlayerHealth>() != null)
                {
                    col.gameObject.GetComponent<PlayerHealth>().TakeDamage(_damage);
                }
            }
            time += 1.0f;
            yield return new WaitForSeconds(1.0f);
        }

        Destroy(this.gameObject);
    }
}
