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

    bool listenersAdded;
    List<Transform> shipDoors = new List<Transform>();

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
    }

    // Update is called once per frame
    void Update()
    {
        if(previousTower != currentTower && !listenersAdded)
        {
            foreach(Transform door in shipDoors)
            {
                door.GetComponent<AirshipDoor>().leaveIsland.AddListener(LeaveTower);
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
        shipDoors = currentTower.GetComponentsInChildren<AirshipDoor>().Select(ad => ad.transform).ToList();
    }

    public void LeaveTower()
    {
        GameManager.instance.towerLeft = true;

        airship.GetComponent<AirshipMovement>().enabled = true;
        airship.GetComponent<AirshipInteraction>().ResetDockStatus();

        Destroy(airship.GetComponent<AirshipInteraction>().character);
        Destroy(previousTower);

        airship.GetComponent<AirshipInteraction>().dockPrompt.parent.gameObject.SetActive(true);
    }
}
