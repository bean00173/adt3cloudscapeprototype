using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayableCharacter : MonoBehaviour
{
    public Character.CharacterId currentCharacter;

    CharacterManager manager;
    public float healthModifier { get; private set; }
    public float attackModifier { get; private set; }
    public float speedModifier { get; private set; }
    public float knockBackModifier { get; private set; }

    public Transform characterContainer;
    private int characterIndex = 1;

    private void Start()
    {
        manager = CharacterManager.instance; // gathers instance of character manager
        characterContainer = this.transform.GetChild(0);

        switch(currentCharacter)
        {
            case Character.CharacterId.seb: characterIndex = 0; break;
            case Character.CharacterId.rav: characterIndex = 1; break;
            case Character.CharacterId.abi: characterIndex = 2; break;
        }

        characterContainer.GetChild(characterIndex).gameObject.SetActive(true);

        GameManager.instance.onLevelLoad.AddListener(SendCharacterData);
    }

    private void Update()
    {
        healthModifier = manager.GetCurrentCharacter(currentCharacter).health; // returns stats from class manager method to return current character based on enum ID
        attackModifier = manager.GetCurrentCharacter(currentCharacter).atkMultiplier;
        speedModifier = manager.GetCurrentCharacter(currentCharacter).spdMultiplier;
        knockBackModifier = manager.GetCurrentCharacter(currentCharacter).knockbackForce;

        Debug.DrawRay(transform.position + Vector3.up * 0.1f, this.GetComponent<PlayerMovement>().orientation.forward, Color.red, 1f);

        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, this.GetComponent<PlayerMovement>().orientation.forward, 1f, LayerMask.GetMask("Door")))
        {
            GameManager.instance.promptText.SetActive(true);
            GameManager.instance.readyToLoad = true;
        }
        else
        {
            GameManager.instance.promptText.SetActive(false);
        }
        

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Switching");
            SwitchCharacter();
        }
    }

    private void SwitchCharacter()
    {
        foreach(Transform character in characterContainer)
        {
            character.gameObject.SetActive(false);
        }
        if(characterIndex < 2)
        {
            characterIndex++;
        }
        else
        {
            characterIndex = 0;
        }

        characterContainer.GetChild(characterIndex).gameObject.SetActive(true);
        //ReturnNextAliveCharacter().gameObject.SetActive(true);
                
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

    //public Transform ReturnNextAliveCharacter()
    //{
    //    foreach(Transform character in characterContainer)
    //    {

    //    }
    //}
}
