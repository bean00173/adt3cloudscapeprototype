using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInterface : MonoBehaviour
{
    public Character.CharacterId currentCharacter;

    // Start is called before the first frame update
    void Start()
    {
        if(currentCharacter != GameManager.activeCharacter.Id)
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
