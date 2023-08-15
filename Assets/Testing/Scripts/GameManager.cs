using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Character activeCharacter;
    public static Tower towerData;
    public static int floorIndex;
    public static int score { get; private set; }

    private List<int> chance = new List<int>();

    public TextMeshProUGUI scoreText;
    public Transform promptText;

    [HideInInspector]
    public bool readyToLoad;

    [Header("All events below will play when player interacts with a door")]
    public UnityEvent onLevelLoad = new UnityEvent();

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
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ReturnCurrentScene().name == "LevelTest")
        {
            scoreText = GameObject.Find("score").GetComponent<TextMeshProUGUI>();

            if (scoreText.text != score.ToString())
            {
                scoreText.text = score.ToString();
            }
        }

        if (readyToLoad && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("SceneTransition");
            onLevelLoad.Invoke();
        }
    }

    public bool RandomChance(int prob) // takes in a probability (out of 100) and returns true or false if selected
    {
        //return UnityEngine.Random.value < prob; This is a much simpler way of doing things 

        for (int i = 0; i < 100; i++) // loop through 100 times, adding 1 to the list prob amount of times (if prob is 67, the list will contain 67 1's and 33 0's)
        {
            if (prob != 0)
            {
                chance.Add(1);
                prob--;
            }
            else
            {
                chance.Add(0);
            }
        }

        int choice = chance[Random.Range(0, 99)]; // randomly select an option from list

        chance.Clear();

        if (choice == 1) // if selected was a 1 return true, otherwise false
        {
            return true;
        }
        else
        {
            return false;
        }


    }

    public Vector3 SpawnPosition(int currentObject, int totalObjects, Vector3 pos, float radius)
    {
        float rad = 2 * Mathf.PI / totalObjects * currentObject + Random.Range(-1f, 1f); // divides radius by how many objects are instantiated and spaces them semi evenly (with a little sprinkle of randomisation)
        float vert = Mathf.Sin(rad); // calculates x,z coordinates based on angle from origin
        float hor = Mathf.Cos(rad);

        Vector3 spawnDir = new Vector3(hor, 0, vert); // creates vector with coords

        return pos + spawnDir * radius; // return spawnPos
    }

    public void ScoreUp(int update)
    {
        score += update;
    }

    public void StoreTowerData(Tower tower)
    {
        towerData = tower;
    }

    public void StoreCharacterData(Character character)
    {
        activeCharacter = character;
    }

    public Tower ReturnTowerData()
    {
        return towerData;
    }

    public Scene ReturnCurrentScene()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene();
    }

    public void NextFloor()
    {
        if(floorIndex == 0)
        {
            Debug.Log("Game Over");
            Light winLight = GameObject.Find("NextLevelSpotLight").GetComponent<Light>();
            winLight.enabled = true;
            winLight.intensity = Mathf.Lerp(0, 15, 3);
        }
        else
        {
            floorIndex += 1;
        }
        
    }

    public Transform ReturnUIComponent(string name)
    {
        foreach(Transform child in GameObject.Find("Canvas").transform)
        {
            if(child.name == name)
            {
                return child.transform;
            }
        }

        return null;
    }

}
