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
        
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(active);

        if(GameManager.instance.currentScene.name == "TowerTest")
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
            if (Input.GetKeyDown(KeyCode.E) && canInteract)
            {
                this.enabled = false;
            }
            else if (!canInteract)
            {
                GameManager.instance.readyToLoad = false;
            }
            else
            {
                GameManager.instance.readyToLoad = true;
            }
        }
        else
        {
            GameManager.instance.readyToLoad = false;
        }
    }

    private void PlayerAtDoor()
    {
        Debug.Log("AT DOOR");
        if (GameManager.instance.currentScene.name == "LevelTest")
        {
            active = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>().enemiesLeft == 0 ? true : false;
        }
        else
        {
            active = true;
        }
    }

    private void PlayerNotAtDoor()
    {
        Debug.Log("LEFT DOOR");
        active = false;
    }
}
