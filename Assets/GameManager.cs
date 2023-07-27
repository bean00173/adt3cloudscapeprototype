using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private List<int> chance = new List<int>();
    private int probIndex;

    // Make this a singleton.
    public void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool RandomChance(int prob)
    {
        for (int i = 0; i < 100; i++)
        {
            if(prob != 0)
            {
                chance.Add(1);
                prob--;
            }
            else
            {
                chance.Add(0);
            }
        }

        int index = Random.Range(0, 99);
        Debug.Log(index);
        int choice = chance[index];

        chance.Clear();

        if (choice == 1)
        {
            return true;
        }
        else
        {
            return false;
        }

        
    }

}
