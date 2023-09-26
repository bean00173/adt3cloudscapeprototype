using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemyCombat : Combat
{
    public GameObject _potionBottle;
    public GameObject _forceField;
    public Transform handPos;

    public GameObject fieldHealthUi;
    public LayerMask target;

    public GameObject pointer;

    Rigidbody bottleRb;

    public float throwStrength;
    public float upwardsForce;

    LineRenderer lr;
    public int maxSteps;
    public float timeBetweenSteps;

    GameObject potion;

    RaycastHit hitInfo;

    // Start is called before the first frame update
    public override void Start()
    {
        lr = handPos.GetComponent<LineRenderer>();
        bottleRb = _potionBottle.GetComponent<Rigidbody>();

        if (GameManager.instance.currentScene.name == "TowerTest")
        {
            this.enabled = false;
        }

        base.Start();

        abilityMethod = AlchemyAbility;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            CastArc();
        }
        else
        {
            //lr.enabled = false;
            pointer.SetActive(false);
        }

        

        base.Update();
    }

    private void CastArc()
    {
        pointer.SetActive(true);
        //lr.enabled = true;
        //lr.positionCount = Mathf.CeilToInt(maxSteps / timeBetweenSteps) + 1;

        //Vector3 startPosition = handPos.position;
        //Vector3 startVelocity = throwStrength * pm.orientation.forward + Vector3.up * upwardsForce;

        //int i = 0;
        //lr.SetPosition(i, startPosition);

        //for (float time = 0; time < maxSteps; time += timeBetweenSteps)
        //{
        //    Vector3 point = startPosition + time * startVelocity;
        //    point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y * .5f * Mathf.Pow(time, 2));

        //    lr.SetPosition(i, point);
        //    i++;
        //}

        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, Mathf.Infinity, target);
        Debug.DrawLine(Camera.main.transform.position, hitInfo.point, Color.magenta);
        Debug.LogError(hitInfo.point);

        pointer.transform.position = hitInfo.point;

        
    }

    public void ThrowPotion()
    {
        potion.GetComponent<Collider>().enabled = true;
        potion.transform.parent = null;
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(potion, UnityEngine.SceneManagement.SceneManager.GetSceneByName("LevelTest"));
        potion.GetComponent<Rigidbody>().AddForce(pm.orientation.forward * throwStrength + Vector3.up * upwardsForce, ForceMode.Impulse);

        if (comboIndex == 2) // if combo index already maxed out, reset index
        {
            potion.GetComponent<Potion>().StoreData(type.flame, pc.attackModifier, player);
            comboIndex = 0;
            Debug.Log("Combo Finished");
        }
        else if (!secondAtk || comboIndex == 1) // if combo index not 2 or only on first attack, increase index by one
        {
            potion.GetComponent<Potion>().StoreData(type.explosive, pc.attackModifier, player);
            comboIndex += 1;
        }

        pm.MoveInterrupt(true);
        readyToAtk = true;
    }

    public void CreatePotion()
    {
        
        potion = Instantiate(_potionBottle, handPos.position, Quaternion.identity, handPos.transform);
        potion.GetComponent<Collider>().enabled = false;
        potion.transform.localPosition = Vector3.zero;
    }

    private void AlchemyAbility()
    {
        fieldHealthUi.SetActive(true);
        GameObject field = Instantiate(_forceField, this.transform.position, Quaternion.identity);
        field.GetComponent<ForceField>().healthImg = this.fieldHealthUi;
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(field, UnityEngine.SceneManagement.SceneManager.GetSceneByName("LevelTest"));

        pm.MoveInterrupt(true);
        readyToAtk = true;
    }


}
