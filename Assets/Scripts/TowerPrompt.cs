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
        TowerBeaten(TowerManager.instance.currentTower.GetComponent<TowerData>().towerBeaten);
    }

    public void TowerBeaten(bool beaten)
    {
        beatenImage.SetActive(beaten);
    }

    public void SetPromptValues(int difficulty, List<enemyType> types, int number)
    {
        difficultyImage.fillAmount = .2f * difficulty;
        titleText.text = number.ToString();
        foreach(Transform img in enemyTypeContainer.transform)
        {
            if (img.transform.GetComponent<EnemyIdentity>()) img.gameObject.SetActive(types.Contains(img.GetComponent<EnemyIdentity>().type));
            else img.gameObject.SetActive(false);
        }

    }

    
}
