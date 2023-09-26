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

    public float maxHeight;
    public float timeToTarget;
    public float flameRadius;
    public float flameDuration;
    private float tickDmgModifier = .5f;
    public float explosionRadius;

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
            if (collision.gameObject.tag == "Ground")
            {
                StartCoroutine(FirePool());
                EndPos();
            }
        }
        else if(this._potionType == type.explosive)
        {
            if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Wall")
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

        Collider[] colliders = Physics.OverlapSphere(this.transform.position, explosionRadius);
        foreach (Collider col in colliders)
        {
            if(col.gameObject.GetComponent<EnemyBehaviour>() != null)
            {
                col.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(_damage, _player);
                Debug.Log($"{col.gameObject} took {_damage} damage!");
            }
        }

        Destroy(this.gameObject);
    }

    private IEnumerator FirePool()
    {
        onFlame.Invoke();
        yield return new WaitForSeconds(.75f);
        float time = 0;

        while(time < flameDuration)
        {
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, flameRadius);
            foreach(Collider col in colliders)
            {
                if (col.gameObject.GetComponent<EnemyBehaviour>() != null)
                {
                    col.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(_damage * tickDmgModifier, _player);
                    Debug.Log($"{col.gameObject} took {_damage * tickDmgModifier} flame damage!");
                }
            }
            time += 1.0f;
            yield return new WaitForSeconds(1.0f);
        }

        Destroy(this.gameObject);
    }

    private IEnumerator DoMoveHor()
    {
        float time = 0;
        float duration = timeToTarget;

        while(time < duration)
        {
            transform.position = Vector3.Lerp(startPos, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator DoMoveVert()
    {
        float time = 0;
        float duration = timeToTarget / 2;

        float initY = transform.position.y;


        while (time < duration)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(initY, maxHeight, time / duration), transform.position.z);
            time += Time.deltaTime;
            yield return null;
        }

        duration = timeToTarget / 2;

        while (time < duration)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(maxHeight, target.y, time / duration), transform.position.z);
            time += Time.deltaTime;
            yield return null;
        }
    }

}
