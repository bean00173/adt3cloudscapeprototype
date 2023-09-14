using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Transform _small, _genericLow, _genericMidBot, _medTop, _tallMidHigh, _tallTop, _kingChamber;

    int floor;
    towerType type;
    
    
    // Start is called before the first frame update
    void Start()
    {
        floor = GameManager.floorIndex;
        type = GameManager.towerData.type;

        ActivateLevel();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ActivateLevel()
    {
        switch (type)
        {
            case towerType.small:
                _small.gameObject.SetActive(true);
                break;
            case towerType.medium:
                if(floor < 2)
                {
                    LevelSwitcher();
                }
                else
                {
                    _medTop.gameObject.SetActive(true);
                }
                break;
            case towerType.large:
                LevelSwitcher();
                break;
        }
    }

    private void LevelSwitcher()
    {
        switch (floor)
        {
            case 0:
                _genericLow.gameObject.SetActive(true);
                break;
            case 1:
                _genericMidBot.gameObject.SetActive(true);
                break;
            case 2:
                _tallMidHigh.gameObject.SetActive(true);
                break;
            case 3:
                _tallTop.gameObject.SetActive(true);
                break;
            case 4:
                _kingChamber.gameObject.SetActive(true);
                break;
        }
    }
}
