using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
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
    private GameObject youDied;
    public Transform promptText;

    bool deathListener;

    [HideInInspector]
    public bool readyToLoad;

    [HideInInspector]
    public bool timeSlow;

    //[Header("All events below will play when player interacts with a door")]
    [HideInInspector]
    public UnityEvent onLevelLoad = new UnityEvent();

    public delegate void TestDelegate();
    public TestDelegate slowTimeMethod;
    public TestDelegate dyingEnable;

    public bool capableOfDying;
    public bool towerFinished { get; private set; }

    public Scene currentScene { get; private set; }

    private string sceneName;

    public Transform fadeImg;

    public List<GameObject> towerPrefabs;

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
        slowTimeMethod = ResumeNormalTimeScale;
         
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Scene scene in GetOpenScenes())
        {
            if (GetOpenScenes().Length == 1)
            {
                currentScene = scene;
            }
        }

        if (currentScene.name == "TowerTest" || currentScene.name == "LevelTest")
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (currentScene.name == "LevelTest")
        {
            try
            {
                scoreText = ReturnUIComponent("score").GetComponent<TextMeshProUGUI>();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                scoreText = null;
            }

            try
            {
                youDied = ReturnUIComponent("YouDied").gameObject;
                if (!deathListener)
                {
                    youDied.GetComponent<InvokeEvent>().playableEvent.AddListener(DiedInGame);
                    deathListener = true;
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                youDied = null;
            }


            dyingEnable = CanDie;

            StartCoroutine(Timer(1f, dyingEnable));

            if (scoreText != null)
            {
                if (scoreText.text != score.ToString())
                {
                    scoreText.text = "Score : " + score.ToString();
                }
            }
        }

        if (readyToLoad && Input.GetKeyDown(KeyCode.E))
        {

            if (towerFinished)
            {
                //currentScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("TowerTest");
                SceneManager.instance.LoadScene("TowerTest", LoadSceneMode.Additive);

            }
            else
            {
                onLevelLoad.Invoke();
                //currentScene = UnityEngine.SceneManagement.SceneManager.("LevelTest");
                SceneManager.instance.LoadScene("LevelTest", LoadSceneMode.Additive);
            }
            Debug.Log("SceneTransition");
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

    public Scene[] GetOpenScenes()
    {
        int countLoaded = UnityEngine.SceneManagement.SceneManager.loadedSceneCount;
        Scene[] scenes = new Scene[countLoaded];

        for (int i = 0; i < countLoaded; i++)
        {
            scenes[i] = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
        }

        return scenes;
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

    public void CanDie()
    {
        capableOfDying = true;
    }

    public void NextFloor()
    {
        Light winLight = GameObject.Find("NextLevelSpotLight").GetComponent<Light>();

        winLight.enabled = true;
        winLight.intensity = Mathf.Lerp(0, 15, 3);

        if (floorIndex < towerData.floors.Count - 1)
        {
            Debug.Log("Next Floor");
            floorIndex += 1;
        }
        else
        {
            Debug.Log("Game Over");
            towerFinished = true;
        }

    }

    public void UpdateCurrentScene(string name)
    {
        currentScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(name);
    }

    public Transform ReturnUIComponent(string name)
    {
        GameObject[] sceneObjects = currentScene.GetRootGameObjects();
        List<GameObject> objects = new List<GameObject>(sceneObjects);
        Transform canvas = objects.Find((x) => x.name == "Canvas").transform;

        foreach (Transform child in canvas)
        {
            if (child.name == name)
            {
                return child.transform;
            }
        }

        return null;
    }

    public void SlowTime(float duration)
    {
        timeSlow = true;

        Time.timeScale = 0.5f;
        StartCoroutine(Timer(duration, slowTimeMethod));

    }

    public void ResumeNormalTimeScale()
    {
        timeSlow = false;
        Time.timeScale = 1;
    }


    public IEnumerator Timer(float time, TestDelegate method)
    {
        yield return new WaitForSecondsRealtime(time);
        method();
    }

    public void ResetGame()
    {
        capableOfDying = false;
        readyToLoad = false;
        towerFinished = false;
        floorIndex = 0;
        towerData = null;
        score = 0;
        deathListener = false;
    }

    private void DiedInGame()
    {
        SceneManager.instance.LoadScene("TowerTest", LoadSceneMode.Additive);

        deathListener = false;
    }

    public void ExitApplication()
    {
        StartCoroutine(CloseApplication());
    }

    private IEnumerator CloseApplication()
    {
        yield return new WaitForSeconds(1.5f);

        if(currentScene.name == "MainMenu")
        {
            fadeImg.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(1.5f);

        Debug.Log("Quit");
        Application.Quit();
    }

    public GameObject NextTower()
    {
        towerFinished = false;

        GameObject tower = towerPrefabs[Random.Range(0, towerPrefabs.Count - 1)];
        towerPrefabs.Remove(tower);

        return tower;
    }
}
