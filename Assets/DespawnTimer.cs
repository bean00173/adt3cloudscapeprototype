using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnTimer : MonoBehaviour
{

    public float despawnTime;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        time = despawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;

        if(time <= 0)
        {
            Destroy(gameObject);
        }
    }
}
