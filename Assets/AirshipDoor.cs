using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class AirshipDoor : MonoBehaviour
{
    private GameObject prompt;
    private TextMeshProUGUI promptMsg;
    
    public UnityEvent leaveIsland;
    

    bool active;
    bool canInteract = false;

    // Start is called before the first frame update
    void Start()
    {
        prompt = TowerManager.instance.shipPrompt;
        promptMsg = TowerManager.instance.shipPromptText;
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(active);

        if (GameManager.instance.currentScene.name == "TowerTest")
        {
            if (this.GetComponentInParent<TowerData>().towerBeaten == true)
            {
                canInteract = true;
            }
        }
        //else if (GameManager.instance.currentScene.name == "LevelTest")
        //{
        //    canInteract = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>().enemiesLeft == 0 ? true : false;
        //}


        prompt.SetActive(active);

        if (GameManager.instance.currentScene.name == "TowerTest" && promptMsg != null)
        {
            promptMsg.text = GameManager.instance.towerFinished ? "Leave ?" : "You cannot leave a tower unexplored!";
        }


        if (active)
        {
            if (Input.GetKeyDown(KeyCode.E) && canInteract)
            {
                this.enabled = false;
                leaveIsland.Invoke();

                prompt.SetActive(false);
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
