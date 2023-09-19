using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using Cinemachine;

public class PlayableCharacter : MonoBehaviour
{
    public Character.CharacterId currentCharacter;

    public Animator ac;
    Rigidbody rb;

    CharacterManager manager;
    public float healthModifier { get; private set; }
    public float attackModifier { get; private set; }
    public float speedModifier { get; private set; }
    public float abilityCd { get; private set; }
    public float slowMoDuration { get; private set; }
    public int slowMoChance { get; private set; }
    public bool charLoaded { get ; private set; }

    public Transform characterContainer;
    private int characterIndex = 1;

    [HideInInspector]
    public bool canInteract = true;

    bool canSwitch;
    RaycastHit hit;
    GameObject hitObject;
    bool hitting = false;

    private void Start()
    {
        this.transform.parent.GetComponentInChildren<CinemachineFreeLook>().Priority = 20;

        rb = this.GetComponent<Rigidbody>();
        manager = CharacterManager.instance; // gathers instance of character manager
        characterContainer = this.transform.GetChild(0);

        if(GameManager.activeCharacter != null)
        {
            currentCharacter = GameManager.activeCharacter.Id;
        }

        switch(currentCharacter)
        {
            case Character.CharacterId.seb: characterIndex = 0; break;
            case Character.CharacterId.rav: characterIndex = 1; break;
            case Character.CharacterId.abi: characterIndex = 2; break;
        }

        characterContainer.GetChild(characterIndex).gameObject.SetActive(true);

        charLoaded = true;

        ac = characterContainer.GetChild(characterIndex).transform.GetComponent<Animator>();

        GameManager.instance.onLevelLoad.AddListener(SendCharacterData);

    }

    private void Update()
    {
        healthModifier = manager.GetCurrentCharacter(currentCharacter).health; // returns stats from class manager method to return current character based on enum ID
        attackModifier = manager.GetCurrentCharacter(currentCharacter).atkMultiplier;
        speedModifier = manager.GetCurrentCharacter(currentCharacter).spdMultiplier;
        abilityCd = manager.GetCurrentCharacter(currentCharacter).abilityCd;
        slowMoDuration = manager.slowMoDuration;
        slowMoChance = manager.slowMoChance;

        Debug.DrawRay(transform.position + Vector3.up * 0.1f, this.GetComponent<PlayerMovement>().orientation.forward, Color.red, 1f);

        if (GameManager.instance.currentScene.name != "TowerTest")
        {
            canSwitch = false;
        }
        else
        {
            canSwitch = true;
            canInteract = true;
        }

        if (CharacterManager.instance.CharIsAlive(currentCharacter) && canSwitch)
        {
            characterContainer.GetChild(characterIndex).gameObject.SetActive(true);
            currentCharacter = characterContainer.GetChild(characterIndex).GetComponent<CharacterIdentity>().id;
        }
        else if(canSwitch)
        {
            SwitchCharacter();
        }

        //if (GameManager.instance.currentScene.name != "LoadingScene")
        //{
        //    if (Physics.Raycast(transform.position + Vector3.up * 0.1f, this.GetComponent<PlayerMovement>().orientation.forward, out hit, 1f))
        //    {
        //        hit.transform.SendMessage("PlayerNear");
        //        GameManager.instance.ReturnUIComponent("PromptParent").GetChild(0).gameObject.SetActive(true);
        //        GameManager.instance.readyToLoad = true;
        //    }
        //    else
        //    {
        //        GameManager.instance.ReturnUIComponent("PromptParent").GetChild(0).gameObject.SetActive(false);
        //    }


        //}

        DoRaycast();

        if (Input.GetKeyDown(KeyCode.Q) && canSwitch)
        {
            Debug.Log("Switching");
            SwitchCharacter();
        }
    }

    //public void CanMove()
    //{
    //    rb.useGravity = true;
    //    rb.constraints = RigidbodyConstraints.None;

    //    this.GetComponent<PlayerMovement>().enabled = true;
    //    
    //}

    private void SwitchCharacter()
    {
        foreach(Transform character in characterContainer)
        {
            character.gameObject.SetActive(false);
        }

        bool switched = false;

        while (!switched)
        {
            IncrementCharIndex();

            Transform newChar = characterContainer.GetChild(characterIndex);

            if (CharacterManager.instance.CharIsAlive(newChar.GetComponent<CharacterIdentity>().id))
            {
                characterContainer.GetChild(characterIndex).gameObject.SetActive(true);
                currentCharacter = newChar.GetComponent<CharacterIdentity>().id;
                switched = true;
            }
        }

        ac = characterContainer.GetChild(characterIndex).transform.GetComponent<Animator>();

        //ReturnNextAliveCharacter().gameObject.SetActive(true);

    }

    private void IncrementCharIndex()
    {
        if (characterIndex < 2)
        {
            characterIndex++;
        }
        else
        {
            characterIndex = 0;
        }
    }

    public Transform ReturnCurrentCharacter()
    {
        foreach(Transform character in characterContainer)
        {
            if (character.gameObject.activeSelf)
            {
                return character;
            }
        }

        return null;
    }

    public void SendCharacterData()
    {
        GameManager.instance.StoreCharacterData(manager.GetCurrentCharacter(currentCharacter));
    }

    public void DoRaycast()
    {
        if(Physics.Raycast(transform.position + Vector3.up * 0.1f, this.GetComponent<PlayerMovement>().orientation.forward, out hit, 1f))
        {
            if(hit.transform.tag == "Door" || hit.transform.tag == "AirshipDoor")
            {
                GameObject go = hit.transform.gameObject;

                // If no registred hitobject => Entering
                if (hitObject == null)
                {
                    go.SendMessage("PlayerAtDoor");
                }
                // If hit object is the same as the registered one => Stay
                else if (hitObject.GetInstanceID() == go.GetInstanceID())
                {
                    //hitObject.SendMessage("OnHitStay");
                    Debug.Log("Still At Door");
                }
                // If new object hit => Exit last + Enter new
                else
                {
                    hitObject.SendMessage("OnHitExit");
                    go.SendMessage("OnHitEnter");
                }

                hitting = true;
                hitObject = go;
            }
        }
        else if (hitting)
        {
            hitObject.SendMessage("PlayerNotAtDoor");
            hitting = false;
            hitObject = null;
        }
    }

    //public Transform ReturnNextAliveCharacter()
    //{
    //    foreach(Transform character in characterContainer)
    //    {

    //    }
    //}
}
