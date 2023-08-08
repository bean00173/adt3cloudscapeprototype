using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public List<GameObject> enemies = new List<GameObject>();
    public int enemyCount;
}
[System.Serializable]
public class Floor
{
    public List<Wave> waves = new List<Wave>();
}
[System.Serializable]
public class Tower
{
    public List<Floor> floors = new List<Floor>();
}

public class LevelData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
