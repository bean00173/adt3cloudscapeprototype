using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnTimer : MonoBehaviour
{
    public bool readyToDespawn { get; private set; }
    public float despawnTime;
    public float time { get; private set; }
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
            readyToDespawn = true;
        }
    }

}
