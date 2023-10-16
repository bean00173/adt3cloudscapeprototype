using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class Door : MonoBehaviour
{
    public GameObject prompt;

    bool active;
    bool canInteract = true;

    // Start is called before the first frame update
    void Start()
    {
        //if(GameManager.instance.currentScene.name == "TowerTest")
        //{
        //    prompt = TowerManager.instance.doorPrompt;
        //    promptMsg = TowerManager.instance.doorPromptText;

        //}

        
    }

    // Update is called once per frame
    void Update()
    {

        if (prompt == null && (this.transform.root.gameObject == TowerManager.instance.currentTower))
        {
            prompt = TowerManager.instance.previousTower.GetComponentInChildren<Door>().prompt;
        }

        //Debug.LogWarning("Door prompt active : " + active + ", Current Scene is : " + GameManager.instance.currentScene.name);

        //if(GameManager.instance.currentScene.name == "TowerTest" && this.GetComponentInParent<TowerData>() != null)
        //{
        //    if (this.GetComponentInParent<TowerData>().towerBeaten == true)
        //    {
        //        canInteract = false;
        //    }
        //}
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
            active = true;

            prompt.SetActive(true);

            TextMeshProUGUI text = prompt.transform.Find("Text").GetComponent<TextMeshProUGUI>();

            if (GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>().enemiesLeft == 0)
            {
                text.text = GameManager.instance.towerFinished == true ? "Leave Tower ?" : "Next Floor ?";
                GameManager.instance.readyToLoad = true;
            }
            else
            {
                GameManager.instance.readyToLoad = false;
                text.text = "You Cannot Leave Until All Enemies Are Defeated!";
            }
        }
        else
        {
            TowerManager.instance.PlayerAtDoor(this.gameObject);
            prompt.GetComponent<TowerPrompt>().SetPromptValues(this.transform.root.GetComponent<TowerData>().tower.difficulty, ReturnEnemyTypes(this.transform.root.GetComponent<TowerData>().tower), GameManager.towersBeaten + 1);
            active = true;
        }
    }

    private List<enemyType> ReturnEnemyTypes(Tower tower)
    {
        List<enemyType> types = new List<enemyType>();

        for(int i = 0; i < tower.floors.Count; i++)
        {
            for(int j = 0; j < tower.floors[i].waves.Count; j++)
            {
                for(int k = 0; k < tower.floors[i].waves[j].enemies.Count; k++)
                {
                    if (!types.Contains(tower.floors[i].waves[j].enemies[k].enemy.GetComponent<EnemyBehaviour>().enemy.enemyType)) types.Add(tower.floors[i].waves[j].enemies[k].enemy.GetComponent<EnemyBehaviour>().enemy.enemyType);
                }
                
            }
        }

        return types;
    }

    private void PlayerNotAtDoor()
    {
        if(GameManager.instance.currentScene.name == "LevelTest")
        {
            prompt.SetActive(false);
        }
        Debug.LogWarning("LEFT DOOR");
        active = false;
        TowerManager.instance.PlayerNotAtDoor(this.gameObject);
    }
}
