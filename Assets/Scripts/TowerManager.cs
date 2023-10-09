using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class TowerManager : MonoBehaviour
{
    public static TowerManager instance;

    public GameObject previousTower { get; private set; }
    public GameObject currentTower { get; private set; }

    public GameObject shipPrompt, doorPrompt;
    public TextMeshProUGUI shipPromptText, doorPromptText;

    public GameObject airship;
    public GameObject cloudImage;

    bool listenersAdded;
    List<Transform> shipDoors = new List<Transform>();

    public int towerIndex { get; private set; }
    public float islandBoundaryRadius;

    private List<GameObject> currentIslands = new List<GameObject>();

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        currentTower = GameObject.Find("Tower");
        shipDoors = currentTower.GetComponentsInChildren<AirshipDoor>(true).Select(ad => ad.transform).ToList();

        SpawnTowers();
    }

    // Update is called once per frame
    void Update()
    {
        if(previousTower != currentTower && !listenersAdded)
        {
            Debug.Log("Grabbing Doors");
            foreach(Transform door in shipDoors)
            {
                door.GetComponent<AirshipDoor>().leaveIsland.AddListener(LeaveTower);
                Debug.Log(door.name);
            }

            listenersAdded = true;
        }
    }

    public void NewCurrentTower(GameObject tower)
    {
        previousTower = currentTower;
        currentTower = tower;

        listenersAdded = false;

        shipDoors = null;
        shipDoors = currentTower.GetComponentsInChildren<AirshipDoor>(true).Select(ad => ad.transform).ToList();

    }

    public void LeaveTower()
    {
        GameManager.instance.towerLeft = true;

        airship.GetComponent<AirshipMovement>().enabled = true;
        airship.GetComponent<AirshipInteraction>().ResetDockStatus();

        Destroy(airship.GetComponent<AirshipInteraction>().character);
        Destroy(previousTower);

        airship.GetComponent<AirshipInteraction>().dockPrompt.transform.parent.gameObject.SetActive(true);
    }


    public void CloudTransition()
    {
        cloudImage.SetActive(true);
    }

    public void AddIsland(GameObject island)
    {
        currentIslands.Add(island);
    }

    public void NextTower()
    {
        towerIndex++;
    }
    
    public bool IsIslandConflicting(Vector3 spawnPos)
    {
        foreach(GameObject island in currentIslands)
        {
            if(Vector3.Distance(spawnPos, island.transform.position) < islandBoundaryRadius || Vector3.Distance(spawnPos, airship.transform.position) < islandBoundaryRadius)
            {
                return true;
            }
        }

        return false;
    }

    public GameObject GetNextTower(GameObject island)
    {
        return currentIslands.Find(item => item == island);
    }

    private void SpawnTowers()
    {
        for(int i = 0; i < GameManager.instance.towerPrefabs.Count - 1; i++)
        {
            Quaternion rotation = Quaternion.Euler(0f, Random.Range(0, 360), 0f);
            bool newPos = false;
            Vector3 newPosition = Vector3.zero;

            while (!newPos)
            {
                newPosition = GameManager.instance.SpawnPosition(Random.Range(0, 24), 24, this.transform.position, currentTower.GetComponent<TowerData>().newIslandMaxRadius * 2);
                if (!TowerManager.instance.IsIslandConflicting(newPosition))
                {
                    newPos = true;
                }
            }

            newPosition = new Vector3(newPosition.x, newPosition.y + (45 * Random.Range(-1, 1)), newPosition.z);

            GameObject newIsland = Instantiate(GameManager.instance.towerPrefabs[i], newPosition, rotation);
            TowerManager.instance.AddIsland(newIsland);
        }
    }
}
