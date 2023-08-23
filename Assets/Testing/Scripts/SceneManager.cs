using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;

    GameObject[] allObjects; 
    List<GameObject> objectsToDisable = new List<GameObject>();

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
        
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log("Loading");

        if(sceneName != "TowerTest")
        {
            GatherObjects();
            ToggleObjects(false);
        }
        //else
        //{
        //    ToggleObjects(true);
        //}

        LoadingData.sceneToLoad = sceneName;
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoadingScreen", UnityEngine.SceneManagement.LoadSceneMode.Additive);

    }

    public void ToggleObjects(bool toggle)
    {
        foreach(GameObject a in objectsToDisable){
            a.SetActive(toggle);
        }
    }

    public void GatherObjects()
    {
        allObjects = FindObjectsOfType<GameObject>();
        objectsToDisable = new List<GameObject>(allObjects);

        foreach(GameObject a in allObjects)
        {
            if(a.name == "SceneManager" || a.name == "GameManager" || a.name == "CharacterManager" || a.name == "[Debug Updater]")
            {
                objectsToDisable.Remove(a);
            }
        }
    }
}
