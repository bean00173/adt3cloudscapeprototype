using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScreen : MonoBehaviour
{
    bool played;
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.playTutorial == false) GameManager.instance.notTutorial = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    public void Enable()
    {
        if (!played && GameManager.playTutorial)
        {
            Time.timeScale = 0f;
            GameManager.instance.notTutorial = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(true);
            }

            played = true;
        }
        else
        {
            GameManager.instance.notTutorial = true;
        }
    }

    public void Close()
    {
        Time.timeScale = 1f;
        GameManager.instance.notTutorial = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(false);
        }
    }
}
