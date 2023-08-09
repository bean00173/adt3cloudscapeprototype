using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerData : MonoBehaviour
{
    public Tower tower = new Tower();
    public bool towerBeaten;

    private void Start()
    {
        GameManager.instance.onLevelLoad.AddListener(sendTowerData);
    }

    private void sendTowerData()
    {
        GameManager.instance.StoreTowerData(tower);
    }
}
