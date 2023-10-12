using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;
    public static scene currentScene;
    GameObject[] allObjects; 
    List<GameObject> objectsToDisable = new List<GameObject>();


    public enum scene
    {
        loading,
        level,
        openWorld,
        menu,
        gameWin
    }

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string sceneName, LoadSceneMode mode)
    {
        Debug.Log("Loading");

        if(GameManager.instance.currentScene.name == "TowerTest")
        {
            GatherObjects();
            ToggleObjects(false);
        }
        else if(sceneName == "TowerTest")
        {
            ToggleObjects(true);
        }

        LoadingData.sceneToLoad = sceneName;
        LoadingData.mode = mode;
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoadingScreen", UnityEngine.SceneManagement.LoadSceneMode.Additive);

    }

    public void ToggleObjects(bool toggle) 
    {
        foreach(GameObject a in objectsToDisable){
            if(a != null)
            {
                a.SetActive(toggle);
            }
        }
    }

    public void GatherObjects()
    {
        allObjects = FindObjectsOfType<GameObject>();
        objectsToDisable = new List<GameObject>(allObjects);

        foreach(GameObject a in allObjects)
        {
            if(a.name == "SceneManager" || a.name == "GameManager" || a.name == "CharacterManager" || a.name == "[Debug Updater]" || a.name == "AudioManager")
            {
                objectsToDisable.Remove(a);
            }
        }
    }

    public void UpdateCurrentScene(string scene)
    {
        switch (scene)
        {
            case "MainMenu": currentScene = SceneManager.scene.menu; break;
            case "LevelTest": currentScene = SceneManager.scene.level; break;
            case "TowerTest": currentScene = SceneManager.scene.openWorld; break;
            case "LoadingScreen": currentScene = SceneManager.scene.loading; break;
            case "GameWin": currentScene = SceneManager.scene.gameWin; break;
        }
    }
}
