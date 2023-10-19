using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotCapturer : MonoBehaviour
{
    int index;
    public string characterName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            index++;
            ScreenCapture.CaptureScreenshot($"{Application.persistentDataPath}/{characterName}{index}.png");
            Debug.Log("PhotoTaken");
        }
    }

}
