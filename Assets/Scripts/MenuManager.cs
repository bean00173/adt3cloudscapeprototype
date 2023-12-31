using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
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
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void LoadWithIntermission(string sceneName)
    {
        SceneManager.instance.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

}
