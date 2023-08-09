using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public CharacterId Id;
    public float health;
    public float atkMultiplier;
    public float spdMultiplier;
    public float knockbackForce;
    public bool isAlive;

    public Character(CharacterId id, float health, float atkMultiplier, float spdMultiplier, float knockbackForce) // constructor
    {
        Id = id;
        this.health = health;
        this.atkMultiplier = atkMultiplier;
        this.spdMultiplier = spdMultiplier;
        this.knockbackForce = knockbackForce;
    }


    public enum CharacterId
    {
        rav,
        abi,
        seb
    }
}
