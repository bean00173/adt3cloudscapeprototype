using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject progressBar, continuePrompt;
    [SerializeField] private TextMeshProUGUI progressText;

    private void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        if (LoadingData.sceneToLoad == "TowerTest")
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("LevelTest");

            GameManager.instance.UpdateCurrentScene(LoadingData.sceneToLoad);

            while (progressBar.transform.localScale != new Vector3(1,1,1))
            {
                yield return new WaitForSeconds(1f);

                progressBar.transform.localScale = new Vector3(.1f, 1, 1);
                progressText.text = "(" + (.1 * 100).ToString() + "%)";
            }

            continuePrompt.gameObject.SetActive(true);

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

            if (Input.GetKeyDown(KeyCode.Return))
            {
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("LoadingScreen");

                SceneManager.instance.ToggleObjects(true);
            }
            yield return null;
        }
        else
        {
            AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(LoadingData.sceneToLoad, UnityEngine.SceneManagement.LoadSceneMode.Additive);

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
