using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayableCharacter : MonoBehaviour
{
    public Character.CharacterId currentCharacter;

    CharacterManager manager;

    public float healthModifier;
    public float attackModifier;
    public float speedModifier;
    public float knockBackModifier;

    public Transform characterContainer;
    public int characterIndex = 1;

    private void Start()
    {
        manager = CharacterManager.instance; // gathers instance of character manager
        characterContainer = this.transform.GetChild(0);
    }

    private void Update()
    {
        healthModifier = manager.GetCurrentCharacter(currentCharacter).health; // returns stats from class manager method to return current character based on enum ID
        attackModifier = manager.GetCurrentCharacter(currentCharacter).atkMultiplier;
        speedModifier = manager.GetCurrentCharacter(currentCharacter).spdMultiplier;
        knockBackModifier = manager.GetCurrentCharacter(currentCharacter).knockbackForce;

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
}
