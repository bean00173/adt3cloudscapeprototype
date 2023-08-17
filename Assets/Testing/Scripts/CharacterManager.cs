using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using static Character;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;

    [Header("Ravi Stats")]
    public float ravHealth;
    public float ravAtk;
    public float ravSpeed;
    public float ravAbDuration;
    public bool ravAlive = true;

    [Header("Abigail Stats")]
    public float abiHealth;
    public float abiAtk;
    public float abiSpeed;
    public float abiAbDuration;
    public bool abiAlive = true;

    [Header("Sebastian Stats")]
    public float sebHealth;
    public float sebAtk;
    public float sebSpeed;
    public float sebAbDuration;
    public bool sebAlive = true;

    [Header("Universal TimeSlow Chance")]
    public int timeSlowChance;

    public List<Character> characters {  get; private set; } = new List<Character>();

    // Make this a singleton.
    public void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start() // premade constructors for each individual character and their statistics (defined in inspector), adds all to list
    {
        AddCharacter(new Character(Character.CharacterId.rav, ravHealth, ravAtk, ravSpeed, ravAbDuration, ravAlive));
        AddCharacter(new Character(Character.CharacterId.abi, abiHealth, abiAtk, abiSpeed, abiAbDuration, abiAlive));
        AddCharacter(new Character(Character.CharacterId.seb, sebHealth, sebAtk, sebSpeed, sebAbDuration, sebAlive));
    }

    private void AddCharacter(Character character)
    {
        characters.Add(character);
    }

    public Character GetCurrentCharacter(CharacterId id) // return currently active character
    {
        return characters.Where(i => i.Id == id).FirstOrDefault();
    }
}
