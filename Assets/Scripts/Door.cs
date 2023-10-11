using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Door : MonoBehaviour
{
    public GameObject prompt;
    public TextMeshProUGUI promptMsg;
    bool active;
    bool canInteract = true;

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.instance.currentScene.name == "TowerTest")
        {
            prompt = TowerManager.instance.doorPrompt;
            promptMsg = TowerManager.instance.doorPromptText;

        }
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.LogWarning("Door prompt active : " + active + ", Current Scene is : " + GameManager.instance.currentScene.name);

        if(GameManager.instance.currentScene.name == "TowerTest" && this.GetComponentInParent<TowerData>() != null)
        {
            if (this.GetComponentInParent<TowerData>().towerBeaten == true)
            {
                canInteract = false;
            }
        }
        //else if (GameManager.instance.currentScene.name == "LevelTest")
        //{
        //    canInteract = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>().enemiesLeft == 0 ? true : false;
        //}

        //if(TowerManager.instance.previousTower == null || (TowerManager.instance.previousTower == this.GetComponentInParent<TowerData>().gameObject))
        //{
        //    prompt.SetActive(active);
        //}

        //if (GameManager.instance.currentScene.name == "LevelTest" && promptMsg != null)
        //{
        //    promptMsg.text = GameManager.instance.towerFinished ? "Leave Tower ?" : "Next Floor ?";
        //}

        //if (GameManager.instance.currentScene.name == "TowerTest" && promptMsg != null)
        //{
        //    promptMsg.text = GameManager.instance.towerFinished ? "Dungeon Finished." : "Enter Tower ?";
        //}


        //if (active)
        //{
        //    if (Input.GetKeyDown(KeyCode.E) && canInteract)
        //    {
        //        canInteract = false;
        //    }
        //    else if (!canInteract)
        //    {
        //        GameManager.instance.readyToLoad = false;
        //    }
        //    else
        //    {
        //        GameManager.instance.readyToLoad = true;
        //    }
        //}
        //else
        //{
        //    GameManager.instance.readyToLoad = false;
        //}

    }

    private void PlayerAtDoor()
    {
        Debug.LogWarning("AT DOOR");

        if (GameManager.instance.currentScene.name == "LevelTest")
        {
            if(GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>().enemiesLeft == 0)
            {
                TowerManager.instance.PlayerAtDoor(this.gameObject);
                active = true;
            }
            else
            {
                TowerManager.instance.PlayerNotAtDoor();
                active = false;
            }
        }
        else
        {
            TowerManager.instance.PlayerAtDoor(this.gameObject);
            active = true;
        }
    }

    private void PlayerNotAtDoor()
    {
        Debug.LogWarning("LEFT DOOR");
        active = false;
        TowerManager.instance.PlayerNotAtDoor();
    }
}
