using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
        GameManager.instance.canPause = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
        GameManager.instance.canPause = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
