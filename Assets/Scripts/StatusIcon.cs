using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusIcon : MonoBehaviour
{
    public Character.CharacterId iconCharacter;

    // Start is called before the first frame update
    void Start()
    {
        switch (iconCharacter)
        {
            case Character.CharacterId.seb:
                if(CharacterManager.instance.sebAlive)
                {
                    this.gameObject.SetActive(false);
                }
                break;
            case Character.CharacterId.abi:
                if (CharacterManager.instance.abiAlive)
                {
                    this.gameObject.SetActive(false);
                }
                break;
            case Character.CharacterId.rav:
                if (CharacterManager.instance.ravAlive)
                {
                    this.gameObject.SetActive(false);
                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
