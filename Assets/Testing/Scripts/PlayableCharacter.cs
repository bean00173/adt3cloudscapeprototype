using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacter : MonoBehaviour
{
    public Character.CharacterId currentCharacter;

    CharacterManager manager;

    public float healthModifier;
    public float attackModifier;
    public float speedModifier;

    private void Start()
    {
        manager = CharacterManager.instance;

        
    }

    private void Update()
    {
        healthModifier = manager.GetCurrentCharacter(currentCharacter).health;
        attackModifier = manager.GetCurrentCharacter(currentCharacter).atkMultiplier;
        speedModifier = manager.GetCurrentCharacter(currentCharacter).spdMultiplier;
    }
}