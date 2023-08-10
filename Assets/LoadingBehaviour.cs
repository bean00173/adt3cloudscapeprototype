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
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(LoadingData.sceneToLoad);

        operation.allowSceneActivation = false;

        while(!operation.isDone)
        {
            progressBar.transform.localScale = new Vector3(operation.progress, 1, 1);
            progressText.text = "(" + (operation.progress * 100 + 10).ToString() + "%)";

            if(operation.progress >= .9f)
            {
                continuePrompt.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    operation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}
