using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Door : MonoBehaviour
{
    public GameObject prompt;
    public TextMeshProUGUI promptMsg;
    bool active;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        prompt.SetActive(active);

        if (GameManager.instance.currentScene.name == "LevelTest" && promptMsg != null)
        {
            promptMsg.text = GameManager.instance.towerFinished ? "Leave Tower ?" : "Next Floor ?";
        }

        if (GameManager.instance.currentScene.name == "TowerTest" && promptMsg != null)
        {
            promptMsg.text = GameManager.instance.towerFinished ? "Dungeon Finished." : "Enter Tower ?";
        }


        if (active)
        {
            GameManager.instance.readyToLoad = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                this.enabled = false;
            }
        }
    }

    private void PlayerAtDoor()
    {
        Debug.Log("AT DOOR");
        active = true;
    }

    private void PlayerNotAtDoor()
    {
        Debug.Log("LEFT DOOR");
        active = false;
    }
}
