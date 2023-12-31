using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Cinemachine;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Character activeCharacter;
    public static float characterHealth;
    public static float scaleIndex;
    public static int towersBeaten;
    public static Tower towerData;
    public static int floorIndex;
    public bool floorBeaten { get; private set; }

    public bool canPause;
    public bool paused { get; private set; }
    public static int score { get; private set; }
    public static int totalScore { get; private set; }

    private List<int> chance = new List<int>();

    public TextMeshProUGUI scoreText;
    private GameObject youDied, backToMenu;
    public Transform promptText;

    [HideInInspector] public bool deathListener;

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

    public bool towerLeft;
    public bool towerFinished { get; private set; }

    public Scene currentScene { get; private set; }

    private string sceneName;

    public Transform fadeImg;

    public List<GameObject> towerPrefabs;

    private bool levelListenerAdded, towerListenerAdded, menuListenerAdded;

    GameObject pauseMenu;

    public Transform toggle;

    public bool dead;
    public bool notTutorial;
    public static bool playTutorial = true;


    public bool victory, defeat;
    public float gruntsDefeated, brutesDefeated, rangersDefeated, specialsDefeated, totalDefeated, totalDeaths;


    // Make this a singleton.
    public void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            //Destroy(this.gameObject);
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
        scaleIndex = 1;


        toggle.GetComponent<Toggle>().onValueChanged.AddListener(PlayTutorial);
    }

    // Update is called once per frame
    void Update()
    {
        if(toggle && toggle.GetComponent<Toggle>().isOn != playTutorial) toggle.GetComponent<Toggle>().isOn = playTutorial;

        switch (currentScene.name)
        {
            case "TowerTest": canPause = true; break;
            case "LevelTest": canPause = floorBeaten ? true : false; break;
            default:  canPause = false; break;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && canPause && notTutorial)
        {
            paused = true;
            canPause = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            pauseMenu = GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject;

            if((currentScene.name == "TowerTest" && !towerListenerAdded) || (currentScene.name == "LevelTest" && !levelListenerAdded))
            {
                Button resume = pauseMenu.transform.Find("Main/ResumeBtn/Resume").GetComponent<Button>();
                Button exit = pauseMenu.transform.Find("Main/Leave/YesBtn/Yes").GetComponent<Button>();
                resume.onClick.AddListener(Resume);
                exit.onClick.AddListener(BackToMenu);

                StoreMixerInfo(pauseMenu.transform.Find("Options").transform);
                pauseMenu.transform.Find("Options").gameObject.SetActive(false);

                switch (currentScene.name)
                {
                    case "TowerTest": towerListenerAdded = true; break;
                    case "LevelTest": levelListenerAdded = true; break;
                }
            }

            pauseMenu.SetActive(true);

            Time.timeScale = 0;
        }

        if (towerLeft) towerFinished = false;


        foreach (Scene scene in GetOpenScenes())
        {
            if (GetOpenScenes().Length == 1)
            {
                currentScene = scene;
            }
        }

        Debug.Log($"Dead ? {dead}");

        if ((currentScene.name == "TowerTest" || (currentScene.name == "LevelTest" && !dead)) && !paused && notTutorial)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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
                    Button dead = youDied.GetComponentInChildren<Button>();
                    dead.onClick.AddListener(GameManager.instance.DiedInGame);
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
                    scoreText.text = score.ToString();
                }
            }
        }

        if (readyToLoad && Input.GetKeyDown(KeyCode.E))
        {
            floorBeaten = false;
            readyToLoad = false;

            if (towerFinished)
            {
                if(TowerManager.instance.towerIndex + 2 > TowerManager.instance.GetIslandCount())
                {
                    SceneManager.instance.LoadScene("GameWin", LoadSceneMode.Single);
                    victory = true;
                }
                else
                {
                    //currentScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("TowerTest");
                    SceneManager.instance.LoadScene("TowerTest", LoadSceneMode.Additive);
                    levelListenerAdded = false;
                    score = 0;
                }
            }
            else
            {
                onLevelLoad.Invoke();
                //currentScene = UnityEngine.SceneManagement.SceneManager.("LevelTest");
                SceneManager.instance.LoadScene("LevelTest", LoadSceneMode.Additive);
            }
            Debug.Log("SceneTransition");
        }

        if (currentScene.name == "GameWin")
        {
            backToMenu = ReturnUIComponent("Results").Find("Btn").gameObject;
            if (!menuListenerAdded)
            {
                Button leave = backToMenu.GetComponentInChildren<Button>();
                leave.onClick.AddListener(ResetGame);
                menuListenerAdded = true;
            }
        }
    }

    public void StoreMixerInfo(Transform main)
    {
        Slider sfxSlider = main.Find("soundbg/SFXSlider").GetComponent<Slider>();
        sfxSlider.onValueChanged.AddListener(AudioManager.instance.SetSfxVol);

        Slider musicSlider = main.Find("soundbg/MusicSlider").GetComponent<Slider>();
        musicSlider.onValueChanged.AddListener(AudioManager.instance.SetMusicVol);

        Slider ambienceSlider = main.Find("soundbg/AmbienceSlider").GetComponent<Slider>();
        ambienceSlider.onValueChanged.AddListener(AudioManager.instance.SetAmbienceVol);

        Slider playerSlider = main.Find("soundbg/PlayerSlider").GetComponent<Slider>();
        playerSlider.onValueChanged.AddListener(AudioManager.instance.SetPlayerVol);

        Slider enemySlider = main.Find("soundbg/EnemySlider").GetComponent<Slider>();
        enemySlider.onValueChanged.AddListener(AudioManager.instance.SetEnemyVol);

        Slider masterSlider = main.Find("soundbg/MasterSlider").GetComponent<Slider>();
        masterSlider.onValueChanged.AddListener(AudioManager.instance.SetMasterVol);
    }

    private void PlayTutorial(bool play)
    {
        playTutorial = play;
    }

    public void BackToMenu()
    {
        Debug.LogError("Baii");

        towerListenerAdded = false;
        levelListenerAdded = false;

        paused = false;
        canPause = true;
        Time.timeScale = 1;
        SceneManager.instance.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        canPause = true;
        paused = false;
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
        floorBeaten = true;
        Light winLight = GameObject.Find("NextLevelSpotLight").GetComponent<Light>();

        winLight.enabled = true;
        winLight.intensity = Mathf.Lerp(0, 7.5f, 3);

        if (floorIndex < towerData.floors.Count - 1)
        {
            Debug.Log("Next Floor");
            floorIndex += 1;
        }
        else
        {
            
            towerFinished = true;
            towersBeaten += 1;

            CharacterManager.instance.ResetCharStatus();

            totalScore += score;

            floorIndex = 0;

            characterHealth = 1;

            UpdateScale();

            Debug.Log("Tower Beaten, Scale Index = " + scaleIndex);
        }

    }

    private void UpdateScale()
    {
        scaleIndex = 1 + (.1f * towersBeaten);
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
        activeCharacter = null;
        characterHealth = 1;
        scaleIndex = 1;
        towersBeaten = 0;
        towerData = null;
        floorIndex = 0;
        score = 0;
        totalScore = 0;
    }

    private void DiedInGame()
    {
        if(!CharacterManager.instance.AnyCharAlive())
        {
            SceneManager.instance.LoadScene("GameWin", LoadSceneMode.Single);
            defeat = true;
            //ResetGame();
        }
        else
        {
            GameManager.instance.towerFinished = false;
            SceneManager.instance.LoadScene("TowerTest", LoadSceneMode.Additive);
            score = 0;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        totalScore -= 100;

        totalScore = totalScore < 0 ? 0 : totalScore;
        totalDeaths++;

        dead = false;
        
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

    //public GameObject NextTower()
    //{
    //    towerFinished = true;
    //    GameObject tower = TowerManager.instance.GetNextTower(towerPrefabs[TowerManager.instance.towerIndex]);
    //    return tower;
    //}

    public float ReturnCharacterHealth()
    {
        if(floorIndex == 0)
        {
            return 1;
        }
        else
        {
            Debug.Log("CHARACTER HEALTH LEFTOVER : " + characterHealth);
            return characterHealth;
        }
    }

    public IEnumerator DoCameraShake(CinemachineFreeLook cfl, float shakeDuration)
    {
        cfl.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 5f;
        yield return new WaitForSeconds(shakeDuration);
        cfl.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
    }
}
