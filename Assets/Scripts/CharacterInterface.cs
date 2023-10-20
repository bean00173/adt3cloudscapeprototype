using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInterface : MonoBehaviour
{
    public Character.CharacterId currentCharacter;
    public PlayableCharacter character;

    private void OnEnable()
    {
        character = GameObject.FindObjectOfType<PlayableCharacter>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (currentCharacter != character.currentCharacter)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.currentScene.name == "TowerTest")
        {
            if (currentCharacter != character.currentCharacter)
            {
                foreach(Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
            else
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
    }

    public void Fell()
    {
        this.transform.parent.gameObject.SetActive(false);
    }
}
