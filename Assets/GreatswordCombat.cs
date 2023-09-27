using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatswordCombat : Combat
{
    public Transform hitContainer;
    List<Collider> colliders = new List<Collider>(); 
    Collider hit;

    bool readyToHold = true;
    bool holding;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        abilityMethod = GreatSwordAbility; // gs

        foreach (Collider col in hitContainer.GetComponentsInChildren<Collider>()) // greatsword
        {
            colliders.Add(col);
            col.enabled = false; // disables collider and enables as trigger
            col.isTrigger = true;
        }

    }

    // Update is called once per frame
    public override void Update()
    {
        hit = colliders[comboIndex]; // greatsword

        base.Update();
    }

    private void GreatSwordAbility() // gs
    {
        BodyPartContainer bpc = GameObject.Find("BodyParts").GetComponent<BodyPartContainer>();
        foreach (Transform child in bpc.gameObject.transform)
        {
            if (child.gameObject.tag == "HealthOrb")
            {
                StartCoroutine(OrbMagnet(child.gameObject));
            }
        }


    }

    IEnumerator SpinAttackMov() //gs
    {
        while (holding)
        {
            yield return new WaitForSeconds(.25f);
            rb.AddForce(Random.onUnitSphere * 10f * pm.walkMultiplier * pc.speedModifier, ForceMode.Force);
        }
    }

    public void HitOn() //gs
    {
        hit.enabled = true;

        this.GetComponent<CinemachineImpulseSource>().GenerateImpulseWithForce((comboIndex + 1));

        //StartCoroutine(GameManager.instance.DoCameraShake(shakeManager.GetComponentInChildren<CinemachineFreeLook>(), .25f));
    }

    public void HitOff() //gs
    {
        hit.enabled = false;

        if (comboIndex == 2) // if combo index already maxed out, reset index
        {
            comboIndex = 0;
            Debug.Log("Combo Finished");
        }
        else /*if (!secondAtk || comboIndex == 1)*/ // if combo index not 2 or only on first attack, increase index by one
        {
            comboIndex += 1;
        }
        pm.MoveInterrupt(true); // re-enables movement
        readyToAtk = true; // re-enables attack capability
        readyToHold = true; // re enable special atk capability

        Debug.LogError(comboIndex);
    }

    private IEnumerator OrbMagnet(GameObject go) //gs
    {
        float duration = 1;

        float time = 0;
        Vector3 startPosition = go.transform.position;
        while (time < duration)
        {
            if (go == null)
            {
                time = duration;
                break;
            }
            go.transform.position = Vector3.Lerp(startPosition, this.transform.position, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        if (go != null)
        {
            go.transform.position = this.transform.position;
        }
        else
        {
            yield return null;
        }

    }

}
