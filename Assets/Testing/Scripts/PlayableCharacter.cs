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

    private void Start()
    {
        manager = CharacterManager.instance; // gathers instance of character manager
    }

    private void Update()
    {
        healthModifier = manager.GetCurrentCharacter(currentCharacter).health; // returns stats from class manager method to return current character based on enum ID
        attackModifier = manager.GetCurrentCharacter(currentCharacter).atkMultiplier;
        speedModifier = manager.GetCurrentCharacter(currentCharacter).spdMultiplier;
        knockBackModifier = manager.GetCurrentCharacter(currentCharacter).knockbackForce;
    }
}
