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

    public bool atDoor;

    private List<GameObject> currentIslands = new List<GameObject>();

    private GameObject lastDoor;

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
        TowerManager.instance.NextTower();
        TowerManager.instance.NewCurrentTower(TowerManager.instance.GetNextTower());

        GameManager.instance.towerLeft = true;

        airship.GetComponent<AirshipMovement>().enabled = true;
        airship.GetComponent<Collider>().enabled = true;
        airship.GetComponent<AirshipInteraction>().ResetDockStatus();

        Destroy(airship.GetComponent<AirshipInteraction>().character);
        //Destroy(previousTower);

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

    public GameObject GetNextTower(/*GameObject island*/)
    {
        //return currentIslands.Find(item => item == island);
        return currentIslands[towerIndex];
    }

    private void SpawnTowers()
    {
        for(int i = 0; i < GameManager.instance.towerPrefabs.Count; i++)
        {
            Quaternion rotation = Quaternion.Euler(0f, Random.Range(0, 360), 0f);
            bool newPos = false;
            Vector3 newPosition = Vector3.zero;

            while (!newPos)
            {
                newPosition = GameManager.instance.SpawnPosition(Random.Range(6, 18), 24, this.transform.position, currentTower.GetComponent<TowerData>().newIslandMaxRadius * 2);
                if (!TowerManager.instance.IsIslandConflicting(newPosition))
                {
                    newPos = true;
                }
            }

            newPosition = new Vector3(newPosition.x, newPosition.y + (45 * Random.Range(-1, 1)), newPosition.z);

            GameObject newIsland = Instantiate(GameManager.instance.towerPrefabs[i], newPosition, rotation);
            AddIsland(newIsland);
        }
    }

    public void PlayerAtDoor(GameObject door, bool airship = false)
    {
        lastDoor = door;

        if (airship)
        {
            shipPrompt.SetActive(true);

            if (GameManager.instance.currentScene.name == "TowerTest" && doorPromptText != null)
            {
                shipPromptText.text = GameManager.instance.towerFinished ? "Leave?" : "You cannot leave an island unexplored!";
            }
        }
        else
        {

            if (GameManager.instance.currentScene.name == "TowerTest" && doorPromptText != null)
            {
                doorPrompt.SetActive(true);
                doorPrompt.GetComponent<TowerPrompt>().promptText.text = GameManager.instance.towerFinished ? "Beaten." : "Enter ?";
                GameManager.instance.readyToLoad = GameManager.instance.towerFinished ? false : true;
            }
        }
        
    }

    public void PlayerNotAtDoor(GameObject door = null, bool airship = false)
    {
        if(door == lastDoor)
        {
            if (airship)
            {
                shipPrompt.SetActive(false);
            }
            else
            {
                doorPrompt.SetActive(false);
                GameManager.instance.readyToLoad = false;
            }

            lastDoor = null;
        }
    }

}
