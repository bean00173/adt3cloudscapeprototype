using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class LoadingBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject progressBar, continuePrompt, bg;
    [SerializeField] private TextMeshProUGUI progressText;

    public List<Sprite> backgrounds = new List<Sprite>();

    private void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    private void ChooseBG()
    {
        if((LoadingData.sceneToLoad == "TowerTest" && GameManager.instance.currentScene.name == "LevelTest") || (LoadingData.sceneToLoad == "LevelTest"))
        {
            int bgIndex = 0;
            switch (GameManager.activeCharacter.Id)
            {
                case Character.CharacterId.seb: bgIndex = 0; break;
                case Character.CharacterId.rav: bgIndex = 1; break; // CHANGE WHEN MORE CHARACTERS
                case Character.CharacterId.abi: bgIndex = 2; break;
            }
            bg.GetComponent<Image>().sprite = backgrounds[bgIndex];
            
        }
        else
        {
            bg.GetComponent<Image>().sprite = backgrounds[backgrounds.Count - 1];
        }
    }

    IEnumerator LoadSceneAsync()
    {
        ChooseBG();

        if (LoadingData.sceneToLoad == "TowerTest" && GameManager.instance.currentScene.name == "LevelTest")
        {

            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("LevelTest");

            GameManager.instance.UpdateCurrentScene(LoadingData.sceneToLoad);

            //while (progressBar.transform.localScale != new Vector3(1,1,1))
            //{
            //    yield return new WaitForSeconds(1f);


            //}

            progressBar.transform.localScale = new Vector3(1, 1, 1);
            progressText.text = "(100%)";

            continuePrompt.gameObject.SetActive(true);

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

            if (Input.GetKeyDown(KeyCode.Return))
            {
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("LoadingScreen");

                GameManager.instance.deathListener = false;
                SceneManager.instance.ToggleObjects(true);
            }
            yield return null;
        }
        else if(LoadingData.sceneToLoad == "LevelTest" && GameManager.instance.currentScene.name == "LevelTest")
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("LevelTest");

            AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(LoadingData.sceneToLoad, LoadingData.mode);

            operation.allowSceneActivation = false;

            while (!operation.isDone)
            {
                progressBar.transform.localScale = new Vector3(operation.progress + 0.1f, 1, 1);
                progressText.text = "(" + (operation.progress * 100 + 10).ToString() + "%)";

                if (operation.progress >= .9f)
                {
                    GameManager.instance.UpdateCurrentScene(LoadingData.sceneToLoad);

                    continuePrompt.gameObject.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        operation.allowSceneActivation = true;
                        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("LoadingScreen");
                    }
                }
                yield return null;
            }
        }
        else
        {
            AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(LoadingData.sceneToLoad, LoadingData.mode);

            operation.allowSceneActivation = false;

            while (!operation.isDone)
            {
                progressBar.transform.localScale = new Vector3(operation.progress + 0.1f, 1, 1);
                progressText.text = "(" + (operation.progress * 100 + 10).ToString() + "%)";

                if (operation.progress >= .9f)
                {
                    GameManager.instance.UpdateCurrentScene(LoadingData.sceneToLoad);

                    continuePrompt.gameObject.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        operation.allowSceneActivation = true;
                        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("LoadingScreen");
                    }
                }
                yield return null;
            }
        }        
    }
}
