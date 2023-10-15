using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerPrompt : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    public Image difficultyImage;
    public Transform enemyTypeContainer;
    public TextMeshProUGUI titleText;
    public GameObject beatenImage;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPromptValues(string text, int difficulty, enemyType[] types, int number, bool beaten)
    {
        // enter details
    }

    
}
