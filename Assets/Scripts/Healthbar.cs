using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public EnemyBehaviour eb;
    public bool functional;
    public Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(functional) // scales the health bar to be a % of original size based on % of health remaining
        {
            this.transform.GetChild(1).GetComponent<Image>().fillAmount = eb.GetHealthPercentage();
        }

        this.transform.position = eb.transform.position + new Vector3(0, 2.5f, 0);

    }

    private void LateUpdate()
    {
        //this.transform.rotation = cam.rotation;

        Vector3 delta = new Vector3(cam.position.x - this.transform.position.x, 0f, cam.position.z - this.transform.position.z);  // calculate x/z position difference between agent and player
        transform.rotation = Quaternion.LookRotation(delta); // create new target location based off of x/z diff

    }


}
