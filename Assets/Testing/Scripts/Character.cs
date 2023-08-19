using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public CharacterId Id;
    public float health;
    public float atkMultiplier;
    public float spdMultiplier;
    public float abilityCd;
    public bool isAlive;

    public Character(CharacterId id, float health, float atkMultiplier, float spdMultiplier, float abilityCd, bool alive) // constructor
    {
        Id = id;
        this.health = health;
        this.atkMultiplier = atkMultiplier;
        this.spdMultiplier = spdMultiplier;
        this.abilityCd = abilityCd;
        this.isAlive = alive;
    }


    public enum CharacterId
    {
        seb,
        rav,
        abi
    }
}
