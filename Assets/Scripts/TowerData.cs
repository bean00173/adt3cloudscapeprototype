using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerData : MonoBehaviour
{
    public Tower tower = new Tower();
    public bool towerBeaten;

    public Transform navPointContainer;
    public Collider AirshipHitbox;

    public bool active;
    bool spawned;

    public float newIslandMaxRadius = 2000f;
    public float newIslandMinRadius = 500f;

    private void Start()
    {
        TowerManager.instance.AddIsland(this.gameObject);

        //SpawnMinorTowers();
    }

    private void Update()
    {
        if(TowerManager.instance.currentTower == this.gameObject)
        {
            GameManager.instance.onLevelLoad.AddListener(SendTowerData);
            active = true;
        }

        if (GameManager.instance.towerFinished && GameManager.instance.ReturnTowerData() == this.tower && !spawned)
        {
            towerBeaten = true;

            //Quaternion rotation = Quaternion.Euler(0f, Random.Range(0, 360), 0f);

            TowerManager.instance.NextTower();
            TowerManager.instance.NewCurrentTower(TowerManager.instance.GetNextTower());

            //GameObject newTower = Instantiate(GameManager.instance.NextTower(), GameManager.instance.SpawnPosition(Random.Range(0, 24), 24, this.transform.position, 2 * newIslandMaxRadius), rotation);

            //TowerManager.instance.NewCurrentTower(newTower);

            spawned = true;

            // DEMO ONLY FUNCTION

            //GameManager.instance.ResetGame();

            //UnityEngine.SceneManagement.SceneManager.LoadScene("GameWin");

        }
    }

    private void SendTowerData()
    {
        GameManager.instance.StoreTowerData(tower);
    }

    private void SpawnMinorTowers()
    {
        GameObject[] potentialNewIslands = Resources.LoadAll<GameObject>("Islands");

        for (int i = 0; i < tower.bgIslandCount; i++)
        {
            Quaternion rotation = Quaternion.Euler(0f, Random.Range(0, 360), 0f);

            bool newPos = false;
            Vector3 newPosition = Vector3.zero;

            while (!newPos)
            {
                newPosition = GameManager.instance.SpawnPosition(Random.Range(0, 24), 24, this.transform.position, Random.Range(newIslandMinRadius, newIslandMaxRadius));
                if (!TowerManager.instance.IsIslandConflicting(newPosition))
                {
                    newPos = true;
                }
            }

            newPosition = new Vector3(newPosition.x, newPosition.y + (250 * Random.Range(-1, 1)), newPosition.z);

            GameObject newIsland = Instantiate(potentialNewIslands[Random.Range(0, potentialNewIslands.Length)], newPosition, rotation);
            TowerManager.instance.AddIsland(newIsland);
        }
    }
}
