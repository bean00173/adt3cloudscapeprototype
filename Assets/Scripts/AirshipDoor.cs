using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

[RequireComponent(typeof(AudioSource))]
public class AirshipDoor : MonoBehaviour
{
    private GameObject prompt;
    private TextMeshProUGUI promptMsg;
    
    public UnityEvent leaveIsland;
    
    

    bool active;
    bool canInteract = false;
    AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        prompt = TowerManager.instance.shipPrompt;
        promptMsg = TowerManager.instance.shipPromptText;
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

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


        //prompt.SetActive(active);

        //if (GameManager.instance.currentScene.name == "TowerTest" && promptMsg != null)
        //{
        //    promptMsg.text = GameManager.instance.towerFinished ? "Leave ?" : "You cannot leave an island unexplored!";
        //}


        if (active)
        {
            if (Input.GetKeyDown(KeyCode.E) && canInteract)
            {
                source.clip = AudioManager.instance.GetClip(AudioType.effects, "Airship_Bell");
                source.Play();

                this.enabled = false;
                leaveIsland.Invoke();

                prompt.SetActive(false);
            }
        }
    }

    private void PlayerAtDoor()
    {
        TowerManager.instance.PlayerAtDoor(this.gameObject, true);
        active = true;
    }

    private void PlayerNotAtDoor()
    {
        Debug.LogWarning("LEFT DOOR");
        active = false;
        TowerManager.instance.PlayerNotAtDoor(this.gameObject, true);
    }
}
