using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerData : MonoBehaviour
{
    public Tower tower = new Tower();
    public bool towerBeaten;

    public Transform navPointContainer;
    public Collider AirshipHitbox;

    private void Start()
    {
        GameManager.instance.onLevelLoad.AddListener(sendTowerData);
    }

    private void Update()
    {
        if(GameManager.instance.towerFinished && GameManager.instance.ReturnTowerData() == this.tower)
        {
            towerBeaten = true;

            Quaternion rotation = Quaternion.Euler(0f, Random.Range(0, 360), 0f);
            GameObject newTower = Instantiate(GameManager.instance.NextTower(), GameManager.instance.SpawnPosition(Random.Range(0, 24), 24, this.transform.position, 5000f), rotation);

            // DEMO ONLY FUNCTION

            //GameManager.instance.ResetGame();

            //UnityEngine.SceneManagement.SceneManager.LoadScene("GameWin");

        }
    }

    private void sendTowerData()
    {
        GameManager.instance.StoreTowerData(tower);
    }
}
