using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public CharacterId Id;
    public float health;
    public float atkMultiplier;
    public float spdMultiplier;

    public Character(CharacterId id, float health, float atkMultiplier, float spdMultiplier) // constructor
    {
        Id = id;
        this.health = health;
        this.atkMultiplier = atkMultiplier;
        this.spdMultiplier = spdMultiplier;
    }


    public enum CharacterId
    {
        rav,
        abi,
        seb
    }
}
