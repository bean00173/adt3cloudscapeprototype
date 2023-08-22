using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class AirshipInteraction : MonoBehaviour
{
    AirshipMovement am;
    Rigidbody rb;
    bool promptReady;
    bool readyToDock;
    bool docked;
    float leastDist;

    Transform closest;

    Transform currentTower;

    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        am = GetComponent<AirshipMovement>();
        rb = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTower = GameObject.Find("Tower").transform;

        if (docked)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if(readyToDock && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Entering Island");
            Transform container = currentTower.GetComponent<TowerData>().navPointContainer;
            currentTower.GetComponent<TowerData>().AirshipHitbox.enabled = false;

            float duration = 1.0f;
            float time = 0;

            Vector3 target = NavToPoint(container).position;
            Vector3 startPos = this.transform.position;

            while (time < duration)
            {
                this.transform.position = Vector3.Lerp(startPos, target, time / duration);
                time += Time.deltaTime;
            }

            docked = true;

            Quaternion start = transform.rotation;

            Vector3 aimTarget = currentTower.position;
            Vector3 tar = new Vector3(aimTarget.x - this.transform.position.x, 0f, aimTarget.z - this.transform.position.z);
            Quaternion aimDir = Quaternion.LookRotation(tar);
            transform.rotation = Quaternion.Slerp(start, aimDir, Time.deltaTime * 1.0f);

            readyToDock = false;
            promptReady = false;
        }

        if (promptReady)
        {
            GameManager.instance.ReturnUIComponent("PromptParent").GetChild(1).gameObject.SetActive(true);
            readyToDock = true;
        }
        else
        {
            GameManager.instance.ReturnUIComponent("PromptParent").GetChild(1).gameObject.SetActive(false);
        }

        if (Physics.SphereCast(this.transform.position, 3.0f, transform.forward, out hit, 3.0f) && !promptReady)
        {
            if (hit.collider.CompareTag("Tower"))
            {
                am.DisableMovement(true);
                promptReady = true;
            }
            else
            {
                promptReady = false;
            }
        }
    }

    private Transform NavToPoint(Transform container)
    {
        foreach(Transform child in container)
        {
            float dist = Vector3.Distance(this.transform.position, child.transform.position);
            Debug.Log(child.name + " " + dist);
            if (leastDist == 0)
            {
                leastDist = dist;
                closest = child;
            }
            else if(dist < leastDist)
            {
                leastDist = dist;
                closest = child;
            }
            else if(dist == leastDist && child.gameObject.name == "homePoint")
            {
                closest = child;
            }
        }

        return closest;
    }
}
